using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace KMWeb
{
    public partial class ArticleView : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
       
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
            string Article = Request.QueryString["ArticleId"];

            DataSet ds = new DataSet();
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT Clanci.Id As Id, Clanci.Naslov AS Naslov, Clanci.Sadrzaj AS Sadrzaj, Clanci.DatumKreiranja AS DatumKreiranja, Korisnici.Id AS IdKorisnik, Korisnici.Ime, Korisnici.Prezime, Korisnici.KorisnickoIme" 
                         +" FROM Clanci INNER JOIN Korisnici ON Clanci.IdKorisnik = Korisnici.Id WHERE Clanci.Id=" + Article, connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            refreshVotingData();


            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtNaslov.Text = reader["Naslov"].ToString();
                txtSadrzaj.Text = reader["Sadrzaj"].ToString();
                txtDate.Text = reader["DatumKreiranja"].ToString();
                lblAutor.Text = reader["Ime"].ToString() + " " + reader["Prezime"].ToString() + " -"; 
                
                reader.Close();
                connection.Close();
            }

            BindData(Convert.ToInt32(Article));
            lblKljucneRijeci.Text = "";
            prikaziKljucneRijeci();
            connection.Close();

        }

        private void prikaziKljucneRijeci()
        { 
          string Article = Request.QueryString["ArticleId"];
          try
          {
              SqlCommand cmdKljucneRijeci = new SqlCommand("spKljucneRijeci");
              cmdKljucneRijeci.CommandType = CommandType.StoredProcedure;
              cmdKljucneRijeci.Connection = connection;

              cmdKljucneRijeci.Parameters.AddWithValue("@IdArticle", Convert.ToInt32(Article));

              connection.Open();
              SqlDataAdapter daKljucneRijeci = new SqlDataAdapter(cmdKljucneRijeci);
              SqlDataReader readerKljucneRijeci = cmdKljucneRijeci.ExecuteReader();
              while (readerKljucneRijeci.Read())
              {
                  lblKljucneRijeci.Text = lblKljucneRijeci.Text + "  -   " + readerKljucneRijeci["KljucnaRijec"].ToString()+ "" ;
                  //lblBrojOcjena.Text = readerBrojPitanja["BrojOcjena"].ToString();
              }
              readerKljucneRijeci.Close();

             // connection.Open();
              //cmdKljucneRijeci.ExecuteNonQuery();

              
          }
          catch (Exception ex) { MessageBox.Show("Greska!", "Important Message"); }
          finally { connection.Close(); }

        }

        private void refreshVotingData ()
        {
            string Article = Request.QueryString["ArticleId"];
            SqlCommand cmdProsjek = new SqlCommand("SELECT AVG(IdOcjena) AS Prosjek, COUNT(IdOcjena) AS BrojOcjena from ClanakOcjenaClanka where IdClanak=" + Article, connection);
            SqlDataAdapter daProsjek = new SqlDataAdapter(cmdProsjek); 
            SqlDataReader readerProsjek = cmdProsjek.ExecuteReader();
            if (readerProsjek.Read())
            {
                lblOcjena.Text = readerProsjek["Prosjek"].ToString();
                lblBrojOcjena.Text = readerProsjek["BrojOcjena"].ToString();
            }
            readerProsjek.Close();

            SqlCommand cmdBrojPitanja = new SqlCommand("SELECT COUNT(Pitanje) AS BrojPitanja, IdClanak FROM Pitanja "
                           +" WHERE IdClanak = " + Article +" GROUP BY IdClanak", connection);
            SqlDataAdapter daBrojPitanja = new SqlDataAdapter(cmdBrojPitanja);
            SqlDataReader readerBrojPitanja = cmdBrojPitanja.ExecuteReader();
            if (readerBrojPitanja.Read())
            {
                lblBrojPitanja.Text = readerBrojPitanja["BrojPitanja"].ToString();
                //lblBrojOcjena.Text = readerBrojPitanja["BrojOcjena"].ToString();
            }
            readerBrojPitanja.Close();
    }

        public void BindData(int IdClanak)
        {
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            SqlCommand cmd = new SqlCommand("Select P.Id AS Id,P.Pitanje AS Pitanje, P.Datum AS Datum, K.Ime AS Ime,"
            + " K.Prezime AS Prezime, "
            + " K.KorisnickoIme AS KorisnickoIme from Pitanja AS P JOIN dbo.Korisnici AS K ON P.IdKorisnik=K.Id" 
            + " where IdClanak=" + IdClanak, connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            SqlCommand cmd2 = null;
            SqlDataAdapter da2;
            DataTable dt = new DataTable();
            int idpitanje;

            Table t = new Table();

            DataTable dataTable = new DataTable();
            DataRow dr;
            dt.Columns.Add("Id");
            dt.Columns.Add("Question");
            dt.Columns.Add("Date");
            dt.Columns.Add("IdType");
            dt.Columns.Add("Type");
            dt.Columns.Add("Username");
            //dt.Columns.Add("Ocjena");
          
            //dt.Columns.Add("josjednacolona");
            da.Fill(dataTable);

            da.Fill(ds, "Pitanja");
            //ds = da.GetOrderStatus(iOrderId);
            if (ds.Tables.Count > 0)
            {
                      
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        if (dataTable != null)
                        {

                            dr = dt.NewRow();
                            dr["Id"] = ds.Tables[0].Rows[i]["Id"].ToString();
                            dr["Question"] = ds.Tables[0].Rows[i]["Pitanje"].ToString();
                            dr["Date"] = ds.Tables[0].Rows[i]["Datum"].ToString();
                            dr["IdType"] = 1;
                            dr["Type"] = "Pitanje postavljeno dana: ";
                            dr["Username"] = ds.Tables[0].Rows[i]["KorisnickoIme"].ToString();
                           // = ds.Tables[0].Rows[i]["Pitanje"].ToString();
                            //dr["Answer"] = txtAnswer.Text;
                           
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                            
                            gvPitanjaOdgovori.DataSource = dt;
                            gvPitanjaOdgovori.DataBind();
                           // gvPitanjaOdgovori.Rows[0].Cells[0].BackColor = System.Drawing.Color.Red;
                        }


                        idpitanje = (int)ds.Tables[0].Rows[i]["Id"];
                        cmd2 = new SqlCommand("Select O.Id AS Id ,O.Odgovor As Odgovor,O.Datum AS Datum, K.KorisnickoIme AS KorisnickoIme"
                        + " from Odgovori AS O JOIN dbo.Korisnici AS K ON O.IdKorisnik=K.Id where IdPitanje=" + idpitanje, connection);
                        da2 = new SqlDataAdapter(cmd2);
                        da2.Fill(ds2, "Odgovori");


                        TableRow tr = new TableRow();
                        TableCell td = new TableCell { Text = ds.Tables[0].Rows[i]["Pitanje"].ToString() };
                        tr.Cells.Add(td);
                        t.Rows.Add(tr);

                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {

                            if (dataTable != null)
                            {

                                dr = dt.NewRow();
                                dr["Id"] = ds2.Tables[0].Rows[j]["Id"].ToString();
                                dr["Question"] = ds2.Tables[0].Rows[j]["Odgovor"].ToString();
                                dr["Date"] = ds2.Tables[0].Rows[j]["Datum"].ToString();
                                dr["IdType"] = 2;
                                dr["Type"] = "Odgovoreno dana: ";
                                dr["Username"] = ds2.Tables[0].Rows[j]["KorisnickoIme"].ToString();
                                dt.Rows.Add(dr);
                                dt.AcceptChanges();

                                gvPitanjaOdgovori.DataSource = dt;
                                gvPitanjaOdgovori.DataBind();
                               // gvPitanjaOdgovori.Rows[0].Cells[0].BackColor = System.Drawing.Color.Yellow;
                            }
                            
                            TableRow tr1 = new TableRow();
                            TableCell td1 = new TableCell { Text = "-------" + ds2.Tables[0].Rows[j]["Odgovor"].ToString() };
                            tr1.Cells.Add(td1);
                            t.Rows.Add(tr1);       
                    


                        }
                        ds2 = new DataSet();
                    }
                    using (StringWriter sw = new StringWriter())
                    {
                        t.RenderControl(new HtmlTextWriter(sw));
                        string html = sw.ToString();
                    }    
             }
            else
            {
                //lblinform.Text = "No data found.";
            }

            for (int d = 0; d<gvPitanjaOdgovori.Rows.Count; d++)
            {
                for (int j = 0; j < gvPitanjaOdgovori.Rows[d].Cells.Count; j++)
                {
                    if (gvPitanjaOdgovori.Rows[d].Cells[3].Text.ToString()=="1")
                        gvPitanjaOdgovori.Rows[d].Cells[2].BackColor = System.Drawing.Color.Lavender;
                    else
                        gvPitanjaOdgovori.Rows[d].Cells[2].BackColor = System.Drawing.Color.Honeydew;

                }
            }
            
            MyPanel.Controls.Add(t);
        }
        private void refreshVotingDataPitanjaIOdgovori(int vrsta, string idPitanjaOdgovora) // vrsta: 1 = pitanje, 2 = odgovor
        {

            string _IdPitanjaOdgovora = idPitanjaOdgovora;
            SqlCommand cmdProsjek;
            if (vrsta==1)
                cmdProsjek = new SqlCommand("SELECT AVG(IdOcjene) AS Prosjek, COUNT(IdOcjene) AS BrojOcjena from PitanjeOcjenaPitanja where IdPitanja=" + _IdPitanjaOdgovora, connection);
            else
                cmdProsjek = new SqlCommand("SELECT AVG(IdOcjena) AS Prosjek, COUNT(IdOcjena) AS BrojOcjena from OdgovoriOcjeneOdgovora where IdOdgovor=" + _IdPitanjaOdgovora, connection);
            connection.Open();
            SqlDataAdapter daProsjek = new SqlDataAdapter(cmdProsjek);
            SqlDataReader readerProsjek = cmdProsjek.ExecuteReader();
            if (readerProsjek.Read())
            {
                lblProsjecnaOcjena.Text = readerProsjek["Prosjek"].ToString();
                lblBrojOcjenaPitanjaOdgovora.Text = readerProsjek["BrojOcjena"].ToString();
            }
            readerProsjek.Close();

            /*SqlCommand cmdBrojPitanja = new SqlCommand("SELECT COUNT(Pitanje) AS BrojPitanja, IdClanak FROM Pitanja "
                           + " WHERE IdClanak = " + Article + " GROUP BY IdClanak", connection);
            SqlDataAdapter daBrojPitanja = new SqlDataAdapter(cmdBrojPitanja);
            SqlDataReader readerBrojPitanja = cmdBrojPitanja.ExecuteReader();
            if (readerBrojPitanja.Read())
            {
                lblBrojPitanja.Text = readerBrojPitanja["BrojPitanja"].ToString();
                //lblBrojOcjena.Text = readerBrojPitanja["BrojOcjena"].ToString();
            }
            readerBrojPitanja.Close();*/
            connection.Close();
        }

        protected void gvPitanjaOdgovori_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            DropDownListVote.Enabled = true;
            btnVote.Enabled = true;
            
            int selectedIdex = gvPitanjaOdgovori.SelectedIndex;
            //string s = gvPitanjaOdgovori.SelectedRow["Id"];

            string s1 = gvPitanjaOdgovori.SelectedRow.Cells[1].Text.ToString();
            txtSelectedIndex.Text = s1;
            s1 = gvPitanjaOdgovori.SelectedRow.Cells[3].Text.ToString();
            txtSelectedType.Text = s1;
            txtTempOdgovor.Text = txtSelectedIndex.Text;
            string IdOdgovora = s1;

            refreshVotingDataPitanjaIOdgovori(Convert.ToInt16(s1), txtSelectedIndex.Text); // 1 = Pitanje
            //refreshVotingDataPitanjaIOdgovori(2); // 2 = Odgovor
           
            if (s1 == "1")
            {
                txtSelectedType.Text = s1;
                btnPitanje.Text = "Odgovori";
                
            }
            else
            {
                btnPitanje.Text = "Postavi pitanje";
                txtSelectedIndex.Text = "";
               
            }

            btnUndo.Enabled = false;
        }

        protected void btnVote_Click(object sender, EventArgs e)
        {
            // UNOS OCJENE PITANJA

            //get
            string username = (string)Session["korisnickoIme"];
            if (username != null)
            {
                if (txtSelectedType.Text == "1")
                {

                    try
                    {
                        SqlCommand cmd = new SqlCommand("Insert into PitanjeOcjenaPitanja(IdPitanja,IdOcjene,IdKorisnik) VALUES (@IdPitanja,@IdOcjene,@IdKorisnika)");
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@IdPitanja", Convert.ToInt32(txtSelectedIndex.Text));
                        cmd.Parameters.AddWithValue("@IdOcjene", Convert.ToInt32(DropDownListVote.SelectedValue));
                        cmd.Parameters.AddWithValue("@IdKorisnika", Convert.ToInt32(Session["UserId"]));
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Pitanje Uspješno ocjenjeno", "Important Message");
                        refreshVotingDataPitanjaIOdgovori(1, txtSelectedIndex.Text);

                    }
                    catch (Exception ex) { MessageBox.Show("Pitanje Nije Uspješno ocjenjeno! Već ste ocjenili pitanje", "Important Message"); }
                }

                // UNOS OCJENE ODGOVORA
                if (txtSelectedType.Text == "2")
                {

                    try
                    {
                        SqlCommand cmd = new SqlCommand("Insert into OdgovoriOcjeneOdgovora(IdOdgovor,IdOcjena,IdKorisnik) VALUES (@IdOdgovor,@IdOcjena,@IdKorisnik)");
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@IdOdgovor", Convert.ToInt32(txtTempOdgovor.Text));
                        cmd.Parameters.AddWithValue("@IdOcjena", Convert.ToInt32(DropDownListVote.SelectedValue));
                        cmd.Parameters.AddWithValue("@IdKorisnik", Convert.ToInt32(Session["UserId"]));
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Odgovor Uspješno ocjenjen", "Important Message");
                        txtSelectedIndex.Text = "";
                        refreshVotingDataPitanjaIOdgovori(2, txtTempOdgovor.Text);

                    }
                    catch (Exception ex) { MessageBox.Show("Odgovor Nije Uspješno ocjenjeno! Već ste ocjenili članak", "Important Message"); }
                }

                txtSelectedIndex.Text = "";
                btnVote.Enabled = false;
                btnPitanje.Text = "Postavi pitanje";
                DropDownListVote.Enabled = false;
                
            }
            else
            {
                MessageBox.Show("Logirajte se !");
                //Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void txtNaslov_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            //OCJENA CLANKA
            string Article = Request.QueryString["ArticleId"];
            //get
            string username = (string)Session["korisnickoIme"];
            if (username != null)
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("Insert into ClanakOcjenaClanka(IdClanak,IdOcjena,IdKorisnik) VALUES (@IdClanak,@IdOcjena,@IdKorisnik)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@IdClanak", Convert.ToInt32(Article));
                    cmd.Parameters.AddWithValue("@IdOcjena", Convert.ToInt32(DropDownListVoteArticle.SelectedValue));
                    cmd.Parameters.AddWithValue("@IdKorisnik", Convert.ToInt32(Session["UserId"]));
                    connection.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Članka Uspješno ocjenjen", "Important Message");
                    refreshVotingData();
                }
                catch (Exception ex) { MessageBox.Show("Članka Nije Uspješno ocjenjen! Već ste ocjenili članak", "Important Message"); }
            }
            else
            {
                MessageBox.Show("Logirajte se !");
                //Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void btnPitanje_Click(object sender, EventArgs e)
        {
            string Article = Request.QueryString["ArticleId"];
            //UNOS PITANJA ZA ČLANAK
            //get
            string username = (string)Session["korisnickoIme"];
            if (username != null)
            {

                if (txtSelectedIndex.Text == "")
                {
                    if (txtPitanje.Text != "")
                    {

                        try
                        {
                            SqlCommand cmd = new SqlCommand("Insert into Pitanja(Pitanje,Datum,IdClanak,IdKorisnik) VALUES (@Pitanje,@Datum,@IdClanak,@IdKorisnik)");
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = connection;
                            cmd.Parameters.AddWithValue("@Pitanje", txtPitanje.Text);
                            cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                            cmd.Parameters.AddWithValue("@IdClanak", Convert.ToInt32(Article));
                            cmd.Parameters.AddWithValue("@IdKorisnik", Convert.ToInt32(Session["UserId"]));
                            connection.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Pitanje Uspješno uneseno", "Important Message");
                            txtSelectedIndex.Text = "";
                        }
                        catch (Exception ex) { MessageBox.Show("Pitanje NIJE uspješno unesenok", "Important Message"); }
                        txtPitanje.Text = "";
                        refreshVotingData();
                        BindData(Convert.ToInt32(Article));
                    }
                    else { MessageBox.Show("Unijeti pitanje", "Important Message"); }
                }
               
            
            else //UNOS ODGOVORA ZA PITANJE
            {
                
                    if (txtPitanje.Text != "")
                    {

                        try
                        {
                            SqlCommand cmd = new SqlCommand("Insert into Odgovori(Odgovor,IdPitanje,IdKorisnik,Datum) VALUES (@Odgovor,@IdPitanje,@IdKorisnik,@Datum)");
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = connection;
                            cmd.Parameters.AddWithValue("@Odgovor", txtPitanje.Text);
                            cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                            cmd.Parameters.AddWithValue("@IdPitanje", Convert.ToInt32(txtSelectedIndex.Text));
                            cmd.Parameters.AddWithValue("@IdKorisnik", Convert.ToInt32(Session["UserId"]));
                            connection.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Odgovor Uspješno unesen", "Important Message");
                            txtSelectedIndex.Text = "";
                            DropDownListVote.Enabled = false;
                            btnVote.Enabled = false;
                        }
                        catch (Exception ex) { MessageBox.Show("Odgovor NIJE uspješno unesen", "Important Message"); }
                        txtPitanje.Text = "";
                        refreshVotingData();
                        btnPitanje.Text = "Postavi Pitanje";
                        BindData(Convert.ToInt32(Article));
                    }
                    else { MessageBox.Show("Unijeti odgovor", "Important Message"); }
                
                
            }
            }
            else
                {
                    MessageBox.Show("Logirajte se !");
                    //Response.Redirect("~/Account/Login.aspx");
                }

            btnUndo.Enabled = false;
            
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            btnPitanje.Text = "Unesi Pitanje";
            txtSelectedIndex.Text = "";
            btnUndo.Enabled = false;
        }

        protected void btnEditArticle_Click(object sender, EventArgs e)
        {
            string Article = Request.QueryString["ArticleId"];
            Response.Redirect("~/Administration/NewRevision.aspx?ArticleId="+Article);
        }

    }
}