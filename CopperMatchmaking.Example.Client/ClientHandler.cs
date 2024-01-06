using CopperMatchmaking.Client;
using CopperMatchmaking.Info;

namespace CopperMatchmaking.Example.Client;

public class ClientHandler : IClientHandler
{
    public ulong ClientRequestedToHost()
    {
        var serverJoinCode = (ulong)Random.Shared.NextInt64(1000000000000);
        Log.Info($"join code: {serverJoinCode}");
        return serverJoinCode;
    }

    public void JoinServer(ulong serverJoinCode)
    {
        Log.Info($"join code: {serverJoinCode}");
    }
}