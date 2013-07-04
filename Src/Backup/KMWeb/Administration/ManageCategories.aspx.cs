using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace KMWeb.Administration
{
    public partial class ManageCategories : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
       
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = (string)Session["korisnickoIme"];
            if (username == null)
            {
                //MessageBox.Show("Logirajte se !");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logirajte se !');window.location.href = '../Account/Login.aspx'", true);
                //Response.Redirect("~/Account/Login.aspx");
            }
        }
 
        private void clear()
        {
            txtNazivKategorije.Text = "";
            txtOpisKategorije.Text = "";
        }
        protected void UpdateKorisniciKategorijeClanaka(int IdKategorija)
        {
            int _IdKategorija = IdKategorija;
            DataSet ds = new DataSet();

            SqlCommand cmdKat = new SqlCommand("Select Id from Korisnici", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmdKat);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            da.Fill(ds, "Kategorije");
            connection.Close();

            // Za svakog korisnika inicijalizirati FALSE kad kreiraram novu kategoriju
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int UserId = (int)ds.Tables[0].Rows[i]["Id"];
                try
                {
                    SqlCommand cmd = new SqlCommand("Insert into KorisniciKategorijeClanaka(IdKorisnik,IdKategorijaClanaka,Preferira) VALUES (@IdKorisnik,@IdKategorijaClanaka,@Preferira)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@IdKorisnik", UserId);
                    cmd.Parameters.AddWithValue("@IdKategorijaClanaka", _IdKategorija);
                    cmd.Parameters.AddWithValue("@Preferira", false);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex) { }
            }
        }


        protected void btnNovaKategorija_Click(object sender, EventArgs e)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            SqlParameter Kategorija;
            string kat="";
            int IdKategorije=0;
            SqlCommand cmd;
            SqlParameter userNameParam;
            if (txtNazivKategorije.Text != "") // Polje naziv kategorije mora biti popunjeno
            {
                //Provjeravam da li ime kategorije vec postoji

                try
                {
                    connection.Open();
                    cmd = new SqlCommand("Select Id, NazivKategorije From KategorijeClanaka Where " + // Da li korisnik e postoji
                                   "NazivKategorije = @NazivKategorije", connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    userNameParam = cmd.Parameters.Add("@NazivKategorije", SqlDbType.VarChar, 25 /* max length of field */ );
                    userNameParam.Value = txtNazivKategorije.Text;
                    SqlDataAdapter saKat = new SqlDataAdapter(cmd);
                    SqlDataReader readerKat = cmd.ExecuteReader();

                    while (readerKat.Read())
                    {
                        kat = readerKat["NazivKategorije"].ToString();
                    }
                    readerKat.Close();
                    connection.Close();
                }
                catch (Exception ex) { }

               if (kat=="")
               {
            
                try
                {
                    cmd = new SqlCommand("Insert into KategorijeClanaka(NazivKategorije,OpisKategorije,DatumKreiranja,IdKorisnik) VALUES (@NazivKategorije,@OpisKategorije,@DatumKreiranja,@IdKorisnik)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    
                    cmd.Parameters.AddWithValue("@NazivKategorije", txtNazivKategorije.Text );
                    cmd.Parameters.AddWithValue("@OpisKategorije", txtOpisKategorije.Text);
                    cmd.Parameters.AddWithValue("@DatumKreiranja", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdKorisnik", UserId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                      
                   // MessageBox.Show("Kategorija unesena !", "Important Message");
                    ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Kategorija unesena !');", true);

                    cmd = new SqlCommand("Select Id,NazivKategorije From KategorijeClanaka Where " + // Spremam u SESSION zadnjeg registrovanog korisnika i idem na pocetnu stranicu ili detalje o korisniku
                              "NazivKategorije = @NazivKategorije", connection);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                    Kategorija = cmd.Parameters.Add("@NazivKategorije", SqlDbType.VarChar, 25 /* max length of field */ );
                    Kategorija.Value = txtNazivKategorije.Text;

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        IdKategorije = (int) reader["Id"];     
                    }
                    reader.Close();
                    
                    //Azuriram tabelu preferensi
                    if (IdKategorije!=0)
                       UpdateKorisniciKategorijeClanaka(IdKategorije);
                }
                catch (Exception ex) { 
                  // MessageBox.Show("Kategorija nije unesena !", "Important Message");
                    ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Kategorija nije unesena !');", true);

                }
               }
               else { 
                   //MessageBox.Show("Kategorija vec postoji !", "Important Message"); clear();
               ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Kategorija vec postoji !');", true);
               }
            }
            else ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Popuniti polja!');", true);
                //MessageBox.Show("Popuniti polja", "Important Message"); 
            clear();
        }
    }
}