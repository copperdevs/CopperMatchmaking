using System.Collections.Generic;
using CopperDevs.Matchmaking.Data;

namespace CopperDevs.Matchmaking.Server
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
        public virtual bool VerifyPlayer(MatchmakingClient client)
        {
            return true;
        }

        /// <summary>
        /// Ran for when a lobby is created on the server
        /// </summary>
        /// <param name="lobby">Created lobby data</param>
        public virtual void LobbyCreated(MatchmakingLobby lobby)
        {
            
        }

        /// <summary>
        /// Functions for choosing who in a lobby should be the host
        /// </summary>
        /// <param name="lobbyClients">All the clients in the lobby</param>
        /// <returns>Index of the list corresponding to who should host</returns>
        public virtual int ChooseLobbyHost(List<MatchmakingClient> lobbyClients)
        {
            return 0;
        }
        
        /// <summary>
        /// Function ran when a lobby join code is received
        /// </summary>
        /// <param name="lobby">Created lobby</param>
        /// <param name="lobbyJoinCode">Created lobby join code</param>
        public virtual void LobbyJoinCodeReceived(MatchmakingLobby lobby, string lobbyJoinCode)
        {
            
        }

        /// <summary>
        /// Ran when the number of players in queue is updated
        /// </summary>
        /// <param name="lobbyPlayerCount">Current count of players in a lobby waiting for a host to send a join code</param>
        /// <param name="queuePlayerCount">Current count of players in queue waiting for a lobby</param>
        public virtual void PlayerQueueCountUpdated(int lobbyPlayerCount, int queuePlayerCount)
        {
        }
    }
}