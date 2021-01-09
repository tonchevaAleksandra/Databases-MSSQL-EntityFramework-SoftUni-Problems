using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _4.Add_Minion
{
    public class StartUp
    {
        private static string connectionString = @"Server=.;Database=MinionsDB;Integrated Security=true";
        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            List<string> minionsData = ReadData();
            string villainName = ReadData()[0];
            string minionName = minionsData[0];
            int age = int.Parse(minionsData[1]);
            string town = minionsData[2];

            connection.Open();
            using (connection)
            {
                //SqlTransaction sqlTransaction = connection.BeginTransaction("AddMinionToVillain");
                //SqlCommand command = connection.CreateCommand();
                //command.Connection = connection;
                //command.Transaction = sqlTransaction;

                //try
                //{
                object obj1 = FindTownID(town);
                if (obj1 == null)
                {
                    AddTownToDB(town);
                    obj1 = FindTownID(town);


                    Console.WriteLine(string.Format(Messages.SuccessfullyAddedTown, town));
                }
                int townId = (int)obj1;

                object obj2 = FindVillainID(villainName);
                if (obj2 == null)
                {
                    AddVillainToDB(villainName);
                    obj2 = FindVillainID(villainName);

                    Console.WriteLine(string.Format(Messages.SuccessfullyAddedVillain, villainName));
                }
                int villainId = (int)obj2;

                object obj3 = FindMinionID(minionName);
                if (obj3 == null)
                {
                    AddMinionToDB(minionName, age, townId);
                    obj3 = FindMinionID(minionName);
                }
                int minionId = (int)obj3;


                AddMinionToVillain(minionId, villainId);

                Console.WriteLine(string.Format(Messages.SuccessfullyAddedMinionToVillain, minionName, villainName));

                //sqlTransaction.Commit();

                //}
                //catch (Exception)
                //{
                //    sqlTransaction.Rollback();
                //    Console.WriteLine("The transaction failed! Make sure that your code didn't violate the database");
                //}
            }
        }

        private static void AddMinionToVillain(int minionId, int villainId)
        {
            SqlCommand addMinionToVillain = new SqlCommand(Queries.InsertMinionToVillain, connection);
            addMinionToVillain.Parameters.AddWithValue("@minionId", minionId);
            addMinionToVillain.Parameters.AddWithValue("@villainId", villainId);

            using (addMinionToVillain)
            {
                addMinionToVillain.ExecuteNonQuery();
            }
        }

        private static void AddMinionToDB(string minionName, int age, int townId)
        {
            SqlCommand addMinion = new SqlCommand(Queries.InsertMinion, connection);
            addMinion.Parameters.AddWithValue("@name", minionName);
            addMinion.Parameters.AddWithValue("@age", age);
            addMinion.Parameters.AddWithValue("@townId", townId);

            using (addMinion)
            {
                addMinion.ExecuteNonQuery();
            }
        }

        private static object FindMinionID(string minionName)
        {
            SqlCommand findMinionId = new SqlCommand(Queries.FindMinionID, connection);
            findMinionId.Parameters.AddWithValue("@Name", minionName);

            using (findMinionId)
            {
                object minionID = findMinionId.ExecuteScalar();

                return minionID;
            }
        }

        private static void AddVillainToDB(string villainName)
        {
            SqlCommand addVillain = new SqlCommand(Queries.InsertVillain, connection);
            addVillain.Parameters.AddWithValue("@villainName", villainName);

            using (addVillain)
            {
                addVillain.ExecuteNonQuery();
            }
        }

        private static object FindVillainID(string villainName)
        {
            SqlCommand findVillainId = new SqlCommand(Queries.FindVillainID, connection);
            findVillainId.Parameters.AddWithValue("@Name", villainName);
            using (findVillainId)
            {
                object villainId = findVillainId.ExecuteScalar();

                return villainId;
            }
        }

        private static void AddTownToDB(string townName)
        {
            SqlCommand addTown = new SqlCommand(Queries.InsertTown, connection);
            addTown.Parameters.AddWithValue("@townName", townName);

            using (addTown)
            {
                addTown.ExecuteNonQuery();
            }
        }
        private static object FindTownID(string townName)
        {

            SqlCommand findTownId = new SqlCommand(Queries.FindTownID, connection);

            findTownId.Parameters.AddWithValue("@townName", townName);

            using (findTownId)
            {
                object townId = findTownId.ExecuteScalar();

                return townId;
            }
        }

        private static List<string> ReadData() =>
           Console.ReadLine()
           .Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries)
           .Skip(1)
           .ToList();
    }
}
