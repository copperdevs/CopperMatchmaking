using CopperMatchmaking.Testing.Server;

namespace CopperMatchmaking.Testing.Client;

public static class Program
{
    public static void Main()
    {
        var client = new MatchmakerClient(Ranks.Bronze);
        client.Connect("127.0.0.1", 7777);

        while (true)
        {
            client.Update();
        }
    }
}