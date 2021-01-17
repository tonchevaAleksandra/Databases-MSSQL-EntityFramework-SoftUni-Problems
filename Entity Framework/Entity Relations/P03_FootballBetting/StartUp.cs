using P03_FootballBetting.Data;
using System;
using System.Linq;

namespace P03_FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            FootballBettingContext context = new FootballBettingContext();

            var users = context
                .Users
                .Select(u => new
                {
                    u.Username,
                    u.Email,
                    Name = u.Name == null ? "(no name)" : u.Name,
                    u.Balance
                });

            foreach (var user in users)
            {
                Console.WriteLine($"User {user.Username} has email: {user.Email} and name: {user.Name} and balance - ${user.Balance}");
            }
        }
    }
}
