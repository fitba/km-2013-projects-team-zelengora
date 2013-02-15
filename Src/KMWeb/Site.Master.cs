using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace KMWeb
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {

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
    }
}
