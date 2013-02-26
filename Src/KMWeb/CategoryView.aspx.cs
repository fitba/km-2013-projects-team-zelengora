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

            DataSet dsCat = new DataSet();
            DataTable dtCat = new DataTable();
           
            SqlDataAdapter daCat = new SqlDataAdapter();
            daCat.SelectCommand = new SqlCommand("Select Id,NazivKategorije from KategorijeClanaka where Id = " + Category, connection);
            daCat.Fill(dsCat, "KategorijeClanaka");
            dtCat = dsCat.Tables["KategorijeClanaka"];

            foreach (DataRow dr in dtCat.Rows)
            {
               lblKategorija.Text = dr["NazivKategorije"].ToString();
            }

           
            //lblKategorija.Text = dsCat.Tables[0].Rows[1]["NazivKategorije"].ToString();
            
            connection.Close();
           
            DataSet ds = new DataSet();
           // SqlConnection MyConn = new SqlConnection(strProvider);
            connection.Open();
            //SqlDataAdapter MyAdapter = new SqlDataAdapter(strSel, connection);
           // MyAdapter.Fill(ds, "inout");

            SqlCommand cmd = new SqlCommand("Select Id,Naslov,DatumKreiranja,Pregleda from Clanci where IdKategorija="+Category + "Order by DatumKreiranja DESC", connection);

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