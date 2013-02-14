using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace KMWeb.Administration
{
    public partial class AdministratorHome : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSel;
            string UserID = Request.QueryString["UserID"];

            DataSet ds = new DataSet();
         
            connection.Open();

            SqlCommand cmd = new SqlCommand
            ("SELECT Clanci.Id AS IdOriginalClanka, Clanci.Naslov AS OriginalNaslov, Clanci.Sadrzaj AS OriginalSadrzaj, Clanci.DatumKreiranja AS DatumKreiranjaClanka, "
              + "PrijeglogRevizije.Id AS IdPrijedlogaRevizije, PrijeglogRevizije.Naslov AS NaslovPrijedloga, PrijeglogRevizije.Sadrzaj AS SadrzajPrijedloga, "
              + "PrijeglogRevizije.DatumPrijedloga, PrijeglogRevizije.IdKorisnik, Korisnici.Ime AS ImePredlagaca, Korisnici.Prezime AS PrezimePredlagaca, PrijeglogRevizije.Status,"
              + "Clanci.IdKorisnik AS AutorClanka "
              + "FROM Korisnici INNER JOIN "
              + "PrijeglogRevizije ON Korisnici.Id = PrijeglogRevizije.IdKorisnik INNER JOIN "
              + "Clanci ON PrijeglogRevizije.IdClanak = Clanci.Id "
              + "WHERE (PrijeglogRevizije.Status = 1) AND (Clanci.IdKorisnik =" + UserID + ")", connection); //SAMO za jednog korisnika

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);
            gvPrijedloziRevizije.DataSource = ds;
            gvPrijedloziRevizije.DataBind();
        }

        protected void gvPrijedloziRevizije_SelectedIndexChanged(object sender, EventArgs e)
        {
            String IdArticle = gvPrijedloziRevizije.SelectedRow.Cells[1].Text;
            String IdPrijedlogaRevizije = gvPrijedloziRevizije.SelectedRow.Cells[5].Text;
            Response.Redirect("~/Administration/CompareRevision.aspx?ArticleId=" + IdArticle + "&IdProposal=" + IdPrijedlogaRevizije);
        }
    }
}