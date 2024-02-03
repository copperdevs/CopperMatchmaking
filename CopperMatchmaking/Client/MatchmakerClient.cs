using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideClient = Riptide.Client;

namespace CopperMatchmaking.Client
{
    /// <summary>
    /// Matchmaker client for connecting to the matchmaker with
    /// </summary>
    public class MatchmakerClient
    {
        internal static MatchmakerClient Instance = null!;

        /// <summary>
        /// Enabled when <see cref="Update"/> needs to be ran to update the client.
        /// </summary>
        public bool ShouldUpdate { get; private set; }

        internal readonly RiptideClient Client;
        internal readonly IClientHandler Handler;

        private readonly byte rankId;
        private readonly ulong playerId;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="ip">Target ip of the matchmaker server</param>
        /// <param name="clientHandler">Handler for the client</param>
        /// <param name="rankId">Id of the clients rank</param>
        /// <param name="playerId">Player id (SteamId for example)</param>
        public MatchmakerClient(string ip, IClientHandler clientHandler, byte rankId, ulong playerId)
        {
            // init logs
            CopperLogger.Initialize(CopperLogger.InternalLogInfo, CopperLogger.InternalLogWarning, CopperLogger.InternalLogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);

            // values/handlers
            this.rankId = rankId;
            this.playerId = playerId;
            Handler = clientHandler;
            Instance = this;

            // start riptide crap
            Client = new RiptideClient(new TcpClient());
            Client.Connect($"{ip}:7777", 5, 0, null, false);
            ShouldUpdate = true;
            Client.Connection.CanQualityDisconnect = false;

            Client.Disconnected += (sender, args) => ShouldUpdate = false;

            Client.Connected += (sender, args) =>
            {
                var joinMessage = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoined);

                joinMessage.Add(playerId); // ushort
                joinMessage.Add(rankId); // byte

                Log.Info($"Creating client join message. | PlayerId {playerId} | RankId {rankId}");
                Client.Send(joinMessage);
            };

            Client.MessageReceived += (sender, args) =>
            {
                Log.Info($"Received message of id {args.MessageId}.");
                switch (args.MessageId)
                {
                    case 2:
                        Log.Info($"Received {nameof(MessageIds.ServerRequestedClientToHost)} message.");
                        ClientMessageHandlers.ServerRequestedClientToHostMessageHandler(args.Message);
                        break;
                    case 4:
                        Log.Info($"Received {nameof(MessageIds.ClientJoinCreatedLobby)} message.");
                        ClientMessageHandlers.ClientJoinCreatedLobbyMessageHandler(args.Message);
                        break;
                    default:
                        Log.Warning($"Received unknown message of id {args.MessageId}.");
                        break;
                }
            };

            Client.Disconnected += (sender, args) =>
            {
                Log.Info($"Client disconnected | Reason: {args.Reason}");
                clientHandler.Disconnected(args.Reason);
            };
        }

        /// <summary>
        /// Method to run often to update the client
        /// </summary>
        public void Update()
        {
            if (ShouldUpdate)
                Client.Update();
        }
    }
}