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

namespace KMWeb.Administration
{
    public partial class UserAdministration : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        int IdKorisnika;

        void loadUsers()
        {
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("Select * from KorisniciGrupe", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds, "KorisniciGrupe");
            gvKorisnici.DataSource = ds;
            gvKorisnici.DataBind();
            gvKorisnici.Columns[0].Visible = false;
            connection.Close();
        }

        public void FillDropDownListKorisnici()
        {
            SqlCommand cmd = new SqlCommand("Select Id, NazivGrupe from GrupeKorisnika", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            DropDownListKorisnici.DataSource = ds.Tables[0];
            DropDownListKorisnici.DataTextField = "NazivGrupe";
            DropDownListKorisnici.DataValueField = "Id";
            DropDownListKorisnici.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
             string username = (string)Session["korisnickoIme"];
             if (username != null)
             {
                 loadUsers();
                 if (!IsPostBack)
                     FillDropDownListKorisnici();
             }
             else
             {
                 ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logirajte se !');window.location.href = '../Account/Login.aspx'", true);
             }
        }

        protected void gvKorisnici_SelectedIndexChanged(object sender, EventArgs e)
        {
            IdKorisnika = (int)gvKorisnici.DataKeys[gvKorisnici.SelectedIndex].Value;
            txtIdKorisnika.Text = IdKorisnika.ToString();
            txtImeKorisnika.Text = gvKorisnici.SelectedRow.Cells[3].Text.ToString();
            txtPrezimeKorisnika.Text = gvKorisnici.SelectedRow.Cells[4].Text.ToString();
            txtKorisnickoIme.Text = gvKorisnici.SelectedRow.Cells[2].Text.ToString();
            DropDownListKorisnici.SelectedIndex = Convert.ToInt32(gvKorisnici.SelectedRow.Cells[6].Text.ToString())-1;
        }

        protected void txtSpremi_Click(object sender, EventArgs e)
        {
            
            int idGrupe = Convert.ToInt32(DropDownListKorisnici.SelectedItem.Value);
            string korisnickoIme = txtKorisnickoIme.Text;
            string Ime=txtImeKorisnika.Text;
            string Prezime=txtPrezimeKorisnika.Text;

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Korisnici SET korisnickoIme=@korisnickoIme, Ime=@Ime , Prezime=@Prezime ,idGrupe=@idGrupe Where Id = " + Convert.ToInt32(txtIdKorisnika.Text));
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);
                cmd.Parameters.AddWithValue("@Ime", Ime);
                cmd.Parameters.AddWithValue("@Prezime", Prezime);
                cmd.Parameters.AddWithValue("@idGrupe", idGrupe);
                connection.Open();
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Azuriranje izvrseno", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Azuriranje izvrseno!');", true);
                loadUsers();
            }
            catch (Exception ex) { 
                //MessageBox.Show("Azuriranje NIJE izvrseno", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Azuriranje NIJE izvrseno!');", true);
            }
                


        }
    }
}