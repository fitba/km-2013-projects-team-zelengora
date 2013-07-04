using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace KMWeb.QA
{
    public partial class QACompareRevision : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        List<QAQuestions> OriginalPitanje = new List<QAQuestions>();
        QAQuestions _pitanjeOriginal = new QAQuestions();

        List<QAQuestions> PitanjePrijedlog = new List<QAQuestions>();
        QAQuestions _pitanjePrijedlog = new QAQuestions();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadOriginal();
            LoadPrijedlog();
        }

        private void LoadOriginal()
        {
            string Question = Request.QueryString["IdQuestion"];

            DataSet ds = new DataSet();
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT Id, NaslovPitanja, Pitanje "+
                               " FROM dbo.QAPitanja WHERE Id=" + Question, connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               // txtNaslov.Text = reader["Naslov"].ToString();
                //txtOriginialSadrzaj.Text = reader["Sadrzaj"].ToString();
                _pitanjeOriginal.NaslovPitanja = reader["NaslovPitanja"].ToString();
               _pitanjeOriginal.Pitanje = reader["Pitanje"].ToString();

               OriginalPitanje.Add(_pitanjeOriginal);
                Repeater1.DataSource = OriginalPitanje;
                Repeater1.DataBind();
                reader.Close();
                connection.Close();
            }

            //BindData(Convert.ToInt32(Article));
            connection.Close();
        }

        private void LoadPrijedlog()
        {
            string Question = Request.QueryString["IdQuestion"];
            string Proposal = Request.QueryString["IdProposal"];
            DataSet ds = new DataSet();
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT dbo.QAPitanja.Id, dbo.QAPitanja.NaslovPitanja, dbo.QAPitanja.Pitanje, dbo.QAPrijedlogRevizije.Id AS IdPrijedlogRevizije, "
                     +" dbo.QAPrijedlogRevizije.NaslovPitanja AS NaslovPitanjaPrijedlog, dbo.QAPrijedlogRevizije.Pitanje AS PitanjePrijedlog, "
                     +" dbo.QAPrijedlogRevizije.IdPitanje AS IdQAPitanja"
                    +" FROM dbo.QAPitanja INNER JOIN"
                      +" dbo.QAPrijedlogRevizije ON dbo.QAPitanja.Id = dbo.QAPrijedlogRevizije.IdPitanje"
                     + " WHERE (QAPrijedlogRevizije.IdPitanje = " + Question + ") AND (QAPrijedlogRevizije.Id = " + Proposal + ")", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               // txtPrijedlogNaslova.Text = reader["Naslov"].ToString();
               // txtPrijedlogSadrzaja.Text = reader["Sadrzaj"].ToString();

                _pitanjePrijedlog.NaslovPitanja = reader["NaslovPitanjaPrijedlog"].ToString();
                _pitanjePrijedlog.Pitanje = reader["PitanjePrijedlog"].ToString();
                PitanjePrijedlog.Add(_pitanjePrijedlog);
                Repeater2.DataSource = PitanjePrijedlog;
                Repeater2.DataBind();
                reader.Close();
                connection.Close();
            }

            //BindData(Convert.ToInt32(Article));
            connection.Close();
        }

        protected void setStatusRevizije(int status)
        {
            string Proposal = Request.QueryString["IdProposal"];
            string UserID = Session["UserId"].ToString();
            DataSet ds = new DataSet();
            connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE QAPrijedlogRevizije "
                    + " SET Status = " + status
                    + " WHERE Id = " + Convert.ToInt32(Proposal), connection);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Status prijedloga izmjene ažuriran!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Status prijedloga izmjene ažuriran!');", true);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Status prijedloga izmjene nije ažuriran!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Status prijedloga izmjene nije ažuriran!');", true);
            }


            connection.Close();
            Response.Redirect("~/Administration/AdministratorHome.aspx?UserID= " + UserID);
        }

        private void prihvatiSveIzmjene(string prijedlogNaslova, string prijedlogSadrzaja)
        {
            string Question = Request.QueryString["IdQuestion"];
            string Proposal = Request.QueryString["IdProposal"];
            //DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE QAPitanja "
                    + " SET NaslovPitanja = @Naslov, "
                    + " Pitanje = @Sadrzaj "
                    + " WHERE Id = " + Convert.ToInt32(Question), connection);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Naslov", prijedlogNaslova);
                cmd.Parameters.AddWithValue("@Sadrzaj", prijedlogSadrzaja);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Izmjena naslova i sadrzaja uspjela!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena naslova i pitanja uspjela!');", true);
                connection.Close();
                setStatusRevizije(2);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Izmjena naslova i sadrzaja nije uspjela!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena naslova i pitanja nije uspjela!');", true);
                connection.Close();
            }
        }

        protected void prihvatiSadrzaj(string Sadrzaj)
        {
            string Question = Request.QueryString["IdQuestion"];
            string Proposal = Request.QueryString["IdProposal"];
            //DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE QAPitanja "
                     + " SET Pitanje = @Sadrzaj "
                    + " WHERE Id = " + Convert.ToInt32(Question), connection);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Sadrzaj", Sadrzaj);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Izmjena sadrzaja uspjela!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena pitanja uspjela!');", true);
                connection.Close();
                setStatusRevizije(4);
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Izmjena sadrzaja nije uspjela!", "Important Message"); connection.Close();
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena pitanja nije uspjela!');", true);
            }

        }

        protected void prihvatiNaslov(string Naslov)
        {
            string Question = Request.QueryString["IdQuestion"];
            string Proposal = Request.QueryString["IdProposal"];
            //DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE QAPitanja "
                    + " SET NaslovPitanja = @Naslov "
                    + " WHERE Id = " + Convert.ToInt32(Question), connection);
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
            catch (Exception ex)
            {
                //MessageBox.Show("Izmjena naslova nije uspjela!", "Important Message"); connection.Close();
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Izmjena naslova nije uspjela!');", true);
            }

        }

        protected void Repeater2_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string isactivestatus = Convert.ToString(e.CommandArgument);

            string[] arg = new string[2];
            arg[0] = isactivestatus.Substring(0, isactivestatus.IndexOf(";"));

            arg[1] = isactivestatus.Substring(isactivestatus.IndexOf(";") + 1);

            string NaslovPitanja = arg[0];
            string Pitanje = arg[1];

            switch (e.CommandName)
            {
                case "btnPrihvatiSve1": prihvatiSveIzmjene(NaslovPitanja, Pitanje);// insertAnswerVoteUP(IdOdgovor, UkupnoOcjena);
                    break;
                case "btnOdbijsve": setStatusRevizije(3);
                    break;
                case "btnPrihvatiSadrzaj": prihvatiSadrzaj(Pitanje);
                    break;
                case "btnPrihvatiNaslov": prihvatiNaslov(NaslovPitanja);
                    break;

            }
        }
    }
}