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
                linkLogOut.Visible = true;
            }
            loadCategories();
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
    }
}
