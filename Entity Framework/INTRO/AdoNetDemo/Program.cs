using Microsoft.Data.SqlClient;
using System;

namespace AdoNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //string connectionString = "Server=.;Database=SoftUni;User Id=Aleksandra;Password=1234";
            string connectionString = "Server=.;Database=SoftUni;Integrated Security=true";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string command = "SELECT COUNT(*) FROM [Employees]";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
              object result=  sqlCommand.ExecuteScalar();
                Console.WriteLine(result);

            }
                
        }
    }
}
