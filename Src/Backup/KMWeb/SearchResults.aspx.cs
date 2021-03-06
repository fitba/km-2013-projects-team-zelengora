﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Attributes;
using System.Net;
using SolrNet.Impl;
using System.IO;
using SolrNet.Commands.Parameters;
using System.Text.RegularExpressions;
using SolrNet.Utils;

namespace KMWeb
{
    public partial class SearchResults : System.Web.UI.Page
    {
        internal class Article // ustvari ova klasa mapira rezultate pretrage
        {

            [SolrUniqueKey("articleid")]
            public int articleId { get; internal set; }

            [SolrField("articletitle")]
            public string articleTitle
            { get; internal set; }

            [SolrField("articletext")]
            public string articleText { get; internal set; }

            [SolrField("question")]
            public string question { get; internal set; }

            [SolrField("answer")]
            public string answer { get; internal set; }

            [SolrField("category")]
            public string category { get; internal set; }

            [SolrField("date")]
            public DateTime date { get; internal set; }

            [SolrUniqueKey("QAid")]
            public int QAid { get; internal set; }

            [SolrField("QAquestion")]
            public string QAquestion { get; internal set; }

            [SolrField("QAquestiontitle")]
            public string QAquestiontitle { get; internal set; }

            [SolrField("doc_type")]
            public string doc_type { get; internal set; }

            [SolrField("QAanswer")]
            public string QAanswer { get; internal set; }

            [SolrField("QAAnswerIdPitanje")]
            public string QAAnswerIdPitanje { get; internal set; }

            [SolrField("QANaslovPitanja")]
            public string QANaslovPitanja { get; internal set; }
            
        }

        public static void SearchSomething(string wordToSearch)
        {
            //state the file location of the index
            string indexDir = @"E:\\GitHub\\LuceneIndex";

            String query = wordToSearch;
            Lucene.Net.Store.Directory dir = FSDirectory.Open(indexDir);

            IndexReader reader = IndexReader.Open(dir,true);
            IndexSearcher searcher = new IndexSearcher(reader);

            Lucene.Net.Util.Version v = Lucene.Net.Util.Version.LUCENE_30;

            String defaultField = "postBody";
            Analyzer analyzer = new StandardAnalyzer(v);

            QueryParser parser = new QueryParser(v, defaultField, analyzer);

            Query q = parser.Parse(query);

            TopDocs hits = searcher.Search(q, 10);

            ScoreDoc[] scoreDocs = hits.ScoreDocs;

            for (int n = 0; n < scoreDocs.Length; n++)
            { 
               ScoreDoc sd = scoreDocs[n];
               float score = sd.Score;
               int docId = sd.Doc;
               Document d = searcher.Doc(docId);
               Document d1 = searcher.Doc(hits.ScoreDocs[n].Doc);
               String f = d.Get("postBody");
               String fp = d.Get("population");
            
            }
           
        }

        public void FixtureSetup()
        {
            try
            {
                Startup.Init<Article>("http://localhost:8983/solr");
            }
            catch (Exception ex) { }
        }

