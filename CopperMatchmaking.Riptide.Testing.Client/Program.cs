
namespace CopperMatchmaking.Riptide.Testing.Client;

public static class Program
{
    public static void Main()
    {
        var client = new MatchmakerClient("127.0.0.1", new ClientHandler());

        while (client.ShouldUpdate)
        {
            client.Update();
        }
    }
}