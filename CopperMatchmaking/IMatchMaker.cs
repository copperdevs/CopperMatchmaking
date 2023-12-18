using System.Collections.Generic;

namespace CopperMatchmaking
{
    public interface IMatchMaker
    {
        public void MatchFound(IEnumerable<ConnectedClient> clients);
    }
}