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

        public void Add()
        {
            var p = new Product
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
            };

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
            solr.Add(p);
            solr.Add(p1);

            solr.Commit();
            
        }

        

        public void FixtureSetup()
        {
            Startup.Init<Product>("http://localhost:8983/solr");
        }
      


        public void Query()
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
            var results = solr.Query(new SolrQueryByField("id", "SP2514A"));
            Assert.AreEqual(1, results.Count);
            string a = results[0].Price.ToString();
            //Console.WriteLine(results[0].Price);
        }

        private static void WriteDocument()
        {

            Directory directory = FSDirectory.Open("E:\\GitHub\\LuceneIndex");
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
               if (!IsPostBack) WriteDocument();      
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
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FixtureSetup();
            Add();
            Query();
            //FixtureSetup();
        }
    }
}
