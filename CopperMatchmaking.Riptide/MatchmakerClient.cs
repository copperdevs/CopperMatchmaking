using System;
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

        private readonly byte rankId;
        private readonly ulong playerId;

        public MatchmakerClient(string ip, IClientHandler clientHandler, Enum rankId, ulong playerId)
        {
            // init logs
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);
            
            // values/handlers
            this.clientHandler = clientHandler;
            this.rankId = Convert.ToByte(rankId);
            this.playerId = playerId;
            
            // start riptide crap
            client = new RiptideClient(new TcpClient());
            client.Connect($"{ip}:7777");
            ShouldUpdate = true;

            client.Send(GetJoinMessage());
        }

        public void Update()
        {
            if(ShouldUpdate)
                client.Update();
        }

        private Message GetJoinMessage()
        {
            var result = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoined);

            result.Add(playerId);
            result.Add(rankId);
            
            return result;
        }
    }
}