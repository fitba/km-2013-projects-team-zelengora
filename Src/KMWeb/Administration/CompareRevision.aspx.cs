using System;
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
    public partial class CompareRevision : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadOriginal();
            LoadPrijedlog();
        }

        private void LoadOriginal()
        {
            string Article = Request.QueryString["ArticleId"];

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
                reader.Close();
                connection.Close();
            }

            //BindData(Convert.ToInt32(Article));
            connection.Close();
        }


        private void LoadPrijedlog()
        {
            string Article = Request.QueryString["ArticleId"];
            string Proposal = Request.QueryString["IdProposal"];
            DataSet ds = new DataSet();
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT PrijeglogRevizije.IdClanak, PrijeglogRevizije.Naslov AS Naslov, PrijeglogRevizije.Sadrzaj AS Sadrzaj," 
            +" PrijeglogRevizije.DatumPrijedloga, PrijeglogRevizije.IdKorisnik, "
            +" PrijeglogRevizije.Status, PrijeglogRevizije.Id AS IdPrijedlog, Korisnici.Ime, Korisnici.Prezime"
            +" FROM PrijeglogRevizije INNER JOIN "
            +" Korisnici ON PrijeglogRevizije.IdKorisnik = Korisnici.Id "
            + " WHERE (PrijeglogRevizije.IdClanak = " + Article + ") AND (PrijeglogRevizije.Id = " + Proposal + ")", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtPrijedlogNaslova.Text = reader["Naslov"].ToString();
                txtPrijedlogSadrzaja.Text = reader["Sadrzaj"].ToString();
                reader.Close();
                connection.Close();
            }

            //BindData(Convert.ToInt32(Article));
            connection.Close();
        }


        private void prihvatiSveIzmjene()
        { 
            string Article = Request.QueryString["ArticleId"];
            string Proposal = Request.QueryString["IdProposal"];
            //DataSet ds = new DataSet();
            try
            {
            connection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Clanci "
                + " SET Naslov = @Naslov, "
                + " Sadrzaj = @Sadrzaj " 
                + " WHERE Id = " + Convert.ToInt32(Article), connection);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Naslov", txtPrijedlogNaslova.Text);
            cmd.Parameters.AddWithValue("@Sadrzaj", txtPrijedlogSadrzaja.Text );
            cmd.ExecuteNonQuery();
            MessageBox.Show("Izmjena naslova i sadrzaja uspjela!", "Important Message");
            connection.Close();
            setStatusRevizije(2);
            }
            catch (Exception ex) { MessageBox.Show("Izmjena naslova i sadrzaja nije uspjela!", "Important Message");
              connection.Close();
            }
            
            
         }
       
        protected void setStatusRevizije(int status) 
        {
            string Proposal = Request.QueryString["IdProposal"];
            DataSet ds = new DataSet();
            connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE PrijeglogRevizije "
                    + " SET Status = " + status
                    + " WHERE Id = " + Convert.ToInt32(Proposal), connection);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Status prijedloga izmjene ažuriran!", "Important Message");
            }
            catch (Exception ex) { MessageBox.Show("Status prijedloga izmjene nije ažuriran!", "Important Message");
            
            }
            
           
            connection.Close();
        }

        protected void btnPrihvatiSve_Click(object sender, EventArgs e)
        {
            prihvatiSveIzmjene();
        }

        protected void btnOdbijSve_Click(object sender, EventArgs e)
        {
            setStatusRevizije(3);
        }

        protected void prihvatiNaslov()
        {
            string Article = Request.QueryString["ArticleId"];
            string Proposal = Request.QueryString["IdProposal"];
            //DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Clanci "
                    + " SET Naslov = @Naslov "
                    + " WHERE Id = " + Convert.ToInt32(Article), connection);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Naslov", txtPrijedlogNaslova.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Izmjena naslova uspjela!", "Important Message");
                connection.Close();
                setStatusRevizije(4);
            }
            catch (Exception ex) { MessageBox.Show("Izmjena naslova nije uspjela!", "Important Message"); connection.Close(); }
            
        }

        protected void prihvatiSadrzaj()
        {
            string Article = Request.QueryString["ArticleId"];
            string Proposal = Request.QueryString["IdProposal"];
            //DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Clanci "
                     + " SET Sadrzaj = @Sadrzaj " 
                    + " WHERE Id = " + Convert.ToInt32(Article), connection);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Sadrzaj", txtPrijedlogSadrzaja.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Izmjena sadrzaja uspjela!", "Important Message");
                connection.Close();
                setStatusRevizije(4);
            }
            catch (Exception ex) { MessageBox.Show("Izmjena sadrzaja nije uspjela!", "Important Message"); connection.Close(); }
            
        }

        protected void btnPrihvatiNaslov_Click(object sender, EventArgs e)
        {
            prihvatiNaslov();
            
        }

        protected void btnPrihvatiSadrzaj_Click(object sender, EventArgs e)
        {
            prihvatiSadrzaj();
            
        }
    }
}