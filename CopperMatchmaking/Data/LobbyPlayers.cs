using System.Collections.Generic;

namespace CopperMatchmaking.Data
{
    public class LobbyPlayers
    {
        public ConnectedClient Host;
        public IEnumerable<ConnectedClient> TeamOne;
        public IEnumerable<ConnectedClient> TeamTwo;

        public LobbyPlayers(ConnectedClient host, IEnumerable<ConnectedClient> teamOne, IEnumerable<ConnectedClient> teamTwo)
        {
            Host = host;
            TeamOne = teamOne;
            TeamTwo = teamTwo;
        }
    }
}