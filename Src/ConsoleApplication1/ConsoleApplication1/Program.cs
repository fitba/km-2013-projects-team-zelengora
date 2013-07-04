using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ConsoleApplication1
{
    public class Program
    {

        public static List<int> getVotingListForItem1(int itemId1, int itemId2)
        {
            if ((itemId1 == 2) && (itemId2 == 54)) 
            {
                int k = 0;
                k++;
            
            }

            List<int> votingLIst = new List<int>();
            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter I1,I2;
            cmd = new SqlCommand("SELECT Item1.IdClanak, Item1.IdOcjena  AS Item1Ocjena, Item1.IdKorisnik, "+
                                 "Item2.IdClanak AS item2IdClanak, Item2.IdOcjena AS Item2Ocjena, Item2.IdKorisnik AS Item2Korisnik " +
                                 "FROM dbo.ClanakOcjenaClanka AS Item1 INNER JOIN "+
                                 "dbo.ClanakOcjenaClanka AS Item2 ON Item1.IdKorisnik = Item2.IdKorisnik "+
                                 "WHERE (Item1.IdClanak = @ItemId1) AND (Item2.IdClanak = @ItemId2)", connection);

            //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            I1 = cmd.Parameters.Add("@ItemId1", SqlDbType.Int, itemId1);
            I1.Value = itemId1;

            I2 = cmd.Parameters.Add("@ItemId2", SqlDbType.Int, itemId2);
            I2.Value = itemId2;
           
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                    {
                        votingLIst.Add((int)reader["Item1Ocjena"]);
                    }
           reader.Close();
           connection.Close();

            return votingLIst;
        
        }

        public static List<int> getVotingListForItem2(int itemId1, int itemId2)
        {

            List<int> votingLIst = new List<int>();
            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter I1, I2;
            cmd = new SqlCommand("SELECT Item1.IdClanak, Item1.IdOcjena, Item1.IdKorisnik, Item2.IdClanak AS item2IdClanak, Item2.IdOcjena AS Item2Ocjena, Item2.IdKorisnik AS Item2Korisnik " +
                                 "FROM dbo.ClanakOcjenaClanka AS Item1 INNER JOIN " +
                                 "dbo.ClanakOcjenaClanka AS Item2 ON Item1.IdKorisnik = Item2.IdKorisnik " +
                                 "WHERE (Item1.IdClanak = @ItemId1) AND (Item2.IdClanak = @ItemId2)", connection);

            //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            I1 = cmd.Parameters.Add("@ItemId1", SqlDbType.Int, itemId1);
            I1.Value = itemId1;

            I2 = cmd.Parameters.Add("@ItemId2", SqlDbType.Int, itemId2);
            I2.Value = itemId2;
           
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                votingLIst.Add((int)reader["Item2Ocjena"]);
            }
            reader.Close();
            connection.Close();

            return votingLIst;

        }

        public static void deletetSimilarities()
        {

            SqlCommand cmd;
            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);

            cmd = new SqlCommand("DELETE FROM ItemSim");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            //cmd.ExecuteNonQuery();

            cmd.ExecuteNonQuery();
            connection.Close();

        }

        public static void insertSimilarities(int itemId1, int itemId2, double? sim12){

            SqlCommand cmd;
            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);

            /*cmd = new SqlCommand("DELETE FROM ItemSim");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;*/
            connection.Open();
            //cmd.ExecuteNonQuery();

            cmd = new SqlCommand("Insert into ItemSim(IdItem1,IdItem2,SimItem1Item2) VALUES (@itemId1,@itemId2,@sim12)");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;

            cmd.Parameters.AddWithValue("@itemId1", itemId1);
            cmd.Parameters.AddWithValue("@itemId2", itemId2);
            cmd.Parameters.AddWithValue("@sim12", sim12);
           
            
            cmd.ExecuteNonQuery();
            connection.Close();
        
        }

        public static List<int> getListOfVotedItems() 
        {
            List<int> _listOfVotedItems = new List<int>();
           

            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter I1, I2;
            cmd = new SqlCommand(" SELECT DISTINCT IdClanak FROM dbo.ClanakOcjenaClanka", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _listOfVotedItems.Add((int)reader["IdClanak"]);
            }
            reader.Close();
            connection.Close();


            return _listOfVotedItems;
        
        }

        public static int getVoteFor(int IdItem,int IdKorisnik)
        {
           int vote=0;

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

        public static List<int> getListOfVotesGivenToSimilarItems(List<int> predictionList)
        {
            List<int> _listOfVotesGivenToSimilarItems = new List<int>();

            string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter I1, I2;
            cmd = new SqlCommand(" ", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _listOfVotesGivenToSimilarItems.Add((int)reader["IdClanak"]);
            }
            reader.Close();
            connection.Close();


            return _listOfVotesGivenToSimilarItems;
        
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
                
                if (i < predictionList.Count-1)
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

        static void Main(string[] args)
        {
            List<int> item1 = new List<int>();
            List<int> item2 = new List<int>();
            List<int> _listOfVotedItems1 = new List<int>();
            List<int> _listOfVotedItems2 = new List<int>();
            List<ClanakOcjenaClanka> _listClanakOcjenaClanka = new List<ClanakOcjenaClanka>();
            double? p = 0;
            List<SimilarItem> _similarItems = new List<SimilarItem>();

            deletetSimilarities();

            _listOfVotedItems1 = getListOfVotedItems();
            _listOfVotedItems2 = getListOfVotedItems();

            for (int i = 0; i < _listOfVotedItems1.Count; i++)
                for (int j = i+1; j < _listOfVotedItems2.Count; j++)
                {
                    item1 = getVotingListForItem1(_listOfVotedItems1[i], _listOfVotedItems2[j]);
                    item2 = getVotingListForItem2(_listOfVotedItems1[i], _listOfVotedItems2[j]);

                    if (item1.Count != 0) 
                    {
                        p = PearsonSim(item1, item2);
                        insertSimilarities(_listOfVotedItems1[i], _listOfVotedItems2[j], p);
                    }  
                }


            _similarItems = getListOfSimilarItems(1);  // PROMJENITI, STAVITI AKTUALNI ID ITEMA

            List<int> listOfNotVotedItems = new List<int>();
            listOfNotVotedItems = getListOfNotVotedItems(_similarItems, 6); // PROMJENITI, STAVITI AKTUALNI ITEM


            List<PredictedItem> predictionList = new List<PredictedItem>();
            predictionList = getPredictionList(_similarItems,listOfNotVotedItems,6);

            List<int> listOfGivenVotes = new List<int>();
           // listOfGivenVotes = getListOfVotesGivenToSimilarItems(predictionList);


           // Console.Read();
        }


        public static List<SimilarItem> getListOfSimilarItems(int ItemId1)
        {
            List<SimilarItem> _listOfSimilarItems = new List<SimilarItem>();
            SimilarItem _simItem = new SimilarItem();

          string connStr = "Data Source=MOBILE-PC\\SQLEXPRESS;Initial Catalog=KMDatabaseMain;User Id=sa; Password=as";
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            
            cmd = new SqlCommand("SELECT TOP (5) IdItem1, IdItem2, SimItem1Item2 FROM dbo.ItemSim "+
                                 "WHERE (IdItem1 = @ItemId1) "+
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


        public static void liste(List<ClanakOcjenaClanka> l1)
        {

            List<ClanakOcjenaClanka> _lista1 = l1;
            List<ClanakOcjenaClanka> _lista2 = l1;

        }


        public static double? PearsonSim(List<int> item1, List<int> item2)
        {
            
            double? _personSim = 0;
            double _averageVotesItem1;
            double _averageVotesItem2;

            List<double> _difVotesAverageVotesItem1 = new List<double>();
            List<double> _difVotesAverageVotesItem2 = new List<double>();

            List<double> _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2 = new List<double>();

            double? _numerator; // sum of _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2

            List<double> _powerOf_difVotesAverageVotesItem1 = new List<double>();
            List<double> _powerOf_difVotesAverageVotesItem2 = new List<double>();  

            double _sumOf_powerOf_difVotesAverageVotesItem1;
            double _sumOf_powerOf_difVotesAverageVotesItem2;

            double? _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem1;
            double? _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem2;

            _averageVotesItem1 = item1.Average();
            _averageVotesItem2 = item2.Average();


            for (int i=0; i<item1.Count; i++)
               _difVotesAverageVotesItem1.Add(item1[i] - _averageVotesItem1);


            for (int j = 0; j < item2.Count; j++)
            _difVotesAverageVotesItem2.Add(item2[j] - _averageVotesItem2);

            for (int k = 0; k < item2.Count; k++)
            _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2.Add(_difVotesAverageVotesItem1[k] * _difVotesAverageVotesItem2[k]);
        

            _numerator = _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2.Sum();


            for (int z = 0; z < item2.Count; z++)
              _powerOf_difVotesAverageVotesItem1.Add(_difVotesAverageVotesItem1[z] * _difVotesAverageVotesItem1[z]);

            for (int y = 0; y < item2.Count; y++)
              _powerOf_difVotesAverageVotesItem2.Add(_difVotesAverageVotesItem2[y] * _difVotesAverageVotesItem2[y]);
           

            _sumOf_powerOf_difVotesAverageVotesItem1 = (double) _powerOf_difVotesAverageVotesItem1.Sum();
            _sumOf_powerOf_difVotesAverageVotesItem2 = (double) _powerOf_difVotesAverageVotesItem2.Sum();

            _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem1 = Math.Sqrt(_sumOf_powerOf_difVotesAverageVotesItem1);
            _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem2 = Math.Sqrt(_sumOf_powerOf_difVotesAverageVotesItem2);

            double? _denominator = _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem1 * _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem2;

            if (_denominator != 0.0)
                _personSim = _numerator / _denominator;
            else
                _personSim = 0;


            Console.Write(_personSim);

            return _personSim;

        }
    }
}
