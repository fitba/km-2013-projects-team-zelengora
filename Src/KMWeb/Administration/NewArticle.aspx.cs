using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;




namespace KMWeb.Administration
{
    public partial class NewArticle : System.Web.UI.Page
    {
        int LastId = 0;
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        string KljucnaRijec1 = "";
        string KljucnaRijec2 = "";
        string KljucnaRijec3 = "";
        string KljucnaRijec4 = "";
        string KljucnaRijec5 = "";
        string KljucnaRijec6 = "";
        string KljucnaRijec7 = "";
        string KljucnaRijec8 = "";
        string KljucnaRijec9 = "";
        string KljucnaRijec10 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // UNOS NOVOG CLANKA
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Clanci (Naslov, Sadrzaj, IdKategorija,IdKorisnik,DatumKreiranja) VALUES (@Naslov, @Sadrzaj, @IdKategorija, @IdKorisnik,@DatumKreiranja); SELECT SCOPE_IDENTITY();");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Naslov", txtNaslov.Text);
                cmd.Parameters.AddWithValue("@Sadrzaj", txtSadrzaj.Text);
                cmd.Parameters.AddWithValue("@IdKategorija", DropDownCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@IdKorisnik", "1");
                cmd.Parameters.AddWithValue("@DatumKreiranja", DateTime.Now.Date.ToShortDateString());

                connection.Open();
                //cmd.exExecuteNonQuery();
                LastId = Convert.ToInt16(cmd.ExecuteScalar());

                MessageBox.Show("Članak uspješno unesen","Important Message");
                connection.Close();
                insertKljucneRijeci();
            }
            catch (Exception ex) { MessageBox.Show("Članak nije unesen!", "Important Message"); }

            ClearFields();
        }


        protected void ClearFields()
        {
            txtNaslov.Text = "";
            txtSadrzaj.Text = "";
            txtKljucnaRijec1.Text = "";
            txtKljucnaRijec2.Text = "";
            txtKljucnaRijec3.Text = "";
            txtKljucnaRijec4.Text = "";
            txtKljucnaRijec5.Text = "";
            txtKljucnaRijec6.Text = "";
            txtKljucnaRijec7.Text = "";
            txtKljucnaRijec8.Text = "";
            txtKljucnaRijec9.Text = "";
            txtKljucnaRijec10.Text = "";
            connection.Close();
        
        }

        public void FillDropDownList()
        {
            SqlCommand cmd = new SqlCommand("Select Id, NazivKategorije from KategorijeClanaka", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DropDownCategory.DataSource = ds.Tables[0];
            DropDownCategory.DataTextField = "NazivKategorije";
            DropDownCategory.DataValueField = "Id";
            DropDownCategory.DataBind();
        }

        protected void DropDownList1_Load(object sender, EventArgs e)
        {

        }

        protected void DropDownCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            FillDropDownList();
        }

        private void insertKljucneRijeci() 
        {
            //Unos Kljucnih rijeci
            try
            {
                SqlCommand cmd = new SqlCommand("InsertKljucneRijeci");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                cmd.Parameters.AddWithValue("@IdArticle", LastId);

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

                if (txtKljucnaRijec6.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec6", txtKljucnaRijec6.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec6", null);

                if (txtKljucnaRijec7.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec7", txtKljucnaRijec7.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec7", null);

                if (txtKljucnaRijec8.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec8", txtKljucnaRijec8.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec8", null);

                if (txtKljucnaRijec9.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec9", txtKljucnaRijec9.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec9", null);

                if (txtKljucnaRijec10.Text != "")
                    cmd.Parameters.AddWithValue("@KljucnaRijec10", txtKljucnaRijec10.Text);
                else
                    cmd.Parameters.AddWithValue("@KljucnaRijec10", null);


                connection.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Kljucne rijeci unesene.", "Important Message");
            }
            catch (Exception ex) { MessageBox.Show("Kljucne rijeci NISU unesene.", "Important Message"); }
            finally { connection.Close(); }
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            insertKljucneRijeci();

        }
    }
}