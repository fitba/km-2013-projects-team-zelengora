using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace KMWeb.QA
{
    public partial class QuestionList : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        QAQuestions pitanje=null;  //=new QAQuestions();
        QAKeyword kljucnaRijec = null;
        List<QAQuestions> Pitanja = new List<QAQuestions>();
        List<QAKeyword> KljucneRijeci = new List<QAKeyword>();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //if(!IsPostBack)
            loadQuestions();

        }

        protected void loadQuestions()
        {
            DataSet ds = new DataSet();
            DataSet dsTags = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtTag = new DataTable();
            DataRow dr = null;
            DataRow drTag = null;
            SqlCommand cmd = new SqlCommand("  SELECT TOP (10) dbo.Korisnici.KorisnickoIme, dbo.QAPitanja.NaslovPitanja, dbo.QAPitanja.Pitanje, dbo.QAPitanja.Datum, "+
                 "dbo.QAPitanja.Pregleda, dbo.QAPitanja.UkupnaOcjena, dbo.QAPitanja.Id AS IdPitanje FROM dbo.Korisnici INNER JOIN " +
                 "dbo.QAPitanja ON dbo.Korisnici.Id = dbo.QAPitanja.IdKorisnik ORDER BY dbo.QAPitanja.Datum DESC", connection);

            dt.Columns.Add("IdPitanje");
            dt.Columns.Add("NaslovPitanja");
            dt.Columns.Add("Pitanje");
            dt.Columns.Add("Datum");
            dt.Columns.Add("Pregleda");
            dt.Columns.Add("UkupnaOcjena");
            dt.Columns.Add("KorisnickoIme");

            dtTag.Columns.Add("Id");
            dtTag.Columns.Add("Rijec");

            connection.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds, "QAPitanja");
                gvQAPitanja.DataSource = ds;
                gvQAPitanja.DataBind();

                da.Fill(dt); 

                //SqlDataReader reader = cmd.ExecuteReader();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
              
                {
                    dr = dt.NewRow();
                    dr["IdPitanje"] = ds.Tables[0].Rows[i]["IdPitanje"].ToString();
                    dr["NaslovPitanja"] = ds.Tables[0].Rows[i]["NaslovPitanja"].ToString();
                    dr["Pitanje"] = ds.Tables[0].Rows[i]["Pitanje"].ToString();
                    dr["Datum"] = ds.Tables[0].Rows[i]["Datum"].ToString();
                    dr["Pregleda"] = ds.Tables[0].Rows[i]["Pregleda"].ToString();
                    dr["UkupnaOcjena"] = ds.Tables[0].Rows[i]["UkupnaOcjena"].ToString();
                    dr["KorisnickoIme"] = ds.Tables[0].Rows[i]["KorisnickoIme"].ToString();

                    pitanje = new QAQuestions();
                    

                    pitanje.Id = Convert.ToInt32(dr["IdPitanje"].ToString());
                    pitanje.NaslovPitanja = dr["NaslovPitanja"].ToString();
                    pitanje.Pitanje = dr["Pitanje"].ToString();
                    pitanje.Datum = dr["Datum"].ToString();
                    pitanje.Pregleda = Convert.ToInt32( dr["Pregleda"].ToString());
                    pitanje.UkupnaOcjena = Convert.ToInt32(dr["UkupnaOcjena"].ToString());
                    pitanje.KorisnickoIme = dr["KorisnickoIme"].ToString();


                    // Tražim kljucne rijeci za svako pitanje

                    SqlCommand cmdTags = new SqlCommand("SELECT dbo.QAKljucneRijeci.Rijec, dbo.QAKljucneRijeci.Id, dbo.QAKljucnaRijecQAPitanje.IdPitanje"
                    + " FROM  dbo.QAKljucnaRijecQAPitanje INNER JOIN dbo.QAKljucneRijeci ON dbo.QAKljucnaRijecQAPitanje.IdKljucnaRijec = dbo.QAKljucneRijeci.Id "
                    + " INNER JOIN  dbo.QAPitanja ON dbo.QAKljucnaRijecQAPitanje.IdPitanje = dbo.QAPitanja.Id Where dbo.QAKljucnaRijecQAPitanje.IdPitanje = @IdPitanje", connection);
                    cmdTags.Parameters.AddWithValue("@IdPitanje", pitanje.Id);
                    SqlDataAdapter daTags = new SqlDataAdapter(cmdTags);
                    dsTags = new DataSet();
                    daTags.Fill(dsTags, "QAKljucneRijeci");
                    daTags.Fill(dtTag);

                    KljucneRijeci = new List<QAKeyword>();
                    for (int j = 0; j < dsTags.Tables[0].Rows.Count; j++)
                    {
                        drTag = dtTag.NewRow();
                        drTag["Id"] = dsTags.Tables[0].Rows[j]["Id"].ToString();
                        drTag["Rijec"] = dsTags.Tables[0].Rows[j]["Rijec"].ToString();
                        kljucnaRijec = new QAKeyword();

                        kljucnaRijec.Id = Convert.ToInt32(drTag["Id"].ToString());
                        kljucnaRijec.Key = drTag["Rijec"].ToString(); ;
                        KljucneRijeci.Add(kljucnaRijec);
                    }
                    
                  if (dsTags.Tables[0].Rows.Count!=0)
                    pitanje.KljucnaRijec = KljucneRijeci;
                 
                  // Tražim broj odgovora za svako pitanje

                  SqlCommand cmdBrojOdgovora = new SqlCommand("SELECT COUNT(dbo.QAOdgovori.IdPitanje) AS BrojOdgovora "
                                    + " FROM dbo.QAOdgovori INNER JOIN "
                                    + " dbo.QAPitanja ON dbo.QAOdgovori.IdPitanje = dbo.QAPitanja.Id Where dbo.QAOdgovori.IdPitanje = @IdPitanje", connection);
                  cmdBrojOdgovora.Parameters.AddWithValue("@IdPitanje", pitanje.Id);

                  SqlDataReader readerBrojOdgovora = cmdBrojOdgovora.ExecuteReader();
                  if (readerBrojOdgovora.Read())
                  {
                      pitanje.BrojOdgovora = Convert.ToInt32(readerBrojOdgovora["BrojOdgovora"].ToString());
                  }
                  readerBrojOdgovora.Close();

                    Pitanja.Add(pitanje);
                }

                //gvQAPitanja.Columns[0].Visible = false;
            }
            catch (Exception e) { }
            finally
            {
                connection.Close();
            }

            Repeater1.DataSource = Pitanja;
            Repeater1.DataBind();
        }

        protected void gvQAPitanja_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            string username = (string)Session["korisnickoIme"];
            if (username != null)
            {
                Response.Redirect("~/QA/NewQuestion.aspx");
            }
            else { ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logiraj se');", true); }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }

        protected void Repeater1_ItemDataBound1(object sender, RepeaterItemEventArgs e)
        {
             if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DataRowView row = (DataRowView)e.Item.DataItem;

                QAQuestions temp = new QAQuestions();
                Repeater nestedRepeater = e.Item.FindControl("rep") as Repeater;
                Object dataItem = e.Item.DataItem;
                temp = (QAQuestions) dataItem;

                List<QAKeyword> tempKey = temp.KljucnaRijec;
                //dataItem("KorisnickoIme")

                //KljucneRijeci = e.Item.DataItem;
                nestedRepeater.DataSource = tempKey;
                nestedRepeater.DataBind();
            }
        }
    }
}