﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace KMWeb.Administration
{
    public partial class NewRevision : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        List<Clanak> Sadrzaj = new List<Clanak>();
        Clanak clanak = new Clanak();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string Article = Request.QueryString["ArticleId"];
            string username = (string)Session["korisnickoIme"];
            if (username != null)
            {
                
            DataSet ds = new DataSet();
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT Clanci.Id As Id, Clanci.Naslov AS Naslov, Clanci.Sadrzaj AS Sadrzaj, Clanci.DatumKreiranja AS DatumKreiranja, Korisnici.Id AS IdKorisnik, Korisnici.Ime, Korisnici.Prezime, Korisnici.KorisnickoIme"
                         + " FROM Clanci INNER JOIN Korisnici ON Clanci.IdKorisnik = Korisnici.Id WHERE Clanci.Id=" + Article, connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtNaslov.Text = reader["Naslov"].ToString();
                txtOriginialSadrzaj.Text = reader["Sadrzaj"].ToString();

                clanak.Sadrzaj = reader["Sadrzaj"].ToString();
                clanak.Naslov = reader["Naslov"].ToString();
                Sadrzaj.Add(clanak);
                Repeater1.DataSource = Sadrzaj;
                Repeater1.DataBind();
                reader.Close();
                connection.Close();
            }

            //BindData(Convert.ToInt32(Article));
            connection.Close();
            if (!IsPostBack)
            LoadPrijedlog();
            }
            else{
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logirajte se !');window.location.href = '../Account/Login.aspx'", true);
            }

        }

        private void LoadPrijedlog()
        {
            txtPrijedlogNaslova.Text = txtNaslov.Text;
            txtPrijedlogSadrzaja.Text = txtOriginialSadrzaj.Text;
        }


        protected void btnPrijedlog_Click(object sender, EventArgs e)
        {
            try
            {
                string Article = Request.QueryString["ArticleId"];
                SqlCommand cmd = new SqlCommand("INSERT INTO PrijeglogRevizije (IdClanak, Naslov, Sadrzaj, DatumPrijedloga, IdKorisnik, Status) VALUES (@IdClanak, @Naslov, @Sadrzaj, @DatumPrijedloga, @IdKorisnik, @Status); SELECT SCOPE_IDENTITY();");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@IdClanak", Convert.ToInt32(Article));
                cmd.Parameters.AddWithValue("@Naslov", txtPrijedlogNaslova.Text);
                cmd.Parameters.AddWithValue("@Sadrzaj", txtPrijedlogSadrzaja.Text);
                cmd.Parameters.AddWithValue("@IdKorisnik", Convert.ToInt32(Session["UserId"]));
                cmd.Parameters.AddWithValue("@DatumPrijedloga", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@Status", "1"); //1 = Predlozeno

                connection.Open();
                cmd.ExecuteNonQuery();
               // LastId = Convert.ToInt16(cmd.ExecuteScalar());

              //  MessageBox.Show("Prijedlog unesen. Vlasnik članka će razmotriti prijedlog izmjena.", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Prijedlog unesen. Vlasnik članka će razmotriti prijedlog izmjena.');", true);
                
                connection.Close();
                
            }
            catch (Exception ex) { 
                //MessageBox.Show("GREŠKA, Prijedlog NIJE unesen!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('GREŠKA, Prijedlog NIJE unesen!');", true);
            }

           // ClearFields();
        }

        protected void ClearFields()
        {
            txtPrijedlogNaslova.Text = "";
            txtPrijedlogSadrzaja.Text = "";
            
            connection.Close();

        }
    }
}