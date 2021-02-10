using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.WireProtocol;

namespace MongoDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            var database = client.GetDatabase("Softuni");
            var collection = database.GetCollection<BsonDocument>("Students");

            //SeedStudents(collection);

            //PrintAllStudents(collection);


        }

        private static void PrintAllStudents(IMongoCollection<BsonDocument> collection)
        {
            var allStudents = collection.Find<BsonDocument>(new BsonDocument()).ToList();

            foreach (var student in allStudents)
            {
                Console.WriteLine(student);
            }
        }

        private static void SeedStudents(IMongoCollection<BsonDocument> collection)
        {
            for (int i = 0; i < 10; i++)
            {
                var student = new BsonDocument
                {
                    {"name", "Pesho" + i}
                };
                collection.InsertOne(student);
            }
        }
    }

    class Student
    {

    }
}
