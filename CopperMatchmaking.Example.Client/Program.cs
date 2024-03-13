using CopperMatchmaking.Client;
using CopperMatchmaking.Example.Server;

namespace CopperMatchmaking.Example.Client;

public static class Program
{
    public static void Main()
    {
        var client = new MatchmakerClient("127.0.0.1", new SimpleClientHandler(), (byte)RankIds.Bronze, GetPlayerId());
        
        while (client.ShouldUpdate)
        {
            client.Update();
        }
    }

    private static ulong GetPlayerId()
    {
        var playerIdEnvVar = Environment.GetEnvironmentVariable("playerId");

        if (playerIdEnvVar != null)
        {
            if (ulong.TryParse(playerIdEnvVar, out var result))
                return result;
        }

        return (ulong)Random.Shared.NextInt64(1000000000000);
    }
}