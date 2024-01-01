using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideServer = Riptide.Server;

namespace CopperMatchmaking
{
    public class MatchmakerServer
    {
        private RiptideServer server;

        public MatchmakerServer(ushort maxClients = 65534)
        {
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);
            
            server = new RiptideServer(new TcpServer());
            server.Start(7777, maxClients);
        }

        public void Update()
        {
            server.Update();
        }
    }
}