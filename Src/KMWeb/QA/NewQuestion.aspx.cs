using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace KMWeb.QA
{
    public partial class NewQuestion : System.Web.UI.Page
    {
        QAQuestions pitanje = new QAQuestions();
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        int LastId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
               //get
            string username = (string)Session["korisnickoIme"];
            int UserId = Convert.ToInt32(Session["UserId"]);
        }

        private void insertKljucneRijeci()
        {
            //Unos Kljucnih rijeci
            try
            {
                SqlCommand cmd = new SqlCommand("QAInsertKljucneRijeci");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                cmd.Parameters.AddWithValue("@IdPitanje", LastId);

                if (txtKljucnaRijec1.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec1", txtKljucnaRijec1.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec1", null);

                if (txtKljucnaRijec2.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec2", txtKljucnaRijec2.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec2", null);

                if (txtKljucnaRijec3.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec3", txtKljucnaRijec3.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec3", null);

                if (txtKljucnaRijec4.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec4", txtKljucnaRijec4.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec4", null);

                if (txtKljucnaRijec5.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec5", txtKljucnaRijec5.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec5", null);

                connection.Open();
                cmd.ExecuteNonQuery();

                // MessageBox.Show("Kljucne rijeci unesene.", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Kljucne rijeci unesene.');", true);
                //ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Članak uspješno unesen!');", true);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Kljucne rijeci NISU unesene.", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Kljucne rijeci NISU unesene.');", true);
            }
            finally { connection.Close(); }
        }

        protected void ClearFields()
        {
            txtNaslovPitanja.Text = "";
            txtPitanje.Text = "";
            txtKljucnaRijec1.Text = "";
            txtKljucnaRijec2.Text = "";
            txtKljucnaRijec3.Text = "";
            txtKljucnaRijec4.Text = "";
            txtKljucnaRijec5.Text = "";
           // connection.Close();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {   int UserId = Convert.ToInt32(Session["UserId"]);
            //int LastId=0;
            if ((txtNaslovPitanja.Text != "") && (txtPitanje.Text != ""))
            {
                pitanje.NaslovPitanja = txtNaslovPitanja.Text;
                pitanje.Pitanje = txtPitanje.Text;
                //pitanje.Datum = DateTime.Now.ToString();
                pitanje.IdKorisnik = UserId;
                pitanje.IdKategorija = 1;

                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO QAPitanja (NaslovPitanja, Pitanje, Datum, IdKategorija,IdKorisnik) VALUES (@NaslovPitanja, @Pitanje, @Datum, @IdKategorija,@IdKorisnik); SELECT SCOPE_IDENTITY();");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@NaslovPitanja", pitanje.NaslovPitanja);
                    cmd.Parameters.AddWithValue("@Pitanje", pitanje.Pitanje);
                    cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdKategorija", pitanje.IdKategorija);
                    cmd.Parameters.AddWithValue("@IdKorisnik", pitanje.IdKorisnik);

                    connection.Open();
                    //cmd.exExecuteNonQuery();
                    LastId = Convert.ToInt16(cmd.ExecuteScalar());

                    //MessageBox.Show("Članak uspješno unesen", "Important Message");
                    ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Pitanje uspješno uneseno!');", true);
                    connection.Close();
                    insertKljucneRijeci();
                    ClearFields();
                   

                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Članak nije unesen!", "Important Message");
                    ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Pitanje nije uspješno uneseno!');", true);
                }
                finally { connection.Close(); }
            }
            else
            {
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Popuniti polja!');", true);
            }
        }
    }
}