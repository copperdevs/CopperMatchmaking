using CopperMatchmaking.Client;
using CopperMatchmaking.Info;
using Riptide;

namespace CopperMatchmaking.Example.Client;

public class ClientHandler : IClientHandler
{
    public string ClientRequestedToHost()
    {
        var serverJoinCode = (ulong)Random.Shared.NextInt64(1000000000000);
        Log.Info($"join code: {serverJoinCode}");
        return serverJoinCode.ToString();
    }

    public void JoinServer(string serverJoinCode)
    {
        Log.Info($"join code: {serverJoinCode}");
    }

    public void Disconnected(DisconnectReason reason)
    {
        Log.Info($"Disconnected. | Reason: {reason}");
    }
}