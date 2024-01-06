using System;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideClient = Riptide.Client;

namespace CopperMatchmaking.Client
{
    public class MatchmakerClient
    {
        internal static MatchmakerClient Instance = null!;
        
        public bool ShouldUpdate { get; private set; }
        
        internal readonly RiptideClient Client;
        internal readonly IClientHandler Handler;

        private readonly byte rankId;
        private readonly ulong playerId;

        public MatchmakerClient(string ip, IClientHandler clientHandler, Enum rankId, ulong playerId)
        {
            // init logs
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);
            
            // values/handlers
            this.rankId = Convert.ToByte(rankId);
            this.playerId = playerId;
            Handler = clientHandler;
            Instance = this;

            // start riptide crap
            Client = new RiptideClient(new TcpClient());
            Client.Connect($"{ip}:7777");
            ShouldUpdate = true;

            Client.Disconnected += (sender, args) => ShouldUpdate = false; 

            Client.Send(GetJoinMessage());
        }

        public void Update()
        {
            if(ShouldUpdate)
                Client.Update();
        }

        private Message GetJoinMessage()
        {
            var result = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoined);

            result.Add(playerId); // ushort
            result.Add(rankId); // byte
            
            return result;
        }
    }
}