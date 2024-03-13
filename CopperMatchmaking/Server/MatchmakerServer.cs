using System;
using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using CopperMatchmaking.Util;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideServer = Riptide.Server;

namespace CopperMatchmaking.Server
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MatchmakerServer
    {
        internal readonly RiptideServer Server = null!;

        internal static readonly List<Rank> Ranks = new List<Rank>();

        internal readonly ServerQueueManager QueueManager = null!;
        internal readonly ServerLobbyManager LobbyManager = null!;
        internal readonly ServerHandler Handler;
        private readonly ServerClientCounter clientCounter;

        internal readonly ServerMessageHandlers MessageHandlers;

        /// <summary>
        /// Called when a potential lobby is found
        /// </summary>
        public Action<CreatedLobby> LobbyCreated = null!;

        /// <summary>
        /// Base Constructor with a pre-made <see cref="ServerHandler"/>
        /// </summary>
        /// <param name="lobbySize">Size of a lobby. Must be an even number</param>
        /// <param name="maxClients">Max amount of clients that can connect to the matchmaking server</param>
        public MatchmakerServer(byte lobbySize = 10, ushort maxClients = 65534) : this(new ServerHandler(), lobbySize, maxClients)
        {
        }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="handler"><see cref="ServerHandler"/></param>
        /// <param name="lobbySize">Size of a lobby. Must be an even number</param>
        /// <param name="maxClients">Max amount of clients that can connect to the matchmaking server</param>
        public MatchmakerServer(ServerHandler handler, byte lobbySize = 10, ushort maxClients = 65534)
        {
            // values
            Handler = handler;
            MessageHandlers = new ServerMessageHandlers(this);

            // checks
            if (lobbySize % 2 != 0)
            {
                Log.Error($"Lobby size is not divisible by 2.");
                return;
            }

            // logs
            CopperLogger.Initialize(CopperLogger.InternalLogInfo, CopperLogger.InternalLogWarning, CopperLogger.InternalLogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);

            // networking
            Server = new RiptideServer(new TcpServer());
            Server.Start(7777, maxClients, 0, false);

            // matchmaking 
            QueueManager = new ServerQueueManager(lobbySize);
            LobbyManager = new ServerLobbyManager(this);

            // actions
            QueueManager.PotentialLobbyFound += LobbyManager.PotentialLobbyFound;
            Server.ClientDisconnected += QueueManager.ClientDisconnected;
            Server.ClientDisconnected += LobbyManager.ClientDisconnected;
            Server.MessageReceived += MessageHandlers.ServerReceivedMessageHandler;
            
            // created last so everything else is setup
            clientCounter = new ServerClientCounter(this);
        }

        ~MatchmakerServer()
        {
            QueueManager.PotentialLobbyFound -= LobbyManager.PotentialLobbyFound;
            Server.ClientDisconnected -= QueueManager.ClientDisconnected;
            Server.ClientDisconnected -= LobbyManager.ClientDisconnected;
            Server.MessageReceived -= MessageHandlers.ServerReceivedMessageHandler;
        }

        /// <summary>
        /// Method to run often to update the server
        /// </summary>
        public void Update()
        {
            // them pesky players
            QueueManager.DisconnectedPlayerCheck();
            LobbyManager.DisconnectedPlayerCheck();
            
            // maybe not all of them are pesky!
            // lets make a lobby
            QueueManager.CheckForLobbies();

            // components and crap ig
            UpdateComponents();
            clientCounter.PlayerCountUpdateCheck();

            // networking!
            Server.Update();
        }

        /// <summary>
        /// Register new ranks
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

            QueueManager.RegisterRanks(Ranks);
        }

        internal void RegisterClient(ConnectedClient client)
        {
            Log.Info($"New Client Joined | Rank: {client.Rank.DisplayName} | ConnectionId: {client.ConnectionId}");

            if (!Handler.VerifyPlayer(client))
            {
                Log.Info($"Couldn't verify client. Disconnecting");
                return;
            }

            QueueManager.RegisterPlayer(client);
        }

        internal void SendMessage(Message message, ushort toClient, bool shouldRelease = true) => Server.Send(message, toClient, shouldRelease);

        internal ushort SendMessage(Message message, Connection toClient, bool shouldRelease = true) => Server.Send(message, toClient, shouldRelease);

        internal void SendMessageToAll(Message message, bool shouldRelease = true) => Server.SendToAll(message, shouldRelease);

        internal void SendMessageToAll(Message message, ushort exceptToClientId, bool shouldRelease = true) => Server.SendToAll(message, exceptToClientId, shouldRelease);

        /// <summary>
        /// Get all currently registered ranks
        /// </summary>
        /// <returns>List of all currently registered ranks</returns>
        public static List<Rank> GetAllRanks()
        {
            return Ranks;
        }

        /// <summary>
        /// Get all current lobbies waiting for a host response
        /// </summary>
        /// <returns>List of all current lobbies awaiting a host response</returns>
        public List<CreatedLobby> GetCurrentLobbies()
        {
            return LobbyManager.Lobbies.Values.ToList();
        }

        /// <summary>
        /// Get all rank queues with their players
        /// </summary>
        /// <returns>Dictionary of all clients currently in queue</returns>
        /// <remarks>The key is the rank</remarks>
        public Dictionary<byte, List<ConnectedClient>> GetRankQueues()
        {
            return QueueManager.RankQueues;
        }
    }
}