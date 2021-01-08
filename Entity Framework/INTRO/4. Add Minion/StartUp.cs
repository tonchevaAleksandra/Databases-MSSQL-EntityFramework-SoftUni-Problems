using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _4._Add_Minion
{
    class StartUp
    {
        static void Main(string[] args)
        {
            List<string> minionsData = ReadData();
            string villainName = ReadData()[0];
            string minionName = minionsData[0];
            int age = int.Parse(minionsData[1]);
            string town = minionsData[2];

            string connectionString = @"Server=.;Database=MinionsDB;Integrated Security=true";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            using (sqlConnection)
            {

                int villainID = FindVillainID(villainName, sqlConnection);
                
            }

        }

        private static int FindVillainID(string villainName, SqlConnection sqlConnection)
        {
            string queryText = @"SELECT Id FROM Villains WHERE Name = @Name";
            SqlCommand findVillainIdByName = new SqlCommand(queryText, sqlConnection);
            findVillainIdByName.Parameters.AddWithValue("@Name", villainName);
            int villainID = 0;
            using (findVillainIdByName)
            {
                try
                {

                    object result = findVillainIdByName.ExecuteScalar();
                    if (result == null)
                    {
                        queryText = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                        SqlCommand insertVillain = new SqlCommand(queryText, sqlConnection);
                        insertVillain.Parameters.AddWithValue("@villainName", villainName);
                        using (insertVillain)
                        {
                            try
                            {
                                findVillainIdByName.ExecuteNonQuery();
                                Console.WriteLine($"Villain { villainName} was added to the database.");
                                villainID = (int)findVillainIdByName.ExecuteScalar();

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("There was an error inserting this villain!");
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    else
                    {
                        villainID = (int)findVillainIdByName.ExecuteScalar();
                    }


                }
                catch (Exception e)
                {

                    Console.WriteLine("There was an error searching this villain!");
                    Console.WriteLine(e.Message);
                }

            }
            return villainID;

        }

        private static List<string> ReadData() =>
            Console.ReadLine()
            .Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToList();


    }
}
