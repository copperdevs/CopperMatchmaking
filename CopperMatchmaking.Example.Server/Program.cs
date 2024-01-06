using CopperMatchmaking.Data;
using CopperMatchmaking.Server;

namespace CopperMatchmaking.Example.Server;

public static class Program
{
    public static void Main()
    {
        var server = new MatchmakerServer(4);
        server.RegisterRanks(
            new Rank("Unranked", RankIds.Unranked),
            new Rank("Bronze", RankIds.Bronze),
            new Rank("Silver", RankIds.Silver),
            new Rank("Gold", RankIds.Gold),
            new Rank("Platinum", RankIds.Platinum),
            new Rank("Diamond", RankIds.Diamond),
            new Rank("Master", RankIds.Master),
            new Rank("Chaos", RankIds.Chaos));

        while (true)
        {
            server.Update();
        }
    }
}