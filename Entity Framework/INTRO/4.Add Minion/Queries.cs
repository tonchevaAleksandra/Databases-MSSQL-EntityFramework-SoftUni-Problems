
namespace _4.Add_Minion
{
    public static class Queries
    {
        public const string FindTownID = "SELECT Id FROM Towns WHERE Name = @townName";

        public const string FindMinionID = "SELECT Id FROM Minions WHERE Name = @Name";

        public const string FindVillainID = "SELECT Id FROM Villains WHERE Name = @Name";

        public const string InsertTown = "INSERT INTO Towns (Name) VALUES (@townName)";

        public const string InsertMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

        public const string InsertVillain = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

        public const string InsertMinionToVillain = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId,@villainId)";
    }
}
