using Riptide;
using Riptide.Transports.Udp;
using Riptide.Utils;
using RiptideClient = Riptide.Client;

namespace CopperMatchmaking
{
    public class MatchmakerClient
    {
        private RiptideClient client;

        public MatchmakerClient(string ip)
        {
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);
            
            client = new RiptideClient(new UdpClient());
            client.Connect($"{ip}:7777");
        }

        public void Update()
        {
            client.Update();
        }
    }
}