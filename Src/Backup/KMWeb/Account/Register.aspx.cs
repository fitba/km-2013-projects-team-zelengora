﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace KMWeb.Account
{
    public partial class Register : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);

        protected void UpdateKorisniciKategorijeClanaka(int UserId)
        {
            DataSet ds = new DataSet();

            SqlCommand cmdKat = new SqlCommand("Select Id,NazivKategorije from KategorijeClanaka", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmdKat);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            da.Fill(ds, "Kategorije");
            connection.Close(); 

            // Za svaku kategoriju inicijalizirati FALSE kad kreiraram novog korisnika
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int IdKategorije = (int) ds.Tables[0].Rows[i]["Id"];
                try
                {
                    SqlCommand cmd = new SqlCommand("Insert into KorisniciKategorijeClanaka(IdKorisnik,IdKategorijaClanaka,Preferira) VALUES (@IdKorisnik,@IdKategorijaClanaka,@Preferira)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@IdKorisnik", UserId);
                    cmd.Parameters.AddWithValue("@IdKategorijaClanaka", IdKategorije);
                    cmd.Parameters.AddWithValue("@Preferira", false);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex) { }
            }
        }

        protected int insertUser(string username, string password, string ime, string prezime)
        {
            int IdUser=0;

            string _username = username;
            string _password = password;
            string _ime = ime;
            string _prezime = prezime;
            string korisnicko = null;
            SqlCommand cmd;
            SqlParameter userNameParam;
            SqlParameter userNameParam1;
            SqlParameter userNameParam2;
            SqlParameter userNameParam3;
                   
            if ((txtIme.Text != "") && (txtPrezime.Text != "") && (UserName.Text != "") &&  (Password.Text != "") && (Password.Text == ConfirmPassword.Text))
            {
                try
                {
                    connection.Open();
                    cmd = new SqlCommand("Select Id, KorisnickoIme From Korisnici Where " + // Da li korisnik e postoji
                                   "KorisnickoIme = @username", connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                     userNameParam = cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25 /* max length of field */ );
                     userNameParam1 = cmd.Parameters.Add("@Ime", SqlDbType.VarChar, 25 /* max length of field */ );
                     userNameParam2 = cmd.Parameters.Add("@Prezime", SqlDbType.VarChar, 25 /* max length of field */ );
                     userNameParam3 = cmd.Parameters.Add("@Password", SqlDbType.VarChar, 25 /* max length of field */ );
                    userNameParam.Value = _username;
                    userNameParam1.Value = _ime;
                    userNameParam2.Value = _prezime;
                    userNameParam3.Value = _password;


                    SqlDataAdapter daKorisnik = new SqlDataAdapter(cmd);
                    SqlDataReader readerKorisnik = cmd.ExecuteReader();
                    
                    while (readerKorisnik.Read())
                    {
                        korisnicko = readerKorisnik["KorisnickoIme"].ToString();
                        IdUser = Convert.ToInt32(readerKorisnik["Id"]);
                    }
                    readerKorisnik.Close();
                }
                catch (Exception ex) { } 

                if (korisnicko == null)// Ako ne postoji INSERT
                {
                    try
                    {
                        cmd = new SqlCommand("Insert INTO Korisnici (KorisnickoIme, Ime, Prezime, IdGrupe, Lozinka) Values " +
                            "(@UserName,@Ime,@Prezime,1,@Password) ", connection);

                        userNameParam = cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25 /* max length of field */ );
                        userNameParam1 = cmd.Parameters.Add("@Ime", SqlDbType.VarChar, 25 /* max length of field */ );
                        userNameParam2 = cmd.Parameters.Add("@Prezime", SqlDbType.VarChar, 25 /* max length of field */ );
                        userNameParam3 = cmd.Parameters.Add("@Password", SqlDbType.VarChar, 25 /* max length of field */ );
                        userNameParam.Value = _username;
                        userNameParam1.Value = _ime;
                        userNameParam2.Value = _prezime;
                        userNameParam3.Value = _password;
                        cmd.ExecuteNonQuery();


                        cmd = new SqlCommand("Select Id, KorisnickoIme, Ime, Prezime From Korisnici Where " + // Spremam u SESSION zadnjeg registrovanog korisnika i idem na pocetnu stranicu ili detalje o korisniku
                              "KorisnickoIme = @UserName", connection);
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                        userNameParam = cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25 /* max length of field */ );
                        userNameParam.Value = _username;

                        SqlDataReader readerKorisnik1 = cmd.ExecuteReader();
                        korisnicko = null;
                        while (readerKorisnik1.Read())
                        {
                            ime = readerKorisnik1["Ime"].ToString();
                            prezime = readerKorisnik1["Prezime"].ToString();
                            korisnicko = readerKorisnik1["KorisnickoIme"].ToString();
                            IdUser = Convert.ToInt32(readerKorisnik1["Id"]);
                            Session["ime"] = ime;
                            Session["prezime"] = prezime;
                            Session["korisnickoIme"] = korisnicko;
                            Session["UserId"] = IdUser;
                        }
                        readerKorisnik1.Close();

                        //NAKON KREIRANJA NOVOG KORISNIKA Forsiraj na false sve preferirane kategorije
                        UpdateKorisniciKategorijeClanaka(IdUser); 

                        connection.Close();
                        Response.Redirect("~/Administration/UserDetails.aspx");
                    }
                    catch (Exception ex) { }
                }
                else { //MessageBox.Show("Korisničko ime zauzeto"); 
                    ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Korisničko ime zauzeto!');", true); }
            }
            else
            { 
                //MessageBox.Show("Greska. Ispravno unijeti podatke. Provjeriti Unos Lozinke");
            ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Greska. Ispravno unijeti podatke. Provjeriti Unos Lozinke');", true);
            }//Kraj IF
            return IdUser;
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
           // RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        

        protected void CreateUserButton_Click(object sender, EventArgs e)
        {
            insertUser(UserName.Text, Password.Text, txtIme.Text, txtPrezime.Text);
        }

    }
}
