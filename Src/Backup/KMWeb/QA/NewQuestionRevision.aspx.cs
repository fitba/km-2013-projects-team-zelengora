using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace KMWeb.QA
{
    public partial class NewQuestionRevision : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        protected void Page_Load(object sender, EventArgs e)
        {
         

            QAQuestions pitanje = new QAQuestions();
            List<QAQuestions> Pitanja = new List<QAQuestions>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
       
            string Question = Request.QueryString["IdQuestion"];

            SqlCommand cmd = new SqlCommand("SELECT dbo.QAPitanja.Id AS IdPitanje, dbo.QAPitanja.NaslovPitanja, dbo.QAPitanja.Pitanje FROM dbo.QAPitanja Where dbo.QAPitanja.Id=@IdPitanje", connection);
            cmd.Parameters.AddWithValue("@IdPitanje", Question);

            dt.Columns.Add("IdPitanje");
            dt.Columns.Add("NaslovPitanja");
            dt.Columns.Add("Pitanje");
 
            connection.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds, "QAPitanja");
                da.Fill(dt);

                SqlDataReader reader = cmd.ExecuteReader();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["IdPitanje"] = ds.Tables[0].Rows[i]["IdPitanje"].ToString();
                    dr["NaslovPitanja"] = ds.Tables[0].Rows[i]["NaslovPitanja"].ToString();
                    dr["Pitanje"] = ds.Tables[0].Rows[i]["Pitanje"].ToString();

                    reader.Close();
                    connection.Close();

                    pitanje = new QAQuestions();

                    pitanje.Id = Convert.ToInt32(dr["IdPitanje"].ToString());
                    pitanje.NaslovPitanja = dr["NaslovPitanja"].ToString();
                    pitanje.Pitanje = dr["Pitanje"].ToString();
                   
                }
            }
            catch (Exception ex) { }
            finally
            {
                connection.Close();
            }
            

            Pitanja.Add(pitanje);
            Repeater1.DataSource = Pitanja;
            Repeater1.DataBind();
            if (!Page.IsPostBack)
            {
                txtPrijedlogNaslova.Text = pitanje.NaslovPitanja;
                txtPrijedlogPitanja.Text = pitanje.Pitanje;
            }
            
        }

       

        protected void Button1_Click(object sender, EventArgs e) // UNOS PRIJEDLOGA
        {
            try
            {
                string Question = Request.QueryString["IdQuestion"];


                SqlCommand cmd = new SqlCommand("INSERT INTO QAPrijedlogRevizije (IdPitanje, NaslovPitanja, Pitanje, DatumPrijedloga, IdKorisnik, Status) VALUES (@IdPitanje, @NaslovPitanja, @Pitanje, @DatumPrijedloga, @IdKorisnik, @Status); SELECT SCOPE_IDENTITY();");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@IdPitanje", Convert.ToInt32(Question));
                cmd.Parameters.AddWithValue("@NaslovPitanja", txtPrijedlogNaslova.Text);
                cmd.Parameters.AddWithValue("@Pitanje", txtPrijedlogPitanja.Text);
                cmd.Parameters.AddWithValue("@IdKorisnik", Convert.ToInt32(Session["UserId"]));
                cmd.Parameters.AddWithValue("@DatumPrijedloga", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@Status", "1"); //1 = Predlozeno

                connection.Open();
                cmd.ExecuteNonQuery();
                // LastId = Convert.ToInt16(cmd.ExecuteScalar());

                //  MessageBox.Show("Prijedlog unesen. Vlasnik članka će razmotriti prijedlog izmjena.", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Prijedlog unesen. Vlasnik pitanja će razmotriti prijedlog izmjena.');", true);

               // connection.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("GREŠKA, Prijedlog NIJE unesen!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('GREŠKA, Prijedlog NIJE unesen!');", true);
            }
            finally { connection.Close(); }

        }
    }
}