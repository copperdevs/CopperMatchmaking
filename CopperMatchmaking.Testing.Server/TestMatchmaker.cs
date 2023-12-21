using CopperMatchmaking.Data;

namespace CopperMatchmaking.Testing.Server;

public class TestMatchmaker : IMatchMaker
{
    public LobbyPlayers MatchFound(IEnumerable<ConnectedClient> clients)
    {
        return new LobbyPlayers(new ConnectedClient(0, 0), new List<ConnectedClient>(), new List<ConnectedClient>());
    }
}