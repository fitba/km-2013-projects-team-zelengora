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
    public partial class QAQuestionShow : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        List<QAKeyword> KljucneRijeci = new List<QAKeyword>();
        QAKeyword kljucnaRijec = null;
        private void Page_PreInit(object sender, System.EventArgs e)
        {
            if ((Session == null))
            {
                // Do something here
            }
        }
      
        protected void Page_Load(object sender, EventArgs e)
        {
            string Question = Request.QueryString["IdQuestion"];
            showQuestion(Convert.ToInt32(Question));
            showAnswers(Convert.ToInt32(Question));

        }
        protected void updatePregledi(string BrojPregleda)  //Azurira broj pregleda pojedinog clanka
        {
            string _brojPregleda = BrojPregleda;
            string IdPitanja = Request.QueryString["IdQuestion"];
            int i = Convert.ToInt32(_brojPregleda);
            i++;
            connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE QAPitanja SET QAPitanja.Pregleda=" + i + " WHERE QAPitanja.Id=" + IdPitanja, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { connection.Close(); }
        }

        #region ShowQuestion
        protected void showQuestion(int Question)
        {
            DataSet dsTags = new DataSet();

            DataTable dtTag = new DataTable();
            
            DataRow drTag = null;
            QAQuestions pitanje = new QAQuestions();
            List<QAQuestions> Pitanja = new List<QAQuestions>();
            int _question = Question;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            string _brPregleda = "0";
            SqlCommand cmd = new SqlCommand("SELECT dbo.Korisnici.KorisnickoIme, dbo.QAPitanja.NaslovPitanja, dbo.QAPitanja.Pitanje, dbo.QAPitanja.Datum, " +
                 "dbo.QAPitanja.Pregleda, dbo.QAPitanja.UkupnaOcjena, dbo.QAPitanja.Id AS IdPitanje FROM dbo.Korisnici INNER JOIN " +
                 "dbo.QAPitanja ON dbo.Korisnici.Id = dbo.QAPitanja.IdKorisnik Where dbo.QAPitanja.Id=@IdPitanje", connection);
            cmd.Parameters.AddWithValue("@IdPitanje", _question);

            dt.Columns.Add("IdPitanje");
            dt.Columns.Add("NaslovPitanja");
            dt.Columns.Add("Pitanje");
            dt.Columns.Add("Datum");
            dt.Columns.Add("Pregleda");
            dt.Columns.Add("UkupnaOcjena");
            dt.Columns.Add("KorisnickoIme");

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
                    dr["Datum"] = ds.Tables[0].Rows[i]["Datum"].ToString();
                    dr["Pregleda"] = ds.Tables[0].Rows[i]["Pregleda"].ToString();
                    dr["UkupnaOcjena"] = ds.Tables[0].Rows[i]["UkupnaOcjena"].ToString();
                    dr["KorisnickoIme"] = ds.Tables[0].Rows[i]["KorisnickoIme"].ToString();

                    // Azuriranje pregleda ovog PITANJA
                    //string d = reader["Pregleda"].ToString();
                    if (dr["Pregleda"].ToString() != "")
                        _brPregleda = dr["Pregleda"].ToString();
                    else
                        _brPregleda = "0";

                    reader.Close();
                    connection.Close();
                    updatePregledi(_brPregleda); //Azurira broj pregleda pojedinog clanka


                    pitanje = new QAQuestions();

                    pitanje.Id = Convert.ToInt32(dr["IdPitanje"].ToString());
                    pitanje.NaslovPitanja = dr["NaslovPitanja"].ToString();
                    pitanje.Pitanje = dr["Pitanje"].ToString();
                    pitanje.Datum = dr["Datum"].ToString();
                    pitanje.Pregleda = Convert.ToInt32(dr["Pregleda"].ToString());
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


                  /*  if (dsTags.Tables[0].Rows.Count != 0)
                        pitanje.KljucnaRijec = KljucneRijeci;*/



                    // Tražim broj odgovora za svako pitanje

                    SqlCommand cmdBrojOdgovora = new SqlCommand("SELECT COUNT(dbo.QAOdgovori.IdPitanje) AS BrojOdgovora "
                                      + " FROM dbo.QAOdgovori INNER JOIN "
                                      + " dbo.QAPitanja ON dbo.QAOdgovori.IdPitanje = dbo.QAPitanja.Id Where dbo.QAOdgovori.IdPitanje = @IdPitanje", connection);
                    cmdBrojOdgovora.Parameters.AddWithValue("@IdPitanje", pitanje.Id);

                    connection.Open();
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
            Rep.DataSource = KljucneRijeci;
            Rep.DataBind();
        }
        #endregion
        #region SHOW ANSWERS
        protected void showAnswers(int Question)     
        {
            QAAnswers odgovor = new QAAnswers();
            List<QAAnswers> OdgovorList = new List<QAAnswers>();

            int _question = Question;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            SqlCommand cmd = new SqlCommand("SELECT dbo.QAOdgovori.Id, dbo.QAOdgovori.Odgovor, dbo.QAOdgovori.IdPitanje, dbo.QAOdgovori.IdKorisnik,"+
                " dbo.QAOdgovori.Datum, dbo.QAOdgovori.UkupnaOcjena, dbo.Korisnici.KorisnickoIme FROM dbo.QAOdgovori INNER JOIN dbo.QAPitanja ON dbo.QAOdgovori.IdPitanje = dbo.QAPitanja.Id " +
                " INNER JOIN dbo.Korisnici ON dbo.QAOdgovori.IdKorisnik = dbo.Korisnici.Id" +
                " Where dbo.QAPitanja.Id=@IdPitanje", connection);

            cmd.Parameters.AddWithValue("@IdPitanje", _question);

            dt.Columns.Add("Odgovor");
            dt.Columns.Add("Datum");
            dt.Columns.Add("UkupnaOcjena");
            dt.Columns.Add("KorisnickoIme");

            connection.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds, "QAOdgovori");
                da.Fill(dt);

                SqlDataReader reader = cmd.ExecuteReader();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["Id"] = ds.Tables[0].Rows[i]["Id"].ToString();
                    dr["Odgovor"] = ds.Tables[0].Rows[i]["Odgovor"].ToString();
                    dr["Datum"] = ds.Tables[0].Rows[i]["Datum"].ToString();
                    dr["UkupnaOcjena"] = ds.Tables[0].Rows[i]["UkupnaOcjena"].ToString();
                    dr["KorisnickoIme"] = ds.Tables[0].Rows[i]["KorisnickoIme"].ToString();

                    odgovor = new QAAnswers();
                    odgovor.Id = Convert.ToInt32(dr["Id"].ToString());
                    odgovor.Odgovor = dr["Odgovor"].ToString();
                    odgovor.Datum = dr["Datum"].ToString();
                    odgovor.UkupnaOcjena = Convert.ToInt32(dr["UkupnaOcjena"]);
                    odgovor.KorisnickoIme = dr["KorisnickoIme"].ToString();

                    OdgovorList.Add(odgovor);

                }

                //gvQAPitanja.Columns[0].Visible = false;
            }
            catch (Exception e) { }
            finally
            {
                connection.Close();
            }
            RepeaterAnswers.DataSource = OdgovorList;
            RepeaterAnswers.DataBind();
        }

