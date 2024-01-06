using System;
using System.Threading.Tasks;
using CopperMatchmaking.Data;
using CopperMatchmaking.Utility;
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

        public MatchmakerClient(string ip, IClientHandler clientHandler, byte rankId, ulong playerId)
        {
            // init logs
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);

            // values/handlers
            this.rankId = rankId;
            this.playerId = playerId;
            Handler = clientHandler;
            Instance = this;

            // start riptide crap
            Client = new RiptideClient(new TcpClient());
            Client.Connect($"{ip}:7777");
            ShouldUpdate = true;

            Client.Disconnected += (sender, args) => ShouldUpdate = false;

            Client.Connected += (sender, args) =>
            {
                
                var joinMessage = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoined);

                joinMessage.Add(playerId); // ushort
                joinMessage.Add(rankId); // byte
            
                Log.Info($"Creating client join message. | PlayerId {playerId} | RankId {rankId}");
                Client.Send(joinMessage);
            };
        }

        public void Update()
        {
            if (ShouldUpdate)
                Client.Update();
        }
    }
}