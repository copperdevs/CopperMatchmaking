using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Transports.Udp;
using Riptide.Utils;
using RiptideClient = Riptide.Client;

namespace CopperMatchmaking
{
    public class MatchmakerClient
    {
        public bool ShouldUpdate { get; private set; }
        
        private RiptideClient client;
        private IClientHandler clientHandler;

        public MatchmakerClient(string ip, IClientHandler clientHandler)
        {
            // init logs
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);
            
            // interface
            this.clientHandler = clientHandler;
            
            // start riptide crap
            client = new RiptideClient(new TcpClient());
            client.Connect($"{ip}:7777");
            ShouldUpdate = true;
        }

        public void Update()
        {
            if(ShouldUpdate)
                client.Update();
        }
    }
}