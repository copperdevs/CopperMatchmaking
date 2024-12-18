using System;
using System.Collections.Generic;
using System.Linq;
using CopperDevs.Logger;
using CopperDevs.Matchmaking.Data;
using CopperDevs.Matchmaking.Server.Internal;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideServer = Riptide.Server;

namespace CopperDevs.Matchmaking.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class MatchmakerServer
    {
        internal readonly RiptideServer Server;

        internal static readonly List<Rank> Ranks = new List<Rank>();

        internal readonly ServerQueueManager QueueManager;
        internal readonly ServerLobbyManager LobbyManager;
        internal readonly ServerHandler Handler;
        private readonly ServerClientCounter clientCounter;

        private readonly ServerMessageHandlers messageHandlers;

        /// <summary>
        /// Called when a potential lobby is found
        /// </summary>
        public Action<MatchmakingLobby> LobbyCreated = null!;

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
            messageHandlers = new ServerMessageHandlers(this);

            // checks
            if (lobbySize % 2 != 0)
                throw new Exception($"Lobby size is not divisible by 2.");

            // logs
            RiptideLogger.Initialize(Log.Debug, Log.Info, Log.Warning, Log.Error, false);

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
            Server.MessageReceived += messageHandlers.ServerReceivedMessageHandler;

            // created last so everything else is setup
            clientCounter = new ServerClientCounter(this);
        }

        ~MatchmakerServer()
        {
            QueueManager.PotentialLobbyFound -= LobbyManager.PotentialLobbyFound;
            Server.ClientDisconnected -= QueueManager.ClientDisconnected;
            Server.ClientDisconnected -= LobbyManager.ClientDisconnected;
            Server.MessageReceived -= messageHandlers.ServerReceivedMessageHandler;
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

            // player count update
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
            Log.Info($"Registering {targetRanks.Length} new ranks, bringing the total to {Ranks.Count}. | Ranks: {Ranks.Aggregate("", (current, rank) => current + $"{rank.DisplayName}[{rank.Id}], ")}");

            QueueManager.RegisterRanks(Ranks);
        }

        internal void RegisterClient(MatchmakingClient client)
        {
            Log.Debug($"New Client Joined | Rank: {client.Rank.DisplayName} | ConnectionId: {client.ConnectionId}");

            if (!Handler.VerifyPlayer(client))
            {
                Log.Warning($"Couldn't verify client. Disconnecting");
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
        public List<MatchmakingLobby> GetCurrentLobbies()
        {
            return LobbyManager.Lobbies.Values.ToList();
        }

        /// <summary>
        /// Get all rank queues with their players
        /// </summary>
        /// <returns>Dictionary of all clients currently in queue</returns>
        /// <remarks>The key is the rank</remarks>
        public Dictionary<byte, List<MatchmakingClient>> GetRankQueues()
        {
            return QueueManager.RankQueues;
        }

        /// <summary>
        /// Tries to get a client connection based off of its user ID
        /// </summary>
        /// <param name="id">The ID of the use you want to get</param>
        /// <returns>The connection if its found, otherwise returns null</returns>
        public Connection? TryGetClientConnection(ushort id)
        {
            return Server.TryGetClient(id, out var connection) ? connection : null;
        }
    }
}