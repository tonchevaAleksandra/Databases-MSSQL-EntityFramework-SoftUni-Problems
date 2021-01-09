using Microsoft.Data.SqlClient;
using System;

namespace _6._Remove_Villain
{
    public class StartUp
    {
        private static string connectionString = @"Server=.;Database=MinionsDB;Integrated Security=true";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {

            int villainId = int.Parse(Console.ReadLine());
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(Queries.FindVillainName, connection);
                command.Parameters.AddWithValue("@villainId", villainId);

                using (command)
                {
                    object result = command.ExecuteScalar();

                    if (result == null)
                    {
                        Console.WriteLine("No such villain was found.");
                    }

                    else
                    {
                        string villainName = (string)result;
                        int affectedRows = DeleteVillainFromMV(villainId);
                        DeleteVillainByID(villainId);

                        Console.WriteLine($"{villainName} was deleted.");
                        Console.WriteLine($"{affectedRows} minions were released.");

                    }
                }
            }

        }

        private static void DeleteVillainByID(int villainId)
        {
            SqlCommand sqlCommand = new SqlCommand(Queries.DeleteVillainFromVillains, connection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);

            using (sqlCommand)
            {
                sqlCommand.ExecuteNonQuery();
            }
        }

        private static int DeleteVillainFromMV(int villainId)
        {
            SqlCommand command = new SqlCommand(Queries.DeleteVillainFromMV, connection);
            command.Parameters.AddWithValue("@villainId", villainId);

            using (command)
            {
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows;
            }
        }
    }
}
