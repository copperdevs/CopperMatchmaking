using CopperMatchmaking.Data;

namespace CopperMatchmaking.Server
{
    public class BasicServerHandler : IServerHandler
    {
        public bool VerifyPlayer(ConnectedClient client)
        {
            return true;
        }
    }
}