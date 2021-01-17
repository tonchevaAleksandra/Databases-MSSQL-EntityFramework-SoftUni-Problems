

namespace P03_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public byte ScoredGoals { get; set; }
        public byte Assists { get; set; }
        public byte MinutesPlayed { get; set; }

       
    }
}
