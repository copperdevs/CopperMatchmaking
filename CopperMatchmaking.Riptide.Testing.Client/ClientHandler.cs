using System.Runtime.CompilerServices;

namespace CopperMatchmaking.Riptide.Testing.Client;

public class ClientHandler : IClientHandler
{
    public ulong ClientRequestedToHost()
    {
        const ulong serverJoinCode = 000000000000000000;
        Log.Info($"join code: {serverJoinCode}");
        return serverJoinCode;
    }

    public void JoinServer(ulong serverJoinCode)
    {
        Log.Info($"join code: {serverJoinCode}");
    }
}