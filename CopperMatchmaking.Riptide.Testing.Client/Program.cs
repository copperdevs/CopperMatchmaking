﻿
using CopperMatchmaking.Riptide.Testing.Server;

namespace CopperMatchmaking.Riptide.Testing.Client;

public static class Program
{
    public static void Main()
    {
        var client = new MatchmakerClient("127.0.0.1", new ClientHandler(), RankIds.Bronze, (ulong)Random.Shared.NextInt64(1000000000000));

        while (client.ShouldUpdate)
        {
            client.Update();
        }
    }
}