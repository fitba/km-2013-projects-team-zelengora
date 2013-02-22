using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace KMWeb.Account
{
    public partial class UserDetails : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);

        DataSet getData(int UserId)
        {
            int _userId = UserId;
          
            string Category = Request.QueryString["CategoryId"];

            DataSet ds = new DataSet();

            connection.Open();


            SqlCommand cmd = new SqlCommand("select KC.Id AS IdKategorijaClanaka, ISNULL(Preferira,'false') AS Preferira, KC.NazivKategorije as NazivKategorije "
                                         + "From KategorijeClanaka AS KC left JOIN KorisniciKategorijeClanaka AS KKC "
                                         + " ON KC.Id=KKC.IdKategorijaClanaka WHERE IdKorisnik=" + _userId, connection);
          /*  
            
             
              SqlCommand cmd = new SqlCommand("select KC.Id AS IdKategorijaClanaka, ISNULL(Preferira,'false') AS Preferira, KC.NazivKategorije as NazivKategorije "
                                        + "From KategorijeClanaka AS KC left JOIN KorisniciKategorijeClanaka AS KKC "
                                        + " ON KC.Id=KKC.IdKategorijaClanaka", connection);
 */
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            DataRow dr;
            //dataTable.Columns.Add("IdKategorijaClanaka");

            dataTable.Columns.Add(new DataColumn("IdKategorijaClanaka", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Preferira", typeof(bool)));
            //dataTable.Columns.Add("NazivKategorije");
            dataTable.Columns.Add(new DataColumn("NazivKategorije", typeof(string)));
            

            da.Fill(dataTable);
            da.Fill(ds, "Preferirane Kategorije");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = dataTable.NewRow();

                dr["IdKategorijaClanaka"] = ds.Tables[0].Rows[i]["IdKategorijaClanaka"].ToString();
                dr["Preferira"] = ds.Tables[0].Rows[i]["Preferira"];


                dr["NazivKategorije"] = ds.Tables[0].Rows[i]["NazivKategorije"].ToString();

                dataTable.Rows.Add(dr);
                dataTable.AcceptChanges();


            }
            connection.Close();
            return ds;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            string username = (string)Session["korisnickoIme"];
            if (username != null)
            {
                if (!IsPostBack)
                {
                    gvPreferiraneKategorije.DataSource = getData(UserId);
                    gvPreferiraneKategorije.DataBind();
                    gvPreferiraneKategorije.Columns[0].Visible = false;
                }
            }else
            {
                MessageBox.Show("Logirajte se !");
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void deleteRows(int UserId)
        {
            int _userId = UserId;
            SqlCommand cmd = new SqlCommand("Delete from KorisniciKategorijeClanaka where IdKorisnik="+ _userId);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            int error = 0;
            connection.Close();

            deleteRows(UserId);
            foreach (GridViewRow dr in gvPreferiraneKategorije.Rows)
            {

                System.Web.UI.WebControls.CheckBox chkBx = (System.Web.UI.WebControls.CheckBox)dr.FindControl("Preferira");
                //int CategoryId = (int)gvPreferiraneKategorije.DataKeys[gvPreferiraneKategorije.SelectedIndex].Value;
                string CategoryId = dr.Cells[0].Text.ToString();

                // if (chkBx != null && chkBx.Checked)
                // {//
                /// INSERT
                try
                {
                    SqlCommand cmd = new SqlCommand("Insert into KorisniciKategorijeClanaka(IdKorisnik,IdKategorijaClanaka,Preferira) VALUES (@IdKorisnik,@IdKategorijaClanaka,@Preferira)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@IdKorisnik", UserId);
                    cmd.Parameters.AddWithValue("@IdKategorijaClanaka", Convert.ToInt32(CategoryId));
                    cmd.Parameters.AddWithValue("@Preferira", chkBx.Checked);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex) { error = 1; }              
            }
            if (error == 1)
                MessageBox.Show("Greška: Podaci NISU ažurirani", "Important Message");
            else
                MessageBox.Show("Podaci ažurirani", "Important Message");
        }
        }
}