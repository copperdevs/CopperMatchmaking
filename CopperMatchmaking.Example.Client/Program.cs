using CopperMatchmaking.Client;
using CopperMatchmaking.Example.Server;

namespace CopperMatchmaking.Example.Client;

public static class Program
{
    public static void Main()
    {
        var client = new MatchmakerClient("127.0.0.1", new ClientHandler(), (byte)RankIds.Bronze, (ulong)76561198133981499);

        while (client.ShouldUpdate)
        {
            client.Update();
        }
    }
}
