using CopperMatchmaking.Data;
using CopperMatchmaking.Info;

namespace CopperMatchmaking.Testing.Server;

// 7781
public static class Program
{
    public static void Main()
    {
        Matchmaker.Initialize(new TestMatchmaker(), 4);
        Matchmaker.RegisterRanks(
            new Rank("Bronze", Ranks.Bronze),
            new Rank("Silver", Ranks.Silver),
            new Rank("Gold", Ranks.Gold),
            new Rank("Platinum", Ranks.Platinum),
            new Rank("Diamond", Ranks.Diamond),
            new Rank("Master", Ranks.Master),
            new Rank("Chaos", Ranks.Chaos));

        Matchmaker.Start();
        
        while (true)
        {
            Matchmaker.Update();
        }
    }
}