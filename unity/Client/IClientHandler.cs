using Riptide;

namespace CopperMatchmaking.Client
{
    /// <summary>
    /// Interface for clients to handle hosting and joining lobbies.
    /// </summary>
    public interface IClientHandler
    {
        /// <summary>
        /// Method for getting the join code of a lobby the client
        /// </summary>
        /// <returns>Lobby join code</returns>
        public string ClientRequestedToHost();
        
        /// <summary>
        /// Method for joining a server from a join code
        /// </summary>
        /// <param name="serverJoinCode">Lobby join code</param>
        public void JoinServer(string serverJoinCode);

        /// <summary>
        /// Method ran when disconnected from the server
        /// </summary>
        public void Disconnected(DisconnectReason reason);
    }
}