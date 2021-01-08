using Microsoft.Data.SqlClient;
using System;

namespace _1._Initial_Setup
{
    class StartUp
    {

        private static string sqlConnectionString = "Server=.;Database={0};Integrated Security=true";

        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionString, "master"));
            sqlConnection.Open();

            using (sqlConnection)
            {
                string queryText = "CREATE DATABASE MinionsDB";
                try
                {

                    SqlCommand createDatabase = new SqlCommand(queryText, sqlConnection);
                    createDatabase.ExecuteNonQuery();

                    Console.WriteLine("Database created successfully!");

                }
                catch (Exception e)
                {
                    Console.WriteLine("There was error creating database!");
                    Console.WriteLine(e.Message);
                    return;
                }
            }


            sqlConnection = new SqlConnection(string.Format(sqlConnectionString, "MinionsDB"));
            sqlConnection.Open();
            using (sqlConnection)
            {
                string queryText = @"CREATE TABLE Countries 
                (
                    Id INT PRIMARY KEY IDENTITY, 
                    Name VARCHAR(50)
                )
 
                CREATE TABLE Towns
                (
                    Id INT PRIMARY KEY IDENTITY, 
                    Name VARCHAR(50), 
                    CountryCode INT FOREIGN KEY REFERENCES Countries(Id)
                )
 
                CREATE TABLE Minions
                (
                    Id INT PRIMARY KEY IDENTITY, 
                    Name VARCHAR(30), 
                    Age INT, 
                    TownId INT FOREIGN KEY REFERENCES Towns(Id)
                )
 
                CREATE TABLE EvilnessFactors
                (
                    Id INT PRIMARY KEY IDENTITY, 
                    Name VARCHAR(50)
                )
 
                CREATE TABLE Villains 
                (
                    Id INT PRIMARY KEY IDENTITY, 
                    Name VARCHAR(50), 
                    EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id)
                )
 
                CREATE TABLE MinionsVillains 
                (
                    MinionId INT FOREIGN KEY REFERENCES Minions(Id),
                    VillainId INT FOREIGN KEY REFERENCES Villains(Id),
                    CONSTRAINT PK_MinionsVillains 
                    PRIMARY KEY (MinionId, VillainId)
                )";


                SqlCommand createTablesCmd = new SqlCommand(queryText, sqlConnection);

                try
                {
                    createTablesCmd.ExecuteNonQuery();
                    Console.WriteLine("Tables created successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an error creating tables!");
                    Console.WriteLine(e.Message);
                }

                queryText = @"INSERT INTO Countries ([Name]) 
                        VALUES ('Bulgaria'), 
                               ('England'), 
                               ('Cyprus'), 
                               ('Germany'), 
                               ('Norway')
 
                INSERT INTO Towns ([Name], CountryCode) 
                        VALUES ('Plovdiv', 1),
                               ('Varna', 1),
                               ('Burgas', 1),
                               ('Sofia', 1),
                               ('London', 2),  
                               ('Southampton', 2),    
                               ('Bath', 2),  
                               ('Liverpool', 2),  
                               ('Berlin',  3),
                               ('Frankfurt', 3),   
                               ('Oslo', 4)
 
                INSERT INTO Minions (Name,Age, TownId) 
                        VALUES ('Bob', 42, 3),
                               ('Kevin', 1, 1),        
                               ('Bob ', 32, 6),
                               ('Simon', 45, 3),    
                               ('Cathleen', 11, 2),  
                               ('Carry ', 50, 10), 
                               ('Becky', 125, 5),
                               ('Mars', 21, 1),
                               ('Misho', 5, 10),
                               ('Zoe', 125, 5),
                               ('Json', 21, 1)
 
                INSERT INTO EvilnessFactors (Name) 
                        VALUES ('Super good'),  
                               ('Good'),      
                               ('Bad'),      
                               ('Evil'),
                               ('Super evil')
 
                INSERT INTO Villains (Name, EvilnessFactorId) 
                        VALUES ('Gru', 2),      
                               ('Victor', 1),       
                               ('Jilly', 3),
                               ('Miro', 4),
                               ('Rosen', 5),    
                               ('Dimityr', 1),   
                               ('Dobromir', 2)
 
                INSERT INTO MinionsVillains (MinionId, VillainId) 
                        VALUES (4,2),
                               (1, 1),
                               (5, 7),
                               (3, 5),
                               (2, 6),
                               (11, 5),
                               (8, 4),
                               (9, 7),
                               (7, 1),
                               (1, 3),
                               (7, 3),
                               (5, 3),
                               (4, 3),
                               (1, 2),
                               (2, 1),
                               (2, 7)";

                SqlCommand insertCmd = new SqlCommand(queryText, sqlConnection);
                try
                {
                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    Console.WriteLine("Data inserted successfully!");
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an error inserting data into tables!");
                    Console.WriteLine(e.Message);
                }
            }

        }
    }
}