        public static string StripHtml(string html, bool allowHarmlessTags)
        {
            if (html == null || html == string.Empty)
                return string.Empty;
            if (allowHarmlessTags)
                return System.Text.RegularExpressions.Regex.Replace(html, "", string.Empty);
            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        public void Query(string WordToSearch)
        {
            string _wordSearchWord = WordToSearch;
            string _articleText;
            string _articleTitle;
            string _question;
            string _answer;
            string _articleId;
            string _url;
            string _category;
            string _date;
            string _QAPitanje;
            string _QAPitanjeNaslov;
            string _QAId;
            string _QAOdgovor;           
            string _QAIdAnswer;
            string _QAAnswerIdPitanje;
            string _QANaslovPitanja;
            int _stringLength=100;
            int _stringLength1 = 100;
            int _limit=0;
            string _docType;
            Table t = new Table();
            lblNotFound.Visible = false;
            if (_wordSearchWord != "")
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Article>>();
              //  _wordSearchWord = Regex.Replace(_wordSearchWord, @"\(", "");
                _wordSearchWord = _wordSearchWord.Trim(new Char[] { '(', ')', '/' });
                var article = solr.Query(new SolrQuery("articletext:" + _wordSearchWord + "~=1" + " articletitle:" + _wordSearchWord + "~=1" + " question:" 
                    + _wordSearchWord + "~=1" + " answer:" + _wordSearchWord + "~=1" + " category:" + _wordSearchWord + "~=1"
                    + " QAquestion:" + _wordSearchWord + "~=1" + " QAquestiontitle:" + _wordSearchWord + "~=1" + " QAanswer:" + _wordSearchWord + "~=1")
                , new QueryOptions
                {
                    Highlight = new HighlightingParameters
                    {
                        Fields = new[] { "articletext" },
                        MaxAnalyzedChars = 10000,
                    }
                });
                 //Rep.DataSource=article;
                 //Rep.DataBind();
                 
                
                if (article.Count == 0)
                {
                    lblNotFound.Visible = true;
                }
                else
                {
                    for (int i = 0; i < article.Count; i++)
                    {
                        _docType = article[i].doc_type.ToString();
                        if (_docType == "CLANAK")
                        {
                            _articleId = article[i].articleId.ToString();
                            _url = "ArticleView.aspx?ArticleId=" + _articleId;
                            _articleTitle = article[i].articleTitle.ToString();
                            TableRow tr = new TableRow();
                            TableCell td = new TableCell { Text = "<a href='" + _url + "'>" + _articleTitle + "</a>" };
                            tr.Cells.Add(td);
                            t.Rows.Add(tr);

                            /* foreach (var h in article.Highlights[article[i].articleId.ToString()])
                             {
                                 TableRow tr1 = new TableRow();
                                 TableCell td1;
                                 td1 = new TableCell { Text = string.Join(",", h.Value.ToArray()) };
                                 tr1.Cells.Add(td1);
                                 t.Rows.Add(tr1);
                             }*/

                            _articleText = article[i].articleText.ToString();
                            // OVAKO  CE biti boldirane u rezultatima pretrage tražene rijeci, ali sadrzaj nece biti prikazan u rezultatima 
                            // pretrage ako je pojam pronadjen u pitanjima ili odgovorima 
                            /* foreach (var h in article.Highlights[article[i].articleId.ToString()])
                             {
                                 TableRow tr1 = new TableRow();
                                 TableCell td1;
                          
                                 td1 = new TableCell { Text = "[Sadrzaj] " + string.Join(",", h.Value.ToArray()) };
                                 tr1.Cells.Add(td1);
                                 t.Rows.Add(tr1);
                             }
                             */

                            // Ovako nista nece biti boldirano, ali ce uvijek biti prikazan sadrzaj, odgovor i ptanje u rezultatima pretrage
                            TableRow tr1 = new TableRow();
                            TableCell td1;
                            Object c = new Object();
                            string s;
                            s = Regex.Replace(_articleText.ToString(), "<(.|\n)*?>", String.Empty);
                            _stringLength = s.Length;
                            if (_stringLength > 200)
                                s = s.Substring(0, 200);
                            td1 = new TableCell { Text = "[Sadrzaj] " + s };
                            tr1.Cells.Add(td1);
                            t.Rows.Add(tr1);


                            if (article[i].question != null)
                            {
                                _question = article[i].question.ToString();
                                TableRow tr4 = new TableRow();
                                TableCell td4;
                                td4 = new TableCell { Text = "[Pitanje] " + _question };
                                tr4.Cells.Add(td4);
                                t.Rows.Add(tr4);
                            }

                            if (article[i].answer != null)
                            {
                                _answer = article[i].answer.ToString();
                                TableRow tr5 = new TableRow();
                                TableCell td5;
                                td5 = new TableCell { Text = "[Odgovor] " + _answer };
                                tr5.Cells.Add(td5);
                                t.Rows.Add(tr5);
                            }

                            _category = article[i].category.ToString();
                            TableRow tr2 = new TableRow();
                            TableCell td2 = new TableCell { Text = "Kategorija: " + _category };
                            tr2.Cells.Add(td2);
                            t.Rows.Add(tr2);

                            _date = article[i].date.ToShortDateString();
                            TableRow tr6 = new TableRow();
                            TableCell td6;
                            td6 = new TableCell { Text = "[Datum Kreiranja] " + _date + "<br>_________________________________________________________________<br>" };
                            tr6.Cells.Add(td6);
                            t.Rows.Add(tr6);


                            /*_QAId = article[i].QAid.ToString();
                            if (_QAId != "")
                            {
                                
                            }*/

                        }
                        if (_docType == "PITANJA") 
                        {
                            _QAId = article[i].QAid.ToString();
                            _url = "QA/QAQuestionShow.aspx?IdQuestion=" + _QAId;
                            _QAPitanjeNaslov = article[i].QAquestiontitle.ToString();
                            //_articleTitle = article[i].articleTitle.ToString();
                            TableRow tr8 = new TableRow();
                            TableCell td8 = new TableCell { Text = "<a href='" + _url + "'>" + _QAPitanjeNaslov + "</a>" + "  [QA Modul]" };
                            tr8.Cells.Add(td8);
                            t.Rows.Add(tr8);

                            _QAPitanje = article[i].QAquestion.ToString();
                            string s1;
                            s1 = Regex.Replace(_QAPitanje.ToString(), "<(.|\n)*?>", String.Empty);
                            _stringLength1 = s1.Length;
                            if (_stringLength1 > 200)
                                s1 = s1.Substring(0, 200);

                           
                            TableRow tr7 = new TableRow();
                            TableCell td7 = new TableCell { Text = "Pitanje: " + s1 + "<br>_________________________________________________________________<br>"};
                            tr7.Cells.Add(td7);
                            t.Rows.Add(tr7);
                        }

                        if (_docType == "ODGOVORI")
                        {
                            _QAAnswerIdPitanje = article[i].QAAnswerIdPitanje.ToString();
                            _url = "QA/QAQuestionShow.aspx?IdQuestion=" + _QAAnswerIdPitanje;
                            _QANaslovPitanja = article[i].QANaslovPitanja.ToString();
                            //_articleTitle = article[i].articleTitle.ToString();
                            TableRow tr9 = new TableRow();
                            TableCell td9 = new TableCell { Text = "<a href='" + _url + "'>" + _QANaslovPitanja + "</a>" + "  [QA Modul]" };
                            tr9.Cells.Add(td9);
                            t.Rows.Add(tr9);

                            _QAOdgovor = article[i].QAanswer.ToString();
                            string s1;
                            s1 = Regex.Replace(_QAOdgovor.ToString(), "<(.|\n)*?>", String.Empty);
                            _stringLength1 = s1.Length;
                            if (_stringLength1 > 200)
                                s1 = s1.Substring(0, 200);


                            TableRow tr10 = new TableRow();
                            TableCell td10 = new TableCell { Text = "Odgovor: " + s1 + "<br>_________________________________________________________________<br>" };
                            tr10.Cells.Add(td10);
                            t.Rows.Add(tr10);
                        }
                    }//Kraj Fora

                    using (StringWriter sw = new StringWriter())
                    {
                        t.RenderControl(new HtmlTextWriter(sw));
                        string html = sw.ToString();
                    }
                    this.MyPanel.Controls.Add(t);
                }

              

                //  var q = new SolrQueryInList("articletext", "nedim", "clanak", "unikat"); //name:solr OR name:samsung OR name:maxtor"
                //article = solr.Query(q); 
                // var article1 = solr.Query(new SolrQuery("articletitle:nedim")); // search for "solr" in the "name" field
                //Assert.AreEqual(1, results.Count);

                /*     if (article.Count == 0)
                     { lblNotFound.Visible = true; }
                     else
                     {

                         for (int i = 0; i < article.Count; i++)
                         {

                             _articleId = article[i].articleId.ToString();
                             _articleText = article[i].articleText.ToString();
                             _articleTitle = article[i].articleTitle.ToString();

                             _url = "ArticleView.aspx?ArticleId=" + _articleId;

                             // txtWordFound.Text = _articleText;

                             TableRow tr = new TableRow();
                             TableCell td1;
                             TableCell td = new TableCell { Text = "<a href='" + _url + "'>" + _articleTitle + "</a>" };
                             //<a href="http://www.w3schools.com">Visit W3Schools.com!</a>

                             tr.Cells.Add(td);
                             TableRow tr1 = new TableRow();
                             _stringLength = _articleText.Length;
                             if (_stringLength > 300) //Dužina teksta koji će se prikazati u rezultatu pretrage
                             {
                                 _limit = 300;

                                 td1 = new TableCell { Text = _articleText.Substring(0, _limit) + "..." };
                             }else
                                 td1 = new TableCell { Text = _articleText.Substring(0, _stringLength) + "" };

                             tr1.Cells.Add(td1);
                             TableRow tr2 = new TableRow();
                             TableCell td2 = new TableCell { Text = "Kategorija:" + "<br>________________________________________________________<br>" };
                             tr2.Cells.Add(td2);

                             t.Rows.Add(tr);
                             t.Rows.Add(tr1);
                             t.Rows.Add(tr2);
                         }

                         using (StringWriter sw = new StringWriter())
                         {
                             t.RenderControl(new HtmlTextWriter(sw));
                             string html = sw.ToString();
                         }
                         this.MyPanel.Controls.Add(t);
                     }
                 }
      */
            }
            else { lblNotFound.Visible = true; }
       
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            FixtureSetup();
            string wordToSearch = Request.QueryString["WordToSearch"];
            txtSearchWord.Text = wordToSearch;
            Query(wordToSearch);
        }
    }
}