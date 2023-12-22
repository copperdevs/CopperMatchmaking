using CopperMatchmaking.Data;

namespace CopperMatchmaking.Testing.Server;

public class TestMatchmaker : IMatchMaker
{
    public LobbyPlayers MatchFound(IEnumerable<ConnectedClient> clients)
    {
        var connectedClients = clients.ToList();
        
        var count = connectedClients.Count;
        var teamSize = count / 2;

        var teamOne = connectedClients.Take(teamSize);
        var teamTwo = connectedClients.Skip(count);

        return new LobbyPlayers(connectedClients[0], teamOne, teamTwo);
    }
}