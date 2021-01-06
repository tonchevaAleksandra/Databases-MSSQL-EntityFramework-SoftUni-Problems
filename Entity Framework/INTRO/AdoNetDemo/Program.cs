using Microsoft.Data.SqlClient;
using System;

namespace AdoNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //string connectionString = "Server=.;Database=SoftUni;User Id=Aleksandra;Password=1234";
            //string connectionString = "Server=.;Database=SoftUni;Integrated Security=true";
            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    string command = "SELECT COUNT(*) FROM [Employees]";
            //    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            //  object result=  sqlCommand.ExecuteScalar();
            //    Console.WriteLine(result);

            //}

            string connectionString = "Server=.;Database=SoftUni;Integrated Security=true";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string command = "SELECT * FROM [Employees] WHERE FirstName like 'N%'";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string firstName = (string)reader["FirstName"];
                        string lastName = (string)reader["LastName"];
                        decimal salary = (decimal)reader["Salary"];
                        Console.WriteLine(firstName + " " + lastName + " => " + salary);
                    }
                }
                SqlCommand updateSalaryCommand = new SqlCommand("UPDATE Employees SET Salary=Salary*0.90 WHERE FirstName like 'N%'", sqlConnection);
                int updatedRows =(int)updateSalaryCommand.ExecuteNonQuery();
                Console.WriteLine($"Slary updated for {updatedRows} employee(e).");

                var reader2 = sqlCommand.ExecuteReader();
                using (reader2)
                {
                    while (reader2.Read())
                    {
                        string firstName = (string)reader2["FirstName"];
                        string lastName = (string)reader2["LastName"];
                        decimal salary = (decimal)reader2["Salary"];
                        Console.WriteLine(firstName + " " + lastName + " => " + salary);
                    }
                }
            }
        }
    }
}
