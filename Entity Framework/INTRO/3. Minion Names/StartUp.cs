using Microsoft.Data.SqlClient;
using System;

namespace _3._Minion_Names
{
    class StartUp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please choose a villain ID to show you the list of his minions : ");
            int id = int.Parse(Console.ReadLine());
            string connectionString = @"Server=.;Database=MinionsDB;Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                using (connection)
                {
                    string queryText = @"SELECT Name FROM Villains WHERE Id = @Id";
                    SqlCommand sqlCommand = new SqlCommand(queryText, connection);
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    using (sqlCommand)
                    {
                        try
                        {
                            string villainsName = (string)sqlCommand.ExecuteScalar();
                            if (villainsName != null)
                            {
                                Console.WriteLine($"Villain: {villainsName}");
                            }
                            else
                            {
                                Console.WriteLine($"No villain with ID {id} exists in the database.");
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("There was an error searching a villain with that ID");
                            Console.WriteLine(e.Message);
                        }
                    }

                    queryText = @"SELECT ROW_NUMBER() 
                                         OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                    WHERE mv.VillainId = @Id
                                    ORDER BY m.Name";
                    sqlCommand = new SqlCommand(queryText, connection);
                    sqlCommand.Parameters.AddWithValue("@Id", id);

                    using (sqlCommand)
                    {
                        try
                        {
                            SqlDataReader reader = sqlCommand.ExecuteReader();
                            int count = 1;
                            while (reader.Read())
                            {
                                string minionName = (string)reader["Name"];
                                int age = (int)reader["Age"];
                                Console.WriteLine($"{count}. {minionName} {age}");
                                count++;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("(no minions)");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection failed!");
                Console.WriteLine(e.Message);
            }

        }
    }
}
