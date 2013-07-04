using System;
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
using SolrNet.Commands;
using SolrNet.Attributes;
using SolrNet;
using Microsoft.Practices.ServiceLocation;
using System.Net;
using System.IO;



namespace KMWeb
{
    public partial class _Default : System.Web.UI.Page
    {

        public class Product
        {
            [SolrUniqueKey("id")]
            public string Id { get; set; }

            [SolrField("manu_exact")]
            public string Manufacturer { get; set; }

            [SolrField("cat")]
            public ICollection<string> Categories { get; set; }

            [SolrField("price")]
            public decimal Price { get; set; }

            [SolrField("inStock")]
            public bool InStock { get; set; }
        }


        internal class Article
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

        }



        public void Add()
        {
           /* var p = new Product
            {
                Id = "SP2514N",
                Manufacturer = "Samsung Electronics Co. Ltd.", Categories = new[] {"electronics","hard drive",},
                Price = 92,
                InStock = true,
            };

            var p1 = new Product
            {
                Id = "SP2514A",
                Manufacturer = "Nedim Samsung",
                Categories = new[] { "electronics", "hard drive", },
                Price = 192,
                InStock = true,
            };*/
            
            var article = new Article
            {              
              articleId = 1, 
              articleTitle = "Naslov 1",
              articleText = "U ovom tekstu postiji rijec mrva",          
            };

            var article1 = new Article
            {
                articleId = 2,
                articleTitle = "Naslov 2",
                articleText = "Ovo je samo tekst za pretragu za primejr i u njemu je moguce pronaci mrvu. jedna mrva",
            };


            //ISolrOperations<Product> solrProduct = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
            ISolrOperations<Article> solrArticle = ServiceLocator.Current.GetInstance<ISolrOperations<Article>>();


            //solrArticle.Delete(SolrQuery.All);

           // var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
           // var solr1 = ServiceLocator.Current.GetInstance<ISolrOperations<Article>>();
            solrArticle.Add(article);
            solrArticle.Add(article1);

            solrArticle.Commit();
            //var products = solrProduct.Query(new SolrQueryByRange<decimal>("price", 10m, 200m));
        }


        public void deleteProduct()
        {
            ISolrOperations<Product> solrProduct = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();

            solrProduct.Delete("SP2514N");

            solrProduct.Commit();
            
        }

        public void FixtureSetup()
        {
            try
            {
                Startup.Init<Article>("http://localhost:8983/solr");
            }
            catch (Exception ex) { }
            //Add();
        }
      



        private static void WriteDocument()
        {

            Lucene.Net.Store.Directory directory = FSDirectory.Open("E:\\GitHub\\LuceneIndex");
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter writer = new IndexWriter(directory, analyzer,IndexWriter.MaxFieldLength.UNLIMITED);

            //Directory directory = FSDirectory.Open("E:\\GitHub\\LuceneIndex");


            //Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

            //var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            Document doc = new Document();
            
            //doc.Add(new Field("id", "1",Field.Store.YES,Field.Index.NOT_ANALYZED ));

            doc.Add(new Field("postBody", "brazil", Field.Store.YES,Field.Index.ANALYZED));
            doc.Add(new Field("population", "70.000", Field.Store.YES, Field.Index.ANALYZED));

            //doc.Add(new Field("id", "2", Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("postBody", "argentina", Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("population", "50.000", Field.Store.YES, Field.Index.ANALYZED));


            //doc.Add(new Field("id", "3", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("postBody", "bih", Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("population", "4.000", Field.Store.YES, Field.Index.ANALYZED));


           // doc.Add(new Field("id", "4", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("postBody", "bihac", Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("population", "1.000", Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);

            writer.Optimize();
           
            writer.Commit();

            writer.Dispose();
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
                HyperLinkKM.NavigateUrl = "CategoryView.aspx?CategoryId=1";
                HyperLinkWEBP.NavigateUrl = "CategoryView.aspx?CategoryId=2";
                HyperLinkStatistika.NavigateUrl = "CategoryView.aspx?CategoryId=3";
                HyperLinkNoCat.NavigateUrl = "CategoryView.aspx?CategoryId=4";
                
                if (!IsPostBack)
                {FixtureSetup();
                   // WriteDocument();
                   // Startup.Init<Article>("http://localhost:8983/solr/km");                  
                }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/NewArticle.aspx");
        }

        protected void LinkButton1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/NewArticle.aspx");
        }

        protected void btnSearchBox_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SearchResults.aspx?WordToSearch=" + txtSearchBox.Text);
            //Query();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            //deleteProduct();  //Ova funkcija radi ali je necu pozivati
            
            string sURL;
            sURL = "http://localhost:8983/solr/dataimport?command=full-import";

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
         
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Article>>();

            var article = solr.Delete(new SolrQuery("*:*"));
            solr.Commit();
           /* string sURL;
            sURL = "http://localhost:8983/solr/update?commit=true -d  '<delete><query>*:*</query></delete>'";

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();*/
             

        }
    }
}
