using CopperMatchmaking.Data;
using CopperMatchmaking.Server;

namespace CopperMatchmaking.Example.Server;

public static class Program
{
    public static void Main()
    {
        var server = new MatchmakerServer(4);
        server.RegisterRanks(
            new Rank("Unranked", RankIds.Unranked), // 0
            new Rank("Bronze", RankIds.Bronze), // 1
            new Rank("Silver", RankIds.Silver), // 2
            new Rank("Gold", RankIds.Gold), // 3
            new Rank("Platinum", RankIds.Platinum), // 4
            new Rank("Diamond", RankIds.Diamond), // 5
            new Rank("Master", RankIds.Master), // 6
            new Rank("Chaos", RankIds.Chaos)); // 7
        
        while (true)
        {
            server.Update();
        }
    }
}