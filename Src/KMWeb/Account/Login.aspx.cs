using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace KMWeb.Account
{
    public partial class Login : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
       
        protected int checkUser(string username, string password)
        {
            int idUser=0;
            string _username = username;
            string _password = password;

            connection.Open();
            SqlCommand cmd = new SqlCommand("Select Id, KorisnickoIme, Ime, Prezime From Korisnici Where "+
                           "KorisnickoIme = @username  and  Lozinka = @Lozinka", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlParameter userNameParam = cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25 /* max length of field */ );
            SqlParameter userNameParam1 = cmd.Parameters.Add("@Lozinka", SqlDbType.VarChar, 25 /* max length of field */ );
           // cmd.Parameters.AddWithValue("?Username", Convert.ToInt32(_username));
            userNameParam.Value = _username;
            userNameParam1.Value = _password;

            SqlDataAdapter daKorisnik = new SqlDataAdapter(cmd);
            SqlDataReader readerKorisnik = cmd.ExecuteReader();
             string ime;
             string prezime;
             string korisnicko;
            while (readerKorisnik.Read())
            {
                //lblKljucneRijeci.Text = lblKljucneRijeci.Text + "  -   " + readerKljucneRijeci["KljucnaRijec"].ToString() + "";
                //lblBrojOcjena.Text = readerBrojPitanja["BrojOcjena"].ToString();
               ime = readerKorisnik["Ime"].ToString();
               prezime = readerKorisnik["Prezime"].ToString();
               korisnicko = readerKorisnik["KorisnickoIme"].ToString();
               idUser = Convert.ToInt32(readerKorisnik["Id"]);
               Session["ime"] = ime;
               Session["prezime"] = prezime;
               Session["korisnickoIme"] = korisnicko;
               Session["UserId"] = idUser; 
            }

           // idUser = cmd.ExecuteNonQuery();
            return idUser;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {


        }

        protected void LoginButton_Click1(object sender, EventArgs e)
        {

            int UserId = checkUser(UserName.Text, Password.Text);

            if (UserId == 0)
            {
                MessageBox.Show("Nemate pravo pristupa!");

            }
            else
            {
                //set
               

                Response.Redirect("~/Default.aspx");
            }
        }
    }
}
