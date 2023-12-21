using CopperMatchmaking.Info;

namespace CopperMatchmaking.Testing.Client;

public class ExampleClientHandler : IClientHandler
{
    public ulong ClientRequestedHost()
    {
        var joinCode = Matchmaker.Random.Next();
        Log.Info($"starting server - code: {joinCode}");
        return (ulong)joinCode;
    }

    public void JoinServer(ulong serverJoinCode)
    {
        Log.Info($"joining server - {serverJoinCode}");
    }
}