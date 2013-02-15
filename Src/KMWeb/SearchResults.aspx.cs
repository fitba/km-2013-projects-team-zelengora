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
            string indexDir = @"E:\\GitHub\\LuceneIndex";

            String query = wordToSearch;
            Directory dir = FSDirectory.Open(indexDir);

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
        protected void Page_Load(object sender, EventArgs e)
        {
            string wordToSearch = Request.QueryString["WordToSearch"];
            SearchSomething(wordToSearch);
        }
    }
}