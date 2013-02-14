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
    public partial class SearchResults : System.Web.UI.Page
    {
        public static void SearchSomething(string wordToSearch)
        {
            //state the file location of the index
            string indexFileLocation = @"E:\\GitHub\\TempLucene";
            Lucene.Net.Store.Directory dir =  Lucene.Net.Store.FSDirectory.Open(indexFileLocation);

            //create an index searcher that will perform the search
            Lucene.Net.Search.IndexSearcher searcher = new
            Lucene.Net.Search.IndexSearcher(dir);

            //build a query object
            Lucene.Net.Index.Term searchTerm =
              new Lucene.Net.Index.Term("content", "plan");
            Lucene.Net.Search.Query query = new Lucene.Net.Search.TermQuery(searchTerm);

            //execute the query
           // Lucene.Net.Search.Hits hits = searcher.Search(query);
            Lucene.Net.Search.TopDocs hits = searcher.Search(query,10);
            Document doc = searcher.Doc(0);
            //iterate over the results.
            for (int i = 0; i < hits.TotalHits; i++)
            {
                //Document doc = hits.Doc(i);
              
               // Document doc = searcher.Doc(0);
                string contentValue = doc.Get("content");

                Console.WriteLine(contentValue);

            }

           
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string wordToSearch = Request.QueryString["WordToSearch"];
            SearchSomething(wordToSearch);
        }
    }
}