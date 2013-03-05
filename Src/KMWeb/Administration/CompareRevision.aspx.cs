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
        List<Clanak> Sadrzaj = new List<Clanak>();
        Clanak clanak = new Clanak();
        List<Clanak> SadrzajPrijedlog = new List<Clanak>();
        Clanak clanakPrijedlog = new Clanak();
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
                clanak.Naslov = reader["Naslov"].ToString();
                clanak.Sadrzaj = reader["Sadrzaj"].ToString();
                Sadrzaj.Add(clanak);
                Repeater1.DataSource = Sadrzaj;
                Repeater1.DataBind();
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
                clanakPrijedlog.Naslov = reader["Naslov"].ToString();
                clanakPrijedlog.Sadrzaj = reader["Sadrzaj"].ToString();
                SadrzajPrijedlog.Add(clanakPrijedlog);
                Repeater2.DataSource = SadrzajPrijedlog;
                Repeater2.DataBind();
                reader.Close();
                connection.Close();
            }

            //BindData(Convert.ToInt32(Article));
            connection.Close();
        }


        private void prihvatiSveIzmjene(string prijedlogNaslova, string prijedlogSadrzaja)
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
            cmd.Parameters.AddWithValue("@Naslov", prijedlogNaslova);
            cmd.Parameters.AddWithValue("@Sadrzaj", prijedlogSadrzaja);
            cmd.ExecuteNonQuery();
            //MessageBox.Show("Izmjena naslova i sadrzaja uspjela!", "Important Message");
            ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena naslova i sadrzaja uspjela!');", true);
            connection.Close();
            setStatusRevizije(2);
            }
            catch (Exception ex) { 
                //MessageBox.Show("Izmjena naslova i sadrzaja nije uspjela!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena naslova i sadrzaja nije uspjela!');", true);
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
                //MessageBox.Show("Status prijedloga izmjene ažuriran!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Status prijedloga izmjene ažuriran!');", true);
            }
            catch (Exception ex) { 
                //MessageBox.Show("Status prijedloga izmjene nije ažuriran!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Status prijedloga izmjene nije ažuriran!');", true);
            }
            
           
            connection.Close();
        }

        protected void btnPrihvatiSve_Click(object sender, EventArgs e)
        {
           // prihvatiSveIzmjene();
        }

        protected void btnOdbijSve_Click(object sender, EventArgs e)
        {
            setStatusRevizije(3);
        }

        protected void prihvatiNaslov(string Naslov)
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
                cmd.Parameters.AddWithValue("@Naslov", Naslov);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Izmjena naslova uspjela!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena naslova uspjela!');", true);
                connection.Close();
                setStatusRevizije(4);
            }
            catch (Exception ex) { 
                //MessageBox.Show("Izmjena naslova nije uspjela!", "Important Message"); connection.Close();
            ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena naslova nije uspjela!');", true);
            }
            
        }

        protected void prihvatiSadrzaj(string Sadrzaj)
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
                cmd.Parameters.AddWithValue("@Sadrzaj", Sadrzaj);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Izmjena sadrzaja uspjela!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena sadrzaja uspjela!');", true);
                connection.Close();
                setStatusRevizije(4);
            }
            catch (Exception ex) { 
                
               // MessageBox.Show("Izmjena sadrzaja nije uspjela!", "Important Message"); connection.Close();
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena sadrzaja nije uspjela!');", true);
            }
            
        }

        protected void btnPrihvatiNaslov_Click(object sender, EventArgs e)
        {
            //prihvatiNaslov();
            
        }

        protected void btnPrihvatiSadrzaj_Click(object sender, EventArgs e)
        {
           // prihvatiSadrzaj();
            
        }

        protected void RepeaterPrijedlog_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string isactivestatus = Convert.ToString(e.CommandArgument);

            string[] arg = new string[2];
            arg[0] = isactivestatus.Substring(0,isactivestatus.IndexOf(";"));

            arg[1] = isactivestatus.Substring(isactivestatus.IndexOf(";")+1);

            string Naslov = arg[0];
            string Sadrzaj = arg[1];

            switch (e.CommandName)
            {
                case "btnPrihvatiSve1": prihvatiSveIzmjene(Naslov, Sadrzaj);// insertAnswerVoteUP(IdOdgovor, UkupnoOcjena);
                    break;
                case "btnOdbijsve": setStatusRevizije(3);
                    break;
                case "btnPrihvatiSadrzaj": prihvatiSadrzaj(Sadrzaj);
                    break;
                case "btnPrihvatiNaslov": prihvatiNaslov(Naslov);
                    break;
                    
            }
        }
    }
}