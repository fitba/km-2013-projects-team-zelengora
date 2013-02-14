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
                /*TextBox3.Text = reader["BRANCH"].ToString();
                TextBox4.Text = reader["ADDRESS"].ToString();
                TextBox5.Text = reader["PHNO"].ToString();
                TextBox6.Text = reader["STATE"].ToString();*/
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
            SqlCommand cmd = new SqlCommand("Select Id,Pitanje,Datum from Pitanja where IdClanak="+ IdClanak, connection);
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
                           // = ds.Tables[0].Rows[i]["Pitanje"].ToString();
                            //dr["Answer"] = txtAnswer.Text;
                           
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                            
                            gvPitanjaOdgovori.DataSource = dt;
                            gvPitanjaOdgovori.DataBind();
                           // gvPitanjaOdgovori.Rows[0].Cells[0].BackColor = System.Drawing.Color.Red;
                        }


                        idpitanje = (int)ds.Tables[0].Rows[i]["Id"];
                        cmd2 = new SqlCommand("Select Id,Odgovor,Datum from Odgovori where IdPitanje=" + idpitanje, connection);
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
                                //dr["Answer"] = txtAnswer.Text;
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
           
           
            if (s1 == "1")
            {
                txtSelectedType.Text = s1;
                btnPitanje.Text = "Postavi Odgovor";
                
            }
            else
            {
                txtSelectedIndex.Text = "";                
            }

            btnUndo.Enabled = false;
        }

        protected void btnVote_Click(object sender, EventArgs e)
        {
            // UNOS OCJENE PITANJA
            if (txtSelectedType.Text=="1")
            {
            
              try
              {
                SqlCommand cmd = new SqlCommand("Insert into PitanjeOcjenaPitanja(IdPitanja,IdOcjene,IdKorisnik) VALUES (@IdPitanja,@IdOcjene,@IdKorisnika)");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@IdPitanja", Convert.ToInt32(txtSelectedIndex.Text));
                cmd.Parameters.AddWithValue("@IdOcjene", Convert.ToInt32(DropDownListVote.SelectedValue));
                cmd.Parameters.AddWithValue("@IdKorisnika", 1);
                connection.Open();
                cmd.ExecuteNonQuery();


                MessageBox.Show("Pitanje Uspješno ocjenjeno", "Important Message");
                
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
                    cmd.Parameters.AddWithValue("@IdKorisnik", 1);
                    connection.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Odgovor Uspješno ocjenjen", "Important Message");
                    txtSelectedIndex.Text = "";
                    
                }
                catch (Exception ex) { MessageBox.Show("Odgovor Nije Uspješno ocjenjeno! Već ste ocjenili članak", "Important Message"); }
            }

            txtSelectedIndex.Text = "";
            btnVote.Enabled = false;
            btnPitanje.Text = "Postavi pitanje" ;
            DropDownListVote.Enabled = false;
        }

        protected void txtNaslov_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            //OCJENA CLANKA
            string Article = Request.QueryString["ArticleId"];
            try
            {
                SqlCommand cmd = new SqlCommand("Insert into ClanakOcjenaClanka(IdClanak,IdOcjena,IdKorisnik) VALUES (@IdClanak,@IdOcjena,@IdKorisnik)");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@IdClanak", Convert.ToInt32(Article));
                cmd.Parameters.AddWithValue("@IdOcjena", Convert.ToInt32(DropDownListVoteArticle.SelectedValue));
                cmd.Parameters.AddWithValue("@IdKorisnik", 1);
                connection.Open();
                cmd.ExecuteNonQuery();
                
                MessageBox.Show("Članka Uspješno ocjenjen", "Important Message");
                refreshVotingData();
            }
            catch (Exception ex) { MessageBox.Show("Članka Nije Uspješno ocjenjen! Već ste ocjenili članak", "Important Message"); }
       
        }

        protected void btnPitanje_Click(object sender, EventArgs e)
        {
            string Article = Request.QueryString["ArticleId"];
            //UNOS PITANJA ZA ČLANAK
            if (txtSelectedIndex.Text == "")
            {
               if (txtPitanje.Text!="")
               {
                
                try
                {
                    SqlCommand cmd = new SqlCommand("Insert into Pitanja(Pitanje,Datum,IdClanak,IdKorisnik) VALUES (@Pitanje,@Datum,@IdClanak,@IdKorisnik)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@Pitanje", txtPitanje.Text);
                    cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdClanak", Convert.ToInt32(Article));
                    cmd.Parameters.AddWithValue("@IdKorisnik", 1);
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
               else {MessageBox.Show("Unijeti pitanje", "Important Message");}
            }
            else //UNOS ODGOVORA ZA PITANJE
            {
               if (txtPitanje.Text!="")
               {
                   
                try
                {
                    SqlCommand cmd = new SqlCommand("Insert into Odgovori(Odgovor,IdPitanje,IdKorisnik,Datum) VALUES (@Odgovor,@IdPitanje,@IdKorisnik,@Datum)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@Odgovor", txtPitanje.Text);
                    cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdPitanje", Convert.ToInt32(txtSelectedIndex.Text));
                    cmd.Parameters.AddWithValue("@IdKorisnik", 1);
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