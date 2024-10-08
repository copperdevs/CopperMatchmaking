using System;
using System.Collections;
using System.Collections.Generic;

namespace CopperDevs.Matchmaking.Data
{
    /// <summary>
    /// Created lobby data
    /// </summary>
    public class MatchmakingLobby : IEnumerable<MatchmakingClient>
    {
        /// <summary>
        /// Id of the created lobby
        /// </summary>
        public uint LobbyId { get; private set; }
        
        /// <summary>
        /// All clients that are in the lobby
        /// </summary>
        public List<MatchmakingClient> LobbyClients { get; private set; }
        
        /// <summary>
        /// Time the lobby was created
        /// </summary>
        public DateTime LobbyCreationTime { get; private set; }
        
        /// <summary>
        /// Rank of the lobby
        /// </summary>
        public byte LobbyRank { get; private set; }


        /// <summary>
        /// Create a new lobby
        /// </summary>
        /// <param name="lobbyId">Lobby Id</param>
        /// <param name="lobbyClients">Lobby Clients</param>
        /// <param name="lobbyRank">Lobby Rank</param>
        internal MatchmakingLobby(uint lobbyId, List<MatchmakingClient> lobbyClients, byte lobbyRank)
        {
            LobbyId = lobbyId;
            LobbyClients = lobbyClients;
            LobbyRank = lobbyRank;

            LobbyCreationTime = DateTime.Now;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<MatchmakingClient> GetEnumerator()
        {
            return LobbyClients.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Get the index of a client in the lobby list
        /// </summary>
        /// <param name="client">Client to find the index of</param>
        /// <returns>Index</returns>
        public int IndexOf(MatchmakingClient client)
        {
            return LobbyClients.IndexOf(client);
        }

        /// <summary>
        /// Get a client from its index
        /// </summary>
        /// <param name="i">Index of the client you want</param>
        public MatchmakingClient this[int i] => LobbyClients[i];
    }
}