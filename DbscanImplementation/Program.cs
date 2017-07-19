using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;


namespace DbscanImplementation
{
    class Program
    {
        static void Main(string[] args)
        {


            MyCustomDatasetItem[] featureData = { };

            /* List<MyCustomDatasetItem> testPoints = new List<MyCustomDatasetItem>();
             for (int i = 0; i < 1000; i++)
             {
                 //points around (1,1) with most 1 distance
                 testPoints.Add(new MyCustomDatasetItem(1, 1 + ((float)i / 1000)));
                 testPoints.Add(new MyCustomDatasetItem(1, 1 - ((float)i / 1000)));
                 testPoints.Add(new MyCustomDatasetItem(1 - ((float)i / 1000), 1));
                 testPoints.Add(new MyCustomDatasetItem(1 + ((float)i / 1000), 1));

                 //points around (5,5) with most 1 distance
                 testPoints.Add(new MyCustomDatasetItem(5, 5 + ((float)i / 1000)));
                 testPoints.Add(new MyCustomDatasetItem(5, 5 - ((float)i / 1000)));
                 testPoints.Add(new MyCustomDatasetItem(5 - ((float)i / 1000), 5));
                 testPoints.Add(new MyCustomDatasetItem(5 + ((float)i / 1000), 5));
             }*/

            List<MyCustomDatasetItem> testPoints = getDataValues();

            featureData = testPoints.ToArray();

            HashSet<MyCustomDatasetItem[]> clusters;

            var dbs = new DbscanAlgorithm<MyCustomDatasetItem>((x, y) => Math.Sqrt(((x.X - y.X) * (x.X - y.X)) + ((x.Y - y.Y) * (x.Y - y.Y))));
            dbs.ComputeClusterDbscan(allPoints: featureData, epsilon: .01, minPts: 1, clusters: out clusters);

            foreach (var clusterouput in clusters)
            {
                MyCustomDatasetItem dsi = clusterouput[0];

            }

        }

        public static List<MyCustomDatasetItem> getDataValues()
        {


            List<MyCustomDatasetItem> testPoints = new List<MyCustomDatasetItem>();
            SqlDataReader datareader = QueryDatabase("SELECT[UUID],[long],[lat],[timestamp]  FROM [statfy_db].[dbo].[statfy_facts]");

            List <string> dataList = new List<string>();

            while (datareader.Read())
            {
                testPoints.Add(new MyCustomDatasetItem((double)datareader[1], (double)datareader[2], (DateTime)datareader[3]));

                //for (int i = 0; i < datareader.FieldCount; i++)
                //{
                //dataList.Add(Convert.ToString(datareader.GetValue(i)));
                //}
            }

            datareader.Close();

            //List<MyCustomDatasetItem> testPoints = new List<MyCustomDatasetItem>();


            /*   for (int i = 0; i < 1000; i++)
               {
                   //points around (1,1) with most 1 distance
                   testPoints.Add(new MyCustomDatasetItem(1, 1 + ((float)i / 1000)));
                   testPoints.Add(new MyCustomDatasetItem(1, 1 - ((float)i / 1000)));
                   testPoints.Add(new MyCustomDatasetItem(1 - ((float)i / 1000), 1));
                   testPoints.Add(new MyCustomDatasetItem(1 + ((float)i / 1000), 1));

                   //points around (5,5) with most 1 distance
                   testPoints.Add(new MyCustomDatasetItem(5, 5 + ((float)i / 1000)));
                   testPoints.Add(new MyCustomDatasetItem(5, 5 - ((float)i / 1000)));
                   testPoints.Add(new MyCustomDatasetItem(5 - ((float)i / 1000), 5));
                   testPoints.Add(new MyCustomDatasetItem(5 + ((float)i / 1000), 5));
               }*/

            return testPoints;

        }



        public static string GetConnectionString()
        {
            // string holding the  DB.
            string managementDatabaseConnectionString = " ";

            try
            {
              
                // get the connection string for the DB. 
                managementDatabaseConnectionString = ConfigurationManager.ConnectionStrings["Statify"].ConnectionString;
                // Check whether the connection string has been populated. 
                if (string.IsNullOrWhiteSpace(managementDatabaseConnectionString))
                {
                    Console.Write("main", "getting management DB Connection String", "Connection String to Management DB Empty", true);
                }
            }
            catch (Exception e)
            {
                Console.Write("main", "getting management DB", "unable to find connection string using registry" + e.ToString(), true);
            }

            return managementDatabaseConnectionString;
        }


        /// <summary>
        /// Query the database using a generic Query function. 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static SqlDataReader QueryDatabase( string queryString)
        {
            SqlConnection con = null;

            try
            {
                con = new SqlConnection(GetConnectionString());

                /*
                DbCommand command = con.CreateCommand();
                command.CommandText = queryString;*/

                // Open the connection.
                con.Open();

                SqlCommand cmd = new SqlCommand(queryString, con);

                Console.Write("DataAccess", "queryDatabase", "Queried " + queryString, false);

                return cmd.ExecuteReader();

            }
            catch (Exception e)
            {
                Console.Write("DataAccess", "queryDatabase", "Connecting to the Database" + e.ToString(), true);
            }

            // return an empty SQLDataReader
            return null;
        }

    }
}