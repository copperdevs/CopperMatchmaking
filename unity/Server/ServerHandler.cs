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
        /// <param name="lobby">Created lobby data</param>
        public virtual void LobbyCreated(CreatedLobby lobby)
        {
            
        }

        /// <summary>
        /// Functions for choosing who in a lobby should be the host
        /// </summary>
        /// <param name="lobbyClients">All the clients in the lobby</param>
        /// <returns>Index of the list corresponding to who should host</returns>
        public virtual int ChooseLobbyHost(List<ConnectedClient> lobbyClients)
        {
            return 0;
        }
    }
}