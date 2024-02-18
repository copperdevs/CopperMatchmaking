using CopperMatchmaking.Data;

namespace CopperMatchmaking.Server
{
    /// <summary>
    /// Base built in server handler
    /// </summary>
    public class BasicServerHandler : IServerHandler
    {
        /// <summary>
        /// Verify if a client should be allowed to connect
        /// </summary>
        /// <param name="client">Target client to connect</param>
        /// <returns>True if client is allowed to connect</returns>
        public bool VerifyPlayer(ConnectedClient client)
        {
            return true;
        }
    }
}