using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideServer = Riptide.Server;

namespace CopperMatchmaking.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class MatchmakerServer
    {
        internal static MatchmakerServer Instance = null!;

        internal readonly RiptideServer Server = null!;

        internal readonly List<Rank> Ranks = new List<Rank>();

        private readonly ServerQueueManager queueManager = null!;
        internal readonly ServerLobbyManager LobbyManager = null!;
        private readonly IServerHandler handler;

        /// <summary>
        /// Base Constructor with a pre-made ServerHandler
        /// </summary>
        /// <param name="lobbySize">Size of a lobby. Must be an even number</param>
        /// <param name="maxClients">Max amount of clients that can connect to the matchmaking server</param>
        public MatchmakerServer(byte lobbySize = 10, ushort maxClients = 65534) : this(new BasicServerHandler(), lobbySize, maxClients)
        {
        }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="handler">Server handler</param>
        /// <param name="lobbySize">Size of a lobby. Must be an even number</param>
        /// <param name="maxClients">Max amount of clients that can connect to the matchmaking server</param>
        public MatchmakerServer(IServerHandler handler, byte lobbySize = 10, ushort maxClients = 65534)
        {
            // values
            this.handler = handler;
            Instance = this;

            // checks
            if (lobbySize % 2 != 0)
                return;

            // logs
            CopperLogger.Initialize(CopperLogger.InternalLogInfo, CopperLogger.InternalLogWarning, CopperLogger.InternalLogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);

            // networking
            Server = new RiptideServer(new TcpServer());
            Server.Start(7777, maxClients, 0, false);
            

            // matchmaking 
            queueManager = new ServerQueueManager(lobbySize);
            LobbyManager = new ServerLobbyManager(this);

            // actions
            queueManager.PotentialLobbyFound += LobbyManager.PotentialLobbyFound;
            Server.ClientDisconnected += (sender, args) =>
            {
                Log.Info($"Client disconnected");
                queueManager.DisconnectClient(args.Client);
            };
            Server.MessageReceived += (sender, args) =>
            {
                Log.Info($"Received message of id {args.MessageId}.");
                switch (args.MessageId)
                {
                    case 1:
                        Log.Info($"Received {nameof(MessageIds.ClientJoined)} message.");
                        ServerMessageHandlers.ClientJoinedMessageHandler(args.FromConnection.Id, args.Message);
                        break;
                    case 3:
                        Log.Info($"Received {nameof(MessageIds.ClientHostLobbyId)} message.");
                        ServerMessageHandlers.ClientHostLobbyIdMessageHandler(args.FromConnection.Id, args.Message);
                        break;
                    default:
                        Log.Warning($"Received unknown message of id {args.MessageId}.");
                        break;
                }
            };
        }

        /// <summary>
        /// Method to run often to update the server
        /// </summary>
        public void Update()
        {
            // internal crap
            queueManager.CheckForLobbies();

            // networking
            Server.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetRanks">Ranks to register</param>
        public void RegisterRanks(params Rank[] targetRanks)
        {
            // bytes are used internally for rank ids so we dont want more than the max amount of a byte
            if (Ranks.Count + targetRanks.Length > byte.MaxValue - 1)
                return;

            Ranks.AddRange(targetRanks.ToList());
            Log.Info(
                $"Registering {targetRanks.Length} new ranks, bringing the total to {Ranks.Count}. | Ranks: {Ranks.Aggregate("", (current, rank) => current + $"{rank.DisplayName}[{rank.Id}], ")}");

            queueManager.RegisterRanks(Ranks);
        }

        internal void RegisterClient(ConnectedClient client)
        {
            Log.Info($"New Client Joined | Rank: {client.Rank.DisplayName} | ConnectionId: {client.ConnectionId}");

            if (!handler.VerifyPlayer(client))
            {
                Log.Info($"Couldn't verify client. Disconnecting");
                return;
            }

            queueManager.RegisterPlayer(client);
        }

        internal void SendMessage(Message message, ushort toClient, bool shouldRelease = true) => Server.Send(message, toClient, shouldRelease);

        internal ushort SendMessage(Message message, Connection toClient, bool shouldRelease = true) => Server.Send(message, toClient, shouldRelease);

        internal void SendMessageToAll(Message message, bool shouldRelease = true) => Server.SendToAll(message, shouldRelease);

        internal void SendMessageToAll(Message message, ushort exceptToClientId, bool shouldRelease = true) => Server.SendToAll(message, exceptToClientId, shouldRelease);
    }
}