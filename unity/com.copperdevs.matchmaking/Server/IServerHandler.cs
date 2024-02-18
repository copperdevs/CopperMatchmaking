using CopperMatchmaking.Data;

namespace CopperMatchmaking.Server
{
    /// <summary>
    /// Server handler interface handler
    /// </summary>
    public interface IServerHandler
    {
        /// <summary>
        /// Server side verification
        /// </summary>
        /// <param name="client">Target client to verify</param>
        /// <returns>True if client is verified and allowed to connect</returns>
        public bool VerifyPlayer(ConnectedClient client);
    }
}