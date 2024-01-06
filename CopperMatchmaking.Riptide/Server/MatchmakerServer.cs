using System;
using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Utility;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideServer = Riptide.Server;

namespace CopperMatchmaking.Server
{
    public class MatchmakerServer
    {
        internal static MatchmakerServer Instance = null!;

        internal readonly RiptideServer server = null!;

        internal readonly List<Rank> ranks = new List<Rank>();

        private readonly List<ConnectedClient> clients = new List<ConnectedClient>();

        private readonly ServerQueueManager queueManager = null!;
        internal readonly ServerLobbyManager LobbyManager = null!;
        private readonly IServerHandler handler;

        public MatchmakerServer(byte lobbySize = 10, ushort maxClients = 65534) : this(new BasicServerHandler(), lobbySize, maxClients)
        {
        }

        public MatchmakerServer(IServerHandler handler, byte lobbySize, ushort maxClients = 65534)
        {
            // values
            this.handler = handler;
            Instance = this;

            // checks
            if (lobbySize % 2 != 0)
                return;

            // logs
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);

            // networking
            server = new RiptideServer(new TcpServer());
            server.Start(7777, maxClients);

            // matchmaking 
            queueManager = new ServerQueueManager(lobbySize);
            LobbyManager = new ServerLobbyManager(handler, this);

            // actions
            queueManager.PotentialLobbyFound += LobbyManager.PotentialLobbyFound;
        }

        public void Update()
        {
            // internal crap
            queueManager.CheckForLobbies();

            // networking
            server.Update();
        }

        public void RegisterRanks(params Rank[] targetRanks)
        {
            // bytes are used internally for rank ids so we dont want more than the max amount of a byte
            if (ranks.Count + targetRanks.Length > byte.MaxValue - 1)
                return;

            ranks.AddRange(targetRanks.ToList());
            Log.Info($"Registering {targetRanks.Length} new ranks, bringing the total to {ranks.Count}. | Ranks: {ranks.Aggregate("", (current, rank) => current + $"{rank.DisplayName}[{rank.Id}], ")}");

            queueManager.RegisterRanks(ranks);
        }

        internal void RegisterClient(ConnectedClient client)
        {
            Log.Info($"New Client Joined | Rank: {client.Rank.DisplayName} | ConnectionId: {client.ConnectionId}");
            
            clients.Add(client);
            queueManager.RegisterPlayer(client);
        }
        
        internal void SendMessage(Message message, ushort toClient, bool shouldRelease = true) => server.Send(message, toClient, shouldRelease);

        internal ushort SendMessage(Message message, Connection toClient, bool shouldRelease = true) => server.Send(message, toClient, shouldRelease);

        internal void SendMessageToAll(Message message, bool shouldRelease = true) => server.SendToAll(message, shouldRelease);

        internal void SendMessageToAll(Message message, ushort exceptToClientId, bool shouldRelease = true) => server.SendToAll(message, exceptToClientId, shouldRelease);
    }
}