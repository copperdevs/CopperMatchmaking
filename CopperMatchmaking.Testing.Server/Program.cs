namespace CopperMatchmaking.Testing.Server;

// 7781
public static class Program
{
    public static void Main()
    {
        Matchmaker.Initialize();
        Matchmaker.RegisterRanks(
            new Rank("Bronze", Ranks.Bronze),
            new Rank("Silver", Ranks.Silver),
            new Rank("Gold", Ranks.Gold),
            new Rank("Platinum", Ranks.Platinum),
            new Rank("Diamond", Ranks.Diamond),
            new Rank("Master", Ranks.Master),
            new Rank("Chaos", Ranks.Chaos));

        foreach (var rank in Matchmaker.Ranks)
        {
            Log.Info($"{rank.Key} {rank.Value.DisplayName}");
        }

        while (true)
        {
            Matchmaker.Update();
        }
    }
}