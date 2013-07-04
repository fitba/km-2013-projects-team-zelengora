using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KMWeb.Administration
{
    public partial class CategoryAdministration : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = (string)Session["korisnickoIme"];
            if (username != null)
            {
                loadCategories();
                //if (!IsPostBack)
                    //FillDropDownListKorisnici();
            }
            else
            {
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logirajte se !');window.location.href = '../Account/Login.aspx'", true);
            }
        }

        protected void loadCategories()
        {
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("SELECT  dbo.Korisnici.Id AS IdKorisnik, dbo.Korisnici.KorisnickoIme AS KorisnickoIme, "
            +" dbo.KategorijeClanaka.NazivKategorije, dbo.KategorijeClanaka.DatumKreiranja AS DatumKreiranja, dbo.KategorijeClanaka.CancelDate AS CancelDate, "
            + " dbo.KategorijeClanaka.CancelUser AS CancelUser, dbo.KategorijeClanaka.OpisKategorije AS OpisKategorije,"
            + "dbo.KategorijeClanaka.Id AS IdKategorija "
                                      +" FROM   dbo.KategorijeClanaka INNER JOIN"
                                    +" dbo.Korisnici ON dbo.KategorijeClanaka.IdKorisnik = dbo.Korisnici.Id"
                                    + " WHERE KategorijeClanaka.CancelDate IS NULL ", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(ds, "KategorijeClanaka");
                gvKategorije.DataSource = ds;
                gvKategorije.DataBind();
               // gvKategorije.Columns[0].Visible = false;
            }
            catch (Exception ex) { }
            finally
            {
                connection.Close();
            }
        }

        protected void gvKategorije_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int IdKategorije = 0; 
            int UserId = Convert.ToInt32(Session["UserId"]);

           // ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Azuriranje NIJE izvrseno!');", true);

            //ClientScript.RegisterStartupScript(typeof(Page), "Message", "<SCRIPT LANGUAGE='javascript'>var ays = confirm('Are you sure?');document.getElementById('" + HiddenField1.ClientID + "').value = ays;</script>");
           // String response = HiddenField1.Text;

           // if (response == "true")
            //{

                IdKategorije = (int)gvKategorije.DataKeys[e.RowIndex].Value;

                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("UPDATE KategorijeClanaka "
                                          + " Set CancelDate = @CancelDate,"
                                          + " CancelUser = @CancelUser "
                                        + " Where Id = @Id ", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CancelDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CancelUser", UserId);
                cmd.Parameters.AddWithValue("@Id", IdKategorije);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    loadCategories();
                    //gvKategorije.DataSource = ds;
                    //gvKategorije.DataBind();
                    // gvKategorije.Columns[0].Visible = false;
                }
                catch (Exception ex) { }
                finally
                {
                    connection.Close();
                }
           /* }
            else
            {
                loadCategories();
                HiddenField1.Text = "";
                    }*/

        }

        protected void gvKategorije_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IdKategorije = (int)gvKategorije.DataKeys[gvKategorije.SelectedIndex].Value;
        }


    }
}