using CopperDevs.Logger;
using CopperDevs.Matchmaking.Client;
using Riptide;

namespace CopperDevs.Matchmaking.Example.Client;

public class SimpleClientHandler : BaseClientHandler
{
    public override void ClientRequestedToHost()
    {
        var serverJoinCode = ((ulong)Random.Shared.NextInt64(1000000000000)).ToString();
        Log.Info($"join code: {serverJoinCode}");
        
        Client.SendLobbyCode(serverJoinCode);
    }

    public override void JoinServer(string serverJoinCode)
    {
        Log.Info($"join code: {serverJoinCode}");
    }

    public override void Disconnected(DisconnectReason reason)
    {
        Log.Info($"Disconnected. | Reason: {reason}");
    }
}