using Riptide;
using RiptideServer = Riptide.Server;

namespace CopperDevs.Matchmaking.Util
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Attempt to get a riptide connection from a server with its connection id
        /// </summary>
        /// <param name="server">Target server to get the connection from</param>
        /// <param name="id">Connection id</param>
        /// <returns>Riptide connection</returns>
        public static Connection? GetConnection(this RiptideServer server, ushort id)
        {
            return server.TryGetClient(id, out var client) ? client : null;
        }
    }
}