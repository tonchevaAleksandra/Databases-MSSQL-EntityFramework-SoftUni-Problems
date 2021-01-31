using System;
using RealEstates.Data;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new RealEstateDbContext();
            db.Database.EnsureCreated();


        }
    }
}
