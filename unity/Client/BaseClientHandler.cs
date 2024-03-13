using Riptide;

namespace CopperMatchmaking.Client
{
    /// <summary>
    /// Interface for clients to handle hosting and joining lobbies.
    /// </summary>
    public abstract class BaseClientHandler
    {
        /// <summary>
        /// The target client for the handler
        /// </summary>
        public MatchmakerClient Client { get; internal set; } = null!;
        
        /// <summary>
        /// Method for getting the join code of a lobby the client
        /// </summary>
        public abstract void ClientRequestedToHost();
        
        /// <summary>
        /// Method for joining a server from a join code
        /// </summary>
        /// <param name="serverJoinCode">Lobby join code</param>
        public abstract void JoinServer(string serverJoinCode);

        /// <summary>
        /// Method ran when disconnected from the server
        /// </summary>
        public abstract  void Disconnected(DisconnectReason reason);
    }
}