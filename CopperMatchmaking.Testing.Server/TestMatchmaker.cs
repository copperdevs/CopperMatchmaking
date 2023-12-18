namespace CopperMatchmaking.Testing.Server;

public class TestMatchmaker : IMatchMaker
{
    public void MatchFound(IEnumerable<ConnectedClient> clients)
    {
        var message = new Message((byte)MessageIds.ServerMatchFound, 0);
        foreach (var client in clients)
        {
            Matchmaker.Server.Send(client.Id, message);
        }
    }
}