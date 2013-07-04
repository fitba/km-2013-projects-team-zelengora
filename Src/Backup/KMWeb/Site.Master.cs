using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using KMWeb.Reccomender;
using KMWeb.QA;
using KMWeb.Helpers;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Threading.Tasks;
using VRK.Controls;
using System.Web.UI.HtmlControls;


namespace KMWeb
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        SqlConnection connection = new SqlConnection(connStr);
        public string[] args { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string surname = (string)Session["prezime"];
            string name = (string)Session["ime"];
            string korisnicko = (string)Session["korisnickoIme"];
            string Article = Request.QueryString["ArticleId"];
            string Pitanje = Request.QueryString["IdQuestion"];


            int UserId=0;
            if (Session["UserId"] != null)
                UserId = Convert.ToInt32(Session["UserId"]);
           
            if (korisnicko != null)
            {
                lblUserIme.Text = name;
                lblUserPrezime.Text = surname;
                loadZadnjiPregledi();
                linkLogOut.Visible = true;
            }
            loadCategories();
            loadPopularno();

            if ((Article!=null) && (UserId!=0))
                loadInternalReccomended(Convert.ToInt32(Article), UserId);

            if (Pitanje != null)
                loadExternalReccomendationsStackOverflow(Convert.ToInt32(Pitanje));

            if (Article != null)
                loadExternalReccomendationsWiki(Convert.ToInt32(Article));

           // if (Article != null)
                loadTagovi();

               // TagCloud();
            
        }

        protected void TagCloud()
        {
            Dictionary<string, int> categoryTags = new Dictionary<string, int>();
            categoryTags.Add("Science", 100);
            categoryTags.Add("Maths", 80);
            categoryTags.Add("Biology", 190);
            categoryTags.Add("Physics", 70);
            categoryTags.Add("Commerce", 60);
            categoryTags.Add("Behavioral Science", 90);
            categoryTags.Add("Psychology", 40);
            categoryTags.Add("Numismatics", 43);
            categoryTags.Add("Philately", 45);
            categoryTags.Add("English", 28);
            categoryTags.Add("Hindi", 145);
            categoryTags.Add("Oriya", 40);
            categoryTags.Add("French", 10);
            categoryTags.Add("German", 9);
            categoryTags.Add("Sanskrit", 8);
            categoryTags.Add("Telugu", 20);
            categoryTags.Add("Kannara", 2);
            categoryTags.Add("Malyalam", 1);
            categoryTags.Add("Pongal", 0);
            categoryTags.Add("Earth Sciences", 90);
            //TagCloudGenerator tag;
            tagCloud.InnerHtml = new TagCloudGenerator().GetTagCloudHTML(categoryTags);

        }


        private string ExtractJsonResponse(WebResponse response)
        {
            string json;
            using (var outStream = new MemoryStream())
            using (var zipStream = new GZipStream(response.GetResponseStream(),
                CompressionMode.Decompress))
            {
                zipStream.CopyTo(outStream);
                outStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outStream, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }
            }
            return json;
        }

        IEnumerable<QuestionsExternal> GetPitanja(string a)
        {
            List<QuestionsExternal> ls = new List<QuestionsExternal>();

            string url = "http://api.stackoverflow.com/1.1/search?intitle=" + HttpUtility.UrlEncode(a) + "&pagesize=5&sort=votes";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = request.GetResponse();

            string json = ExtractJsonResponse(response);

            JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic d = js.Deserialize<dynamic>(json);


            dynamic[] questions = d["questions"];
            for (int i = 0; i < questions.Length; i++)
            {
                QuestionsExternal q = new QuestionsExternal();
                q.question_timeline_url = questions[i]["question_timeline_url"];
                q.title = questions[i]["title"];
                //q.CompleteURL
                ls.Add(q);
            }

            return ls;
        }

        IEnumerable<WikiExtrenal> GetWikiByXml(string a)
        {

            using (StringReader textReader = new StringReader(a))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader))
                {
                    while (xmlReader.Read())
                    {
                        WikiExtrenal we = new WikiExtrenal();
                        if (xmlReader.Name == "Image")
                            we.Slika = xmlReader.ReadInnerXml();
                        if (xmlReader.Name == "Text")
                            we.Naziv = xmlReader.ReadInnerXml();
                        if (xmlReader.Name == "Description")
                            we.Opis = xmlReader.ReadInnerXml();
                        if (xmlReader.Name == "Url")
                            we.Url = xmlReader.ReadInnerXml();
                        if (we.Naziv == null || we.Url == null)
                            continue;
                        yield return we;
                    }

                }
                yield break;
            }
        }
        IEnumerable<WikiExtrenal> GetRecFromWiki(string a)
        {
            List<WikiExtrenal> ls = new List<WikiExtrenal>();
            HttpWebRequest request
                = WebRequest.Create("http://en.wikipedia.org/w/api.php?action=opensearch&search=" + HttpUtility.UrlEncode(a) + "&limit=5&namespace=0&format=xml") as HttpWebRequest;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";

            string odg;
           
            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                odg = reader.ReadToEnd();
                ls.AddRange(GetWikiByXml(odg));
            }
            return ls;

        }

        public void loadTagovi()
        {
           
            DataSet ds1 = new DataSet();

            SqlCommand cmd = new SqlCommand("SELECT  Rijec, COUNT(Rijec) AS BrojPonavljanja "+
                                            "FROM         dbo.KljucneRijeci "+
                                            " GROUP BY Rijec  " +
                                            "", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds1, "TagPonavljanja");
            
            gvTagovi.DataSource = ds1;
            gvTagovi.DataBind();
           
            connection.Close();
           
            Dictionary<string, int> categoryTags = new Dictionary<string, int>();
            foreach (DataRow myRow in ds1.Tables["TagPonavljanja"].Rows)
            {
                // here you can access myRow object
                //Ex.
               // myRow["columnname"] = "your value";
                if ((int)myRow["BrojPonavljanja"]>=2)
                categoryTags.Add((string)myRow["Rijec"], (int)myRow["BrojPonavljanja"]);
            }

           
           /* categoryTags.Add("Science", 100);
            categoryTags.Add("Maths", 80);
            categoryTags.Add("Biology", 190);
            categoryTags.Add("Physics", 70);
            categoryTags.Add("Commerce", 60);
            categoryTags.Add("Behavioral Science", 90);
            categoryTags.Add("Psychology", 40);
            categoryTags.Add("Numismatics", 43);
            categoryTags.Add("Philately", 45);
            categoryTags.Add("English", 28);
            categoryTags.Add("Hindi", 145);
            categoryTags.Add("Oriya", 40);
            categoryTags.Add("French", 10);
            categoryTags.Add("German", 9);
            categoryTags.Add("Sanskrit", 8);
            categoryTags.Add("Telugu", 20);
            categoryTags.Add("Kannara", 2);
            categoryTags.Add("Malyalam", 1);
            categoryTags.Add("Pongal", 0);
            categoryTags.Add("Earth Sciences", 90);*/
           
            tagCloud.InnerHtml = new TagCloudGenerator().GetTagCloudHTML(categoryTags);



        }

        public void loadExternalReccomendationsWiki(int Clanak)
        {
            List<string> kr = new List<string>();
            SqlCommand cmdKljucneRijeci = new SqlCommand("spKljucneRijeci");
            cmdKljucneRijeci.CommandType = CommandType.StoredProcedure;
            cmdKljucneRijeci.Connection = connection;

            cmdKljucneRijeci.Parameters.AddWithValue("@IdArticle", Convert.ToInt32(Clanak));

            connection.Open();
            SqlDataAdapter daKljucneRijeci = new SqlDataAdapter(cmdKljucneRijeci);
            SqlDataReader readerKljucneRijeci = cmdKljucneRijeci.ExecuteReader();
            while (readerKljucneRijeci.Read())
            {
                kr.Add(readerKljucneRijeci["KljucnaRijec"].ToString());
                //lblKljucneRijeci.Text = lblKljucneRijeci.Text + "  -   " + readerKljucneRijeci["KljucnaRijec"].ToString() + "";
                //lblBrojOcjena.Text = readerBrojPitanja["BrojOcjena"].ToString();
            }
            readerKljucneRijeci.Close();

            List<WikiExtrenal> lista;
            lista = new List<WikiExtrenal>();
            IEnumerable<WikiExtrenal> ls = null;
            if (kr.Count != 0)
            {
                for (int i = 0; i < kr.Count; i++ )
                    lista.AddRange(GetRecFromWiki(kr[i]));
                //ls = GetRecFromWiki(kr[0]);
                if (lista != null)
                {
                    gvWiki.DataSource = lista;
                    gvWiki.DataBind();
                }

            }
        }

        public void loadExternalReccomendationsStackOverflow(int Pitanje)
        {
            // Tražim kljucne rijeci za svako pitanje
            DataSet dsTags = new DataSet();
            DataTable dtTag = new DataTable();
            DataRow drTag = null;
            List<QAKeyword> KljucneRijeci = new List<QAKeyword>();

            SqlCommand cmdTags = new SqlCommand("SELECT dbo.QAKljucneRijeci.Rijec, dbo.QAKljucneRijeci.Id, dbo.QAKljucnaRijecQAPitanje.IdPitanje"
            + " FROM  dbo.QAKljucnaRijecQAPitanje INNER JOIN dbo.QAKljucneRijeci ON dbo.QAKljucnaRijecQAPitanje.IdKljucnaRijec = dbo.QAKljucneRijeci.Id "
            + " INNER JOIN  dbo.QAPitanja ON dbo.QAKljucnaRijecQAPitanje.IdPitanje = dbo.QAPitanja.Id Where dbo.QAKljucnaRijecQAPitanje.IdPitanje = @IdPitanje", connection);
            cmdTags.Parameters.AddWithValue("@IdPitanje", Pitanje);
            SqlDataAdapter daTags = new SqlDataAdapter(cmdTags);
            dsTags = new DataSet();
            daTags.Fill(dsTags, "QAKljucneRijeci");
            daTags.Fill(dtTag);

            KljucneRijeci = new List<QAKeyword>();

            QAKeyword kljucnaRijec = null;
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


            List<QuestionsExternal> lista;
            lista = new List<QuestionsExternal>();

            IEnumerable<QuestionsExternal> ls = null;
            if (KljucneRijeci.Count != 0)
            {
                for (int i = 0; i < KljucneRijeci.Count; i++)
                    lista.AddRange(GetPitanja(KljucneRijeci[i].Key));
               //ls = GetPitanja(KljucneRijeci[0].Key);
               if (lista != null)
               {
                   gvExternal.DataSource = lista;
                   gvExternal.DataBind();
               }
                
            }
        }


        protected void LinkButton1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/NewArticle.aspx");
            
        }

        protected void btnAdminHome_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);  //Useti iz sesije
            Response.Redirect("~/Administration/AdministratorHome.aspx?UserID=" + userId);
        }

        protected void linkLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/ManageCategories.aspx");
        }

        void loadCategories()
        {
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("Select Id,NazivKategorije from KategorijeClanaka WHERE KategorijeClanaka.CancelDate IS NULL", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            da.Fill(ds, "Kategorije");
            gvKategorije.DataSource = ds;
            gvKategorije.DataBind();
            gvKategorije.Columns[0].Visible = false;
            connection.Close();
        }

        protected void gvKategorije_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s1 = gvKategorije.SelectedRow.Cells[1].Text.ToString();
            int CategoryId = (int)gvKategorije.DataKeys[gvKategorije.SelectedIndex].Value;
            Response.Redirect("~/CategoryView.aspx?CategoryId=" + CategoryId);
        }

        protected void btnPodaciOKorisniku_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/UserDetails.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/UserAdministration.aspx");
        }

        protected void loadZadnjiPregledi()
        {
            int userId = Convert.ToInt32(Session["UserId"]);  //Useti iz sesije
            DataSet ds1 = new DataSet();

            SqlCommand cmd = new SqlCommand("Select TOP(5) IdKorisnik,IdClanak,Naslov,DatumZadnjegPregleda from ZadnjiPregledi where IdKorisnik= " + userId +
                " ORDER BY DatumZadnjegPregleda DESC", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds1, "ZadnjiPregledi");
            gvZadnjiPregledi.DataSource = ds1;
            gvZadnjiPregledi.DataBind();
            gvZadnjiPregledi.Columns[0].Visible = false;
            gvZadnjiPregledi.Columns[1].Visible = false;
            connection.Close();
        }

        public static int getVoteFor(int IdItem, int IdKorisnik)
        {
            int vote = 0;

            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter I1, I2;
            cmd = new SqlCommand("SELECT IdOcjena FROM dbo.ClanakOcjenaClanka WHERE IdKorisnik=@IdKorisnik AND IdClanak=@IdItem", connection);
            cmd.Parameters.AddWithValue("@IdKorisnik", IdKorisnik);
            cmd.Parameters.AddWithValue("@IdItem", IdItem);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                vote = ((int)reader["IdOcjena"]);
            }
            reader.Close();
            connection.Close();
            return vote;
        }
        public static List<SimilarItem> getListOfSimilarItems(int ItemId1)
        {
            List<SimilarItem> _listOfSimilarItems = new List<SimilarItem>();
            SimilarItem _simItem = new SimilarItem();

            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("SELECT TOP (5) IdItem1, IdItem2, SimItem1Item2 FROM dbo.ItemSim " +
                                 "WHERE (IdItem1 = @ItemId1) " +
                                 "ORDER BY SimItem1Item2 DESC", connection);
            cmd.Parameters.AddWithValue("@itemId1", ItemId1);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _simItem = new SimilarItem();
                _simItem.IdItem2 = (int)reader["IdItem2"];
                _simItem.SimItem1Item2 = Convert.ToDouble(reader["SimItem1Item2"]);

                if (_simItem.SimItem1Item2 != 0.00)
                    _listOfSimilarItems.Add(_simItem);
            }
            reader.Close();
            connection.Close();


            return _listOfSimilarItems;

        }
        public static List<int> getListOfNotVotedItems(List<SimilarItem> predictionList, int IdKorisnik)
        {
            List<int> _listOfNotVotedItems = new List<int>();
            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            // SqlCommand cmd = new SqlCommand();
            string _itemList = "";
            for (int i = 0; i < predictionList.Count; i++)
            {
                if (i == 0)
                {
                    _itemList = _itemList + "'";
                    _itemList = _itemList + predictionList[i].IdItem2.ToString() + "'"; //lista slicnih itema za pregledati za koje korisnik nije dao ocjenu

                }
                else
                    _itemList = _itemList + "'" + predictionList[i].IdItem2.ToString() + "'"; //lista slicnih itema za pregledati za koje korisnik nije dao ocjenu

                if (i < predictionList.Count - 1)
                    _itemList = _itemList + ",";
            }

            // = cmd.ExecuteReader();

            for (int i = 0; i < predictionList.Count; i++)
            {
                // cmd = new SqlCommand("select IdClanak,IdOcjena,IdKorisnik from dbo.ClanakOcjenaClanka " +
                //                    "where IdClanak NOT IN (" + _itemList + ") AND IdKorisnik=@IdKorisnik", connection);
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("select IdClanak,IdOcjena,IdKorisnik from dbo.ClanakOcjenaClanka " +
                                     "where IdClanak = @itemList  AND IdKorisnik=@IdKorisnik", connection);
                cmd.Parameters.AddWithValue("@IdKorisnik", IdKorisnik);
                cmd.Parameters.AddWithValue("@itemList", predictionList[i].IdItem2);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    //if ((int)reader["IdClanak"] == null)
                    _listOfNotVotedItems.Add(predictionList[i].IdItem2);
                }
                reader.Close();
                connection.Close();
            }

            return _listOfNotVotedItems;
        }
        public static List<PredictedItem> getPredictionList(List<SimilarItem> _similarItems, List<int> listOfNotVotedItems, int IdKorisnik)
        {
            List<PredictedItem> _predictionList = new List<PredictedItem>();
            //PredictedItem _predictedItem = new PredictedItem();

            double? numerator = 0;
            double? denominator = 0;
            int VoteForItem2 = 0;

            for (int k = 0; k < _similarItems.Count; k++)
            {
                VoteForItem2 = getVoteFor(_similarItems[k].IdItem2, IdKorisnik);
                if (VoteForItem2 != 0)
                    denominator = denominator + Math.Abs((double)_similarItems[k].SimItem1Item2);
            }

            for (int i = 0; i < listOfNotVotedItems.Count; i++)
            {
                PredictedItem _predictedItem = new PredictedItem();
                for (int j = 0; j < _similarItems.Count; j++)
                {
                    VoteForItem2 = getVoteFor(_similarItems[j].IdItem2, IdKorisnik);
                    if (VoteForItem2 != 0)
                        numerator = numerator + _similarItems[i].SimItem1Item2 * VoteForItem2;
                }

                _predictedItem.ItemId = listOfNotVotedItems[i];
                _predictedItem.PredictedVOte = numerator / denominator;
                numerator = 0;
                _predictionList.Add(_predictedItem);

            }
            //numerator = _similarItems[i].SimItem1Item2 * _similarItems[]; 
            return _predictionList;
        }

        protected void loadInternalReccomended(int ActualArticle, int UserId)
        {
            int userId = Convert.ToInt32(Session["UserId"]);  //Useti iz sesije
            DataSet ds1 = new DataSet();

            List<SimilarItem> _similarItems = new List<SimilarItem>();
            _similarItems = getListOfSimilarItems(ActualArticle);  // PROMJENITI, STAVITI AKTUALNI ID ITEMA

            List<int> listOfNotVotedItems = new List<int>();
            listOfNotVotedItems = getListOfNotVotedItems(_similarItems, UserId); // PROMJENITI, STAVITI AKTUALNI ITEM

            List<PredictedItem> predictionList = new List<PredictedItem>();
            predictionList = getPredictionList(_similarItems, listOfNotVotedItems, UserId);

            string _itemList = "";
            for (int i = 0; i < predictionList.Count; i++)
            {
                if (i == 0)
                {
                    _itemList = _itemList + "'";
                    _itemList = _itemList + predictionList[i].ItemId.ToString() + "'"; //lista slicnih itema za pregledati za koje korisnik nije dao ocjenu
                }
                else
                    _itemList = _itemList + "'" + predictionList[i].ItemId.ToString() + "'"; //lista slicnih itema za pregledati za koje korisnik nije dao ocjenu

                if (i < predictionList.Count - 1)
                    _itemList = _itemList + ",";
            }
           
            SqlCommand cmd = new SqlCommand("Select TOP(5) Id,Naslov from Clanci " +
                    " WHERE Id = @Id  ", connection);
           SqlDataAdapter da = new SqlDataAdapter(cmd);
           SqlParameter I1;
           int ItemId = 0;
           I1 = cmd.Parameters.Add("@Id", SqlDbType.Int, ItemId);
          // I1.Value = ItemId;

            for (int i = 0; i < predictionList.Count; i++)
            {
                I1.Value = predictionList[i].ItemId;
                da.Fill(ds1, "Clanci");
            }

            if (ds1.Tables.Count != 0)
            {

                GridReccomendations.DataSource = ds1;
                GridReccomendations.DataBind();
                GridReccomendations.Columns[0].Visible = false;
                GridReccomendations.Columns[1].Visible = false;
            }
            connection.Close();
        }

        protected void loadPopularno()
        {
            int userId = Convert.ToInt32(Session["UserId"]);  //Useti iz sesije
            DataSet ds1 = new DataSet();

            SqlCommand cmd = new SqlCommand("Select TOP(5) IdKategorija,IdClanak,Naslov,Pregleda from PopularniClanci " +
                " ORDER BY Pregleda DESC", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds1, "PopularniClanci");
            gvPopularno.DataSource = ds1;
            gvPopularno.DataBind();
            gvPopularno.Columns[0].Visible = false;
            gvPopularno.Columns[1].Visible = false;
            connection.Close();
        }


        protected void gvZadnjiPregledi_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s1 = gvKategorije.SelectedRow.Cells[1].Text.ToString();
            int ArticleId = (int)gvZadnjiPregledi.DataKeys[gvZadnjiPregledi.SelectedIndex].Value;
            Response.Redirect("~/ArticleView.aspx?ArticleId=" + ArticleId);
        }

        protected void gvPopularno_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ArticleId = (int)gvPopularno.DataKeys[gvPopularno.SelectedIndex].Value;
            Response.Redirect("~/ArticleView.aspx?ArticleId=" + ArticleId);
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/CategoryAdministration.aspx");
        }

        protected void GridReccomendations_SelectedIndexChanged(object sender, EventArgs e)
        {

            int ArticleId = (int)GridReccomendations.DataKeys[GridReccomendations.SelectedIndex].Value;
            Response.Redirect("~/ArticleView.aspx?ArticleId=" + ArticleId);
        }

        protected void gvExternal_SelectedIndexChanged(object sender, EventArgs e)
        {
            string question_timeline_url = (string)gvExternal.DataKeys[gvExternal.SelectedIndex].Value;
            Response.Redirect("http://www.stackoverflow.com/" + question_timeline_url);
        }

        protected void gvWiki_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = (string)gvWiki.DataKeys[gvWiki.SelectedIndex].Value;
            Response.Redirect(url);
        }

        protected void gvTagovi_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tag = (string)gvTagovi.DataKeys[gvTagovi.SelectedIndex].Value;
            Response.Redirect("~/SearchResults.aspx?WordToSearch=" + tag);
            

        }

       
    }
}
