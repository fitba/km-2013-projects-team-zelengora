using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace KMWeb.Administration
{
    public partial class ManageCategories : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
       
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = (string)Session["korisnickoIme"];
            if (username == null)
            {
                MessageBox.Show("Logirajte se !");
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        
        private void clear()
        {
            txtNazivKategorije.Text = "";
            txtOpisKategorije.Text = "";

        }

        protected void btnNovaKategorija_Click(object sender, EventArgs e)
        {
            if (txtNazivKategorije.Text != "")
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("Insert into KategorijeClanaka(NazivKategorije,OpisKategorije,DatumKreiranja) VALUES (@NazivKategorije,@OpisKategorije,@DatumKreiranja)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    
                    cmd.Parameters.AddWithValue("@NazivKategorije", txtNazivKategorije.Text );
                    cmd.Parameters.AddWithValue("@OpisKategorije", txtOpisKategorije.Text);
                    cmd.Parameters.AddWithValue("@DatumKreiranja", DateTime.Today);
                    connection.Open();
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Kategorija unesena !", "Important Message");

                }
                catch (Exception ex) { MessageBox.Show("Kategorija nije unesena !", "Important Message"); }
            }
            else MessageBox.Show("Popuniti polja", "Important Message"); 
            clear();
        }
    }
}