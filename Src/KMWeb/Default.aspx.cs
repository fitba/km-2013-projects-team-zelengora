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

namespace KMWeb
{
    public partial class _Default : System.Web.UI.Page
    {

        private static void writeDoc ()
        {
          //state the file location of the index
            string indexFileLocation = @"E:\\GitHub\\TempLucene";
            Lucene.Net.Store.Directory dir = Lucene.Net.Store.FSDirectory.Open(indexFileLocation);

//create an analyzer to process the text
Lucene.Net.Analysis.Analyzer analyzer = new
Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30); 

//create the index writer with the directory and analyzer defined.
Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED); 

//create a document, add in a single field
Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();

Lucene.Net.Documents.Field fldContent = 
  new Lucene.Net.Documents.Field("content", 
  "The quick brown fox jumps over the lazy dog",
  Lucene.Net.Documents.Field.Store.YES, 


Lucene.Net.Documents.Field.Index.NOT_ANALYZED, 
Lucene.Net.Documents.Field.TermVector.YES);

doc.Add(fldContent);

//write the document to the index
indexWriter.AddDocument(doc);

//optimize and close the writer
indexWriter.Optimize(); 
indexWriter.Close();

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
    }
}
