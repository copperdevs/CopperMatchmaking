using CopperMatchmaking.Testing.Server;

namespace CopperMatchmaking.Testing.Client;

public static class Program
{
    public static void Main()
    {
        var client = new MatchmakerClient(Ranks.Bronze, new ExampleClientHandler());
        client.Connect("127.0.0.1", 7777);

        // fixed update for example
        while (true)
        {
            client.Update();
        }
    }
}