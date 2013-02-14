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
            
        }

        protected void LinkButton1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/NewArticle.aspx");
        }

        protected void btnAdminHome_Click(object sender, EventArgs e)
        {
            int userId = 1;  //Useti iz sesije
            Response.Redirect("~/Administration/AdministratorHome.aspx?UserID=" + userId);
        }
    }
}
