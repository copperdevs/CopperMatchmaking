using System;
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
    public class MatchmakerServer
    {
        internal static MatchmakerServer Instance = null!;
        
        private readonly RiptideServer server = null!;

        private readonly List<Rank> ranks = new List<Rank>();

        private readonly ServerQueueManager queueManager = null!;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        internal readonly ServerLobbyManager lobbyManager = null!;
        private readonly IServerHandler handler;


        public MatchmakerServer(ushort maxClients = 65534) : this(new BasicServerHandler(), maxClients)
        {
        }

        public MatchmakerServer(IServerHandler handler, ushort maxClients = 65534)
        {
            // values
            this.handler = handler;
            Instance = this;

            // checks
            if (maxClients % 2 != 0)
                return;

            // logs
            CopperLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError);
            RiptideLogger.Initialize(CopperLogger.LogInfo, CopperLogger.LogInfo, CopperLogger.LogWarning, CopperLogger.LogError, false);

            // networking
            server = new RiptideServer(new TcpServer());
            server.Start(7777, maxClients);

            // matchmaking 
            queueManager = new ServerQueueManager(maxClients);
            lobbyManager = new ServerLobbyManager(handler, this);

            // actions
            queueManager.PotentialLobbyFound += lobbyManager.PotentialLobbyFound;
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

            queueManager.RegisterRanks(ranks);
        }

        internal void SendMessage(Message message, ushort toClient, bool shouldRelease = true) => server.Send(message, toClient, shouldRelease);

        internal ushort SendMessage(Message message, Connection toClient, bool shouldRelease = true) => server.Send(message, toClient, shouldRelease);

        internal void SendMessageToAll(Message message, bool shouldRelease = true) => server.SendToAll(message, shouldRelease);

        internal void SendMessageToAll(Message message, ushort exceptToClientId, bool shouldRelease = true) => server.SendToAll(message, exceptToClientId, shouldRelease);
    }
}