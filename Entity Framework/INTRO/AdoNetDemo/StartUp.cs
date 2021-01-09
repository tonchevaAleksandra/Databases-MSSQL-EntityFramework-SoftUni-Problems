using Microsoft.Data.SqlClient;
using System;

namespace AdoNetDemo
{
    class StartUp
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
                int updatedRows = (int)updateSalaryCommand.ExecuteNonQuery();
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

                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string pass = Console.ReadLine();

                using SqlConnection sqlConnection1 = new SqlConnection("Server=.;Database=Service;Integrated Security=true");
                sqlConnection1.Open();

                SqlCommand sqlCommand1 = new SqlCommand("Select count(*) from [Users] Where Username='@Username' and Password='@Password';", sqlConnection1);
                sqlCommand1.Parameters.AddWithValue("@Username", username);
                sqlCommand1.Parameters.AddWithValue("QPassword", pass);

                int usersCount = (int)sqlCommand1.ExecuteScalar();
                if (usersCount > 0)
                    Console.WriteLine("Welcome to our secret data! :)");
                else
                    Console.WriteLine("Access forbidden! :(");
            }
        }
    }
}
