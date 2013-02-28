using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace KMWeb
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);

        protected void Page_Load(object sender, EventArgs e)
        {
            string surname = (string)Session["prezime"];
            string name = (string)Session["ime"];
            string korisnicko = (string)Session["korisnickoIme"];
            if (korisnicko != null)
            {
                lblUserIme.Text = name;
                lblUserPrezime.Text = surname;
                loadZadnjiPregledi();
                linkLogOut.Visible = true;
            }
            loadCategories();
            loadPopularno();
            
        }

        protected void LinkButton1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/NewArticle.aspx");
        }

        protected void btnAdminHome_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);  //Useti iz sesije
            Response.Redirect("~/Administration/AdministratorHome.aspx?UserID=" + userId);
        }

        protected void linkLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/ManageCategories.aspx");
        }

        void loadCategories()
        {
            DataSet ds = new DataSet();
           
            SqlCommand cmd = new SqlCommand("Select Id,NazivKategorije from KategorijeClanaka", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            da.Fill(ds, "Kategorije");
            gvKategorije.DataSource = ds;
            gvKategorije.DataBind();
            gvKategorije.Columns[0].Visible = false;
            connection.Close();
        }

        protected void gvKategorije_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s1 = gvKategorije.SelectedRow.Cells[1].Text.ToString();
            int CategoryId = (int)gvKategorije.DataKeys[gvKategorije.SelectedIndex].Value;
            Response.Redirect("~/CategoryView.aspx?CategoryId=" + CategoryId);
        }

        protected void btnPodaciOKorisniku_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/UserDetails.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/UserAdministration.aspx");
        }

        protected void loadZadnjiPregledi()
        {
            int userId = Convert.ToInt32(Session["UserId"]);  //Useti iz sesije
            DataSet ds1 = new DataSet();

            SqlCommand cmd = new SqlCommand("Select TOP(5) IdKorisnik,IdClanak,Naslov,DatumZadnjegPregleda from ZadnjiPregledi where IdKorisnik= " + userId +
                " ORDER BY DatumZadnjegPregleda DESC", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds1, "ZadnjiPregledi");
            gvZadnjiPregledi.DataSource = ds1;
            gvZadnjiPregledi.DataBind();
            gvZadnjiPregledi.Columns[0].Visible = false;
            gvZadnjiPregledi.Columns[1].Visible = false;
            connection.Close();
        }

        protected void loadPopularno()
        {
            int userId = Convert.ToInt32(Session["UserId"]);  //Useti iz sesije
            DataSet ds1 = new DataSet();

            SqlCommand cmd = new SqlCommand("Select TOP(5) IdKategorija,IdClanak,Naslov,Pregleda from PopularniClanci " +
                " ORDER BY Pregleda DESC", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds1, "PopularniClanci");
            gvPopularno.DataSource = ds1;
            gvPopularno.DataBind();
            gvPopularno.Columns[0].Visible = false;
            gvPopularno.Columns[1].Visible = false;
            connection.Close();
        }


        protected void gvZadnjiPregledi_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s1 = gvKategorije.SelectedRow.Cells[1].Text.ToString();
            int ArticleId = (int)gvZadnjiPregledi.DataKeys[gvZadnjiPregledi.SelectedIndex].Value;
            Response.Redirect("~/ArticleView.aspx?ArticleId=" + ArticleId);
        }

        protected void gvPopularno_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ArticleId = (int)gvPopularno.DataKeys[gvPopularno.SelectedIndex].Value;
            Response.Redirect("~/ArticleView.aspx?ArticleId=" + ArticleId);
        }

       
    }
}