#endregion

        protected void insertAnswer()
        {
            QAAnswers odgovor = new QAAnswers();
            string Question = Request.QueryString["IdQuestion"];
            int UserId = Convert.ToInt32(Session["UserId"]);
            int LastId = 0;
           // odgovor.Odgovor = txtUserAnswer.Text;
            odgovor.Odgovor = CKEditor1.Text;
            odgovor.Datum = DateTime.Now.ToString();
            odgovor.IdPitanje = Convert.ToInt32(Question);
            odgovor.IdKorisnik = UserId ;


            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO QAOdgovori (Odgovor, IdPitanje, IdKorisnik, Datum) VALUES (@Odgovor, @IdPitanje, @IdKorisnik, @Datum); SELECT SCOPE_IDENTITY();");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                cmd.Parameters.AddWithValue("@Odgovor", odgovor.Odgovor);
                cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                cmd.Parameters.AddWithValue("@IdPitanje", odgovor.IdPitanje);
                cmd.Parameters.AddWithValue("@IdKorisnik", odgovor.IdKorisnik);

                connection.Open();
                //cmd.exExecuteNonQuery();
                LastId = Convert.ToInt16(cmd.ExecuteScalar());

                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Odgovor uspješno unesen!');", true);
                txtUserAnswer.Text = "";
                connection.Close();
                // insertKljucneRijeci();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Članak nije unesen!", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Odgovor nije uspješno unesen!');", true);
            }
            finally { connection.Close(); }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
             string username = (string)Session["korisnickoIme"];
             if (username != null)
             {
                 if (CKEditor1.Text != "")
                 {
                     insertAnswer();
                 }
                 else { ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Napisati odgovor!');", true); }
             }
             else { ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logiraj se!');", true); }
        }


        protected void insertQuestionVoteUP(int QuestionId, int TotalVotes)
        { 
            int _IdQuestion=QuestionId;
            int _brojOcjena = TotalVotes;
            //UNOS OCJENE PITANJA

            _brojOcjena++;
          
            
                    try
                    {
                        SqlCommand cmd1 = new SqlCommand("Update QAPitanja SET dbo.QAPitanja.UkupnaOcjena=@UkupnaOcjena Where dbo.QAPitanja.Id=@IdPitanje");
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = connection;
                        cmd1.Parameters.AddWithValue("@IdPitanje", _IdQuestion);
                        cmd1.Parameters.AddWithValue("@UkupnaOcjena", _brojOcjena);
                      
                        connection.Open();
                        cmd1.ExecuteNonQuery();
                        connection.Close();

                        //MessageBox.Show("Pitanje Uspješno ocjenjeno", "Important Message");
                        ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Pitanje Uspješno ocjenjeno');", true);
                        //refreshVotingDataPitanjaIOdgovori(1, txtSelectedIndex.Text);

                    }
                    catch (Exception ex) { 
                       // MessageBox.Show("Pitanje Nije Uspješno ocjenjeno! Već ste ocjenili pitanje", "Important Message");
                        ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Pitanje Nije Uspješno ocjenjeno! Već ste ocjenili pitanje');", true);
                    }
                     finally{connection.Close();}
        }

        protected void insertQuestionVoteDOWN(int QuestionId, int TotalVotes)
        {
            int _IdQuestion = QuestionId;
            int _brojOcjena = TotalVotes;
            //UNOS OCJENE PITANJA

           
            _brojOcjena--;


            try
            {
                SqlCommand cmd1 = new SqlCommand("Update QAPitanja SET dbo.QAPitanja.UkupnaOcjena=@UkupnaOcjena Where dbo.QAPitanja.Id=@IdPitanje");
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@IdPitanje", _IdQuestion);
                cmd1.Parameters.AddWithValue("@UkupnaOcjena", _brojOcjena);

                connection.Open();
                cmd1.ExecuteNonQuery();
                connection.Close();

                //MessageBox.Show("Pitanje Uspješno ocjenjeno", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Pitanje Uspješno ocjenjeno');", true);
                //refreshVotingDataPitanjaIOdgovori(1, txtSelectedIndex.Text);

            }
            catch (Exception ex)
            {
                // MessageBox.Show("Pitanje Nije Uspješno ocjenjeno! Već ste ocjenili pitanje", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Pitanje Nije Uspješno ocjenjeno! Već ste ocjenili pitanje');", true);
            }
            finally { connection.Close(); }
        }


        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //Id Pitanja , IdOcjene, IdKorisnika
            string Question = Request.QueryString["IdQuestion"];
            int UserId = Convert.ToInt32(Session["UserId"]);
            string username = (string)Session["korisnickoIme"];

            string isactivestatus = Convert.ToString(e.CommandArgument);
            string[] arg = new string[2];
            arg = isactivestatus.Split(';');

            int IdPitanje = Convert.ToInt32(arg[0]);
            int UkupnoOcjena = Convert.ToInt32(arg[1]);


            if (username != null)
            {
                switch (e.CommandName)
                {
                    case "Up": insertQuestionVoteUP(IdPitanje, UkupnoOcjena);
                        break;
                    case "Down": insertQuestionVoteDOWN(IdPitanje, UkupnoOcjena);
                        break;
                }
            }
            else { ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logiraj se!');", true); }

            
        
        }

        protected void insertAnswerVoteDOWN(int QuestionId, int TotalVotes)
        {
            int _IdQuestion = QuestionId;
            int _brojOcjena = TotalVotes;
            //UNOS OCJENE PITANJA
            _brojOcjena--;


            try
            {
                SqlCommand cmd1 = new SqlCommand("Update QAOdgovori SET dbo.QAOdgovori.UkupnaOcjena=@UkupnaOcjena Where dbo.QAOdgovori.Id=@IdQuestion");
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@IdQuestion", _IdQuestion);
                cmd1.Parameters.AddWithValue("@UkupnaOcjena", _brojOcjena);

                connection.Open();
                cmd1.ExecuteNonQuery();
                connection.Close();

                //MessageBox.Show("Pitanje Uspješno ocjenjeno", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Odgovor uspješno ocjenjeno');", true);
                //refreshVotingDataPitanjaIOdgovori(1, txtSelectedIndex.Text);

            }
            catch (Exception ex)
            {
                // MessageBox.Show("Pitanje Nije Uspješno ocjenjeno! Već ste ocjenili pitanje", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Odgovor Nije Uspješno ocjenjeno! Već ste ocjenili pitanje');", true);
            }
            finally { connection.Close(); }
        }

        protected void insertAnswerVoteUP(int QuestionId, int TotalVotes)
        {
            int _IdQuestion = QuestionId;
            int _brojOcjena = TotalVotes;
            //UNOS OCJENE PITANJA
            _brojOcjena++;
            try
            {
                SqlCommand cmd1 = new SqlCommand("Update QAOdgovori SET dbo.QAOdgovori.UkupnaOcjena=@UkupnaOcjena Where dbo.QAOdgovori.Id=@IdQuestion");
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@IdQuestion", _IdQuestion);
                cmd1.Parameters.AddWithValue("@UkupnaOcjena", _brojOcjena);

                connection.Open();
                cmd1.ExecuteNonQuery();
                connection.Close();

                //MessageBox.Show("Pitanje Uspješno ocjenjeno", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Odgovor uspješno ocjenjeno');", true);
                //refreshVotingDataPitanjaIOdgovori(1, txtSelectedIndex.Text);

            }
            catch (Exception ex)
            {
                // MessageBox.Show("Pitanje Nije Uspješno ocjenjeno! Već ste ocjenili pitanje", "Important Message");
                ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Odgovor Nije Uspješno ocjenjeno! Već ste ocjenili pitanje');", true);
            }
            finally { connection.Close(); }
        }

        protected void RepeaterAnswers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //Id Pitanja , IdOcjene, IdKorisnika
            string Question = Request.QueryString["IdQuestion"];
            int UserId = Convert.ToInt32(Session["UserId"]);
            string username = (string)Session["korisnickoIme"];

            string isactivestatus = Convert.ToString(e.CommandArgument);

            string[] arg = new string[2];

            arg = isactivestatus.Split(';');
          
            int IdOdgovor = Convert.ToInt32(arg[0]);
            int UkupnoOcjena = Convert.ToInt32(arg[1]);


            
            if (username != null)
            {
                switch (e.CommandName)
                {
                    case "UPAnswer": insertAnswerVoteUP(IdOdgovor, UkupnoOcjena);
                        break;
                    case "DownAnswer": insertAnswerVoteDOWN(IdOdgovor, UkupnoOcjena);
                        break;
                }
            }
            else { ClientScript.RegisterStartupScript(typeof(Page), "myscript", "alert('Logiraj se!');", true); }



            

        }
    }
}