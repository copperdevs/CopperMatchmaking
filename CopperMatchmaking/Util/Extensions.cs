using System.Linq;
using Riptide;
using RiptideServer = Riptide.Server;

namespace CopperMatchmaking.Util
{
    public static class Extensions
    {
        public static Connection? GetConnection(this RiptideServer server, ushort id)
        {
            return server.Clients.FirstOrDefault(connection => !(connection is null) && connection.Id == id);
        }
    }
}