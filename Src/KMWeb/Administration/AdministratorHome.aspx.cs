﻿using System;
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
    public partial class AdministratorHome : System.Web.UI.Page
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        protected void Page_Load(object sender, EventArgs e)
        {
            //get
            string username = (string)Session["korisnickoIme"];

            string strSel;
            string UserID = Request.QueryString["UserID"];
            if (username != null)
            {
                DataSet ds = new DataSet();

                connection.Open();

                SqlCommand cmd = new SqlCommand
                ("SELECT Clanci.Id AS IdOriginalClanka, Clanci.Naslov AS OriginalNaslov, Clanci.Sadrzaj AS OriginalSadrzaj, Clanci.DatumKreiranja AS DatumKreiranjaClanka, "
                  + "PrijeglogRevizije.Id AS IdPrijedlogaRevizije, PrijeglogRevizije.Naslov AS NaslovPrijedloga, PrijeglogRevizije.Sadrzaj AS SadrzajPrijedloga, "
                  + "PrijeglogRevizije.DatumPrijedloga AS DatumPrijedloga, PrijeglogRevizije.IdKorisnik, Korisnici.Ime AS ImePredlagaca, Korisnici.Prezime AS PrezimePredlagaca, PrijeglogRevizije.Status,"
                  + "Clanci.IdKorisnik AS AutorClanka "
                  + "FROM Korisnici INNER JOIN "
                  + "PrijeglogRevizije ON Korisnici.Id = PrijeglogRevizije.IdKorisnik INNER JOIN "
                  + "Clanci ON PrijeglogRevizije.IdClanak = Clanci.Id "
                  + "WHERE (PrijeglogRevizije.Status = 1) AND (Clanci.IdKorisnik =" + UserID + ")", connection); //SAMO za jednog korisnika

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                /* da.Fill(ds);
                   gvPrijedloziRevizije.DataSource = ds;
                   gvPrijedloziRevizije.DataBind();
                */

                DataTable dt = new DataTable();
                DataTable dataTable = new DataTable();
                DataRow dr;
                dt.Columns.Add("Id Clanka");
                dt.Columns.Add("OriginalNaslov");
                dt.Columns.Add("DatumPrijedloga");
                dt.Columns.Add("ImePredlagaca");
                dt.Columns.Add("PrezimePredlagaca");
                dt.Columns.Add("Status");
                dt.Columns.Add("Id Prijedloga");

                //dt.Columns.Add("josjednacolona");
                da.Fill(dataTable);

                da.Fill(ds, "Prijedlozi");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (dataTable != null)
                    {

                        dr = dt.NewRow();
                        dr["Id Clanka"] = ds.Tables[0].Rows[i]["IdOriginalClanka"].ToString();
                        dr["OriginalNaslov"] = ds.Tables[0].Rows[i]["OriginalNaslov"].ToString();
                        dr["DatumPrijedloga"] = ds.Tables[0].Rows[i]["DatumPrijedloga"].ToString();
                        dr["ImePredlagaca"] = ds.Tables[0].Rows[i]["ImePredlagaca"].ToString();
                        dr["PrezimePredlagaca"] = ds.Tables[0].Rows[i]["PrezimePredlagaca"].ToString();
                        dr["Status"] = ds.Tables[0].Rows[i]["Status"].ToString();
                        dr["Id Prijedloga"] = ds.Tables[0].Rows[i]["IdPrijedlogaRevizije"].ToString();
                        //dr["Type"] = "Pitanje postavljeno dana: ";
                        // = ds.Tables[0].Rows[i]["Pitanje"].ToString();
                        //dr["Answer"] = txtAnswer.Text;

                        dt.Rows.Add(dr);
                        dt.AcceptChanges();

                        gvPrijedloziRevizije.DataSource = dt;
                        gvPrijedloziRevizije.DataBind();
                        // gvPitanjaOdgovori.Rows[0].Cells[0].BackColor = System.Drawing.Color.Red;
                    }
                }
            
            
            
            }
            else
            {
                MessageBox.Show("Logirajte se !");
                Response.Redirect("~/Account/Login.aspx");
            }

        }

        protected void gvPrijedloziRevizije_SelectedIndexChanged(object sender, EventArgs e)
        {
            String IdArticle = gvPrijedloziRevizije.SelectedRow.Cells[1].Text;
            String IdPrijedlogaRevizije = gvPrijedloziRevizije.SelectedRow.Cells[7].Text;
            Response.Redirect("~/Administration/CompareRevision.aspx?ArticleId=" + IdArticle + "&IdProposal=" + IdPrijedlogaRevizije);
        }
    }
}