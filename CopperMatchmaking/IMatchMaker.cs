using System.Collections.Generic;
using CopperMatchmaking.Data;

namespace CopperMatchmaking
{
    public interface IMatchMaker
    {
        public LobbyPlayers MatchFound(IEnumerable<ConnectedClient> clients);
    }
}