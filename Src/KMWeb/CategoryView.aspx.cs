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
    public partial class CategoryView : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        protected void Page_Load(object sender, EventArgs e)
        {
            //string aaa;
            //aaa = Request.Params["aaa"];
            string strSel;
            string Category = Request.QueryString["CategoryId"];
            /*if (aaa != "" && aaa != null)
            {
                // Response.Write("<script>alert('" + aaa + "');</script>");
                strSel = "Select * from Clanci where IdKategorija=3";

            }
            else
            {
                strSel = "Select * from inout";
            }

            Response.Write(strSel);*/
            //string strProvider = "Server=(local);DataBase=AIS20060712101417;UID=sa;PWD=";
            DataSet ds = new DataSet();
           // SqlConnection MyConn = new SqlConnection(strProvider);
            connection.Open();
            //SqlDataAdapter MyAdapter = new SqlDataAdapter(strSel, connection);
           // MyAdapter.Fill(ds, "inout");

            SqlCommand cmd = new SqlCommand("Select Id,Naslov,DatumKreiranja from Clanci where IdKategorija="+Category, connection);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Id = GridView1.SelectedRow.Cells[1].Text;
            Response.Redirect("~/ArticleView.aspx?ArticleId="+Id);
        
        }
    }
}