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
        public ulong ClientRequestedToHost();
        /// <summary>
        /// Method for joining a server from a join code
        /// </summary>
        /// <param name="serverJoinCode">Lobby join code</param>
        public void JoinServer(ulong serverJoinCode);
    }
}