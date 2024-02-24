using System.Collections.Generic;
using CopperMatchmaking.Data;

namespace CopperMatchmaking.Server
{
    /// <summary>
    /// Base server handler
    /// </summary>
    public class ServerHandler
    {
        /// <summary>
        /// Server side verification
        /// </summary>
        /// <param name="client">Target client to verify</param>
        /// <returns>True if client is verified and allowed to connect</returns>
        public virtual bool VerifyPlayer(ConnectedClient client)
        {
            return true;
        }

        /// <summary>
        /// Ran for when a lobby is created on the server
        /// </summary>
        /// <param name="lobbyClients">Clients in the lobby</param>
        public virtual void LobbyCreated(List<ConnectedClient> lobbyClients)
        {
            
        }
    }
}