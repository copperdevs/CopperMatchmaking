using System;
using CopperDevs.Logger;
using CopperDevs.Matchmaking.Data;
using Riptide;
using Riptide.Transports.Tcp;
using Riptide.Utils;
using RiptideClient = Riptide.Client;

namespace CopperDevs.Matchmaking.Client
{
    /// <summary>
    /// Matchmaker client for connecting to the matchmaker with
    /// </summary>
    public partial class MatchmakerClient
    {
        /// <summary>
        /// Enabled when <see cref="Update"/> needs to be ran to update the client.
        /// </summary>
        public bool ShouldUpdate { get; private set; }

        internal readonly RiptideClient Client;
        internal readonly BaseClientHandler Handler;
        internal readonly ClientMessageHandlers MessageHandlers;

        private readonly byte rankId;
        private readonly ulong playerId;

        internal bool NeededToHost = false;
        internal uint CurrentLobbyCode = 0;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="ip">Target ip of the matchmaker server</param>
        /// <param name="baseClientHandler">Handler for the client</param>
        /// <param name="rankId">Id of the clients rank</param>
        /// <param name="playerId">Player id (SteamId for example)</param>
        public MatchmakerClient(string ip, BaseClientHandler baseClientHandler, byte rankId, ulong playerId)
        {
            // init logs
            RiptideLogger.Initialize(Log.Debug, Log.Info, Log.Warning, Log.Error, false);

            // values/handlers
            this.rankId = rankId;
            this.playerId = playerId;
            Handler = baseClientHandler;
            Handler.Client = this;
            MessageHandlers = new ClientMessageHandlers(this);

            // start riptide crap
            Client = new RiptideClient(new TcpClient());
            ShouldUpdate = true;
            
            Client.Connect($"{ip}:7777", 5, 0, null, false);
            Client.Connection.CanQualityDisconnect = false;
            
            Client.Connected += ClientConnectedHandler;
            Client.MessageReceived += MessageHandlers.ClientReceivedMessageHandler;
            Client.Disconnected += ClientDisconnectedHandler;
        }

        ~MatchmakerClient()
        {
            ShouldUpdate = false;
            Client.Connected -= ClientConnectedHandler;
            Client.MessageReceived -= MessageHandlers.ClientReceivedMessageHandler;
            Client.Disconnected -= ClientDisconnectedHandler;
        }


        /// <summary>
        /// Method to run often to update the client
        /// </summary>
        public void Update()
        {
            if (!ShouldUpdate) 
                return;
            
            UpdateComponents();
            Client.Update();
        }

        private void ClientConnectedHandler(object sender, EventArgs eventArgs)
        {
            var joinMessage = Message.Create(MessageSendMode.Reliable, NetworkingMessageIds.ClientJoined);

            joinMessage.Add(playerId); // ulong
            joinMessage.Add(rankId); // byte

            Log.Info($"Creating client join message. | PlayerId {playerId} | RankId {rankId}");
            Client.Send(joinMessage);
        }
        
        private void ClientDisconnectedHandler(object sender, DisconnectedEventArgs args)
        {
            ShouldUpdate = false;
            Log.Info($"Client disconnected | Reason: {args.Reason}");
            Handler.Disconnected(args.Reason);
        }

        /// <summary>
        /// When requested in <see cref="BaseClientHandler"/> to host, you can call this function to send the lobby id.
        /// </summary>
        /// <param name="hostedLobbyId"></param>
        public void SendLobbyCode(string hostedLobbyId)
        {
            if (!NeededToHost)
            {
                Log.Error($"Client is trying to send a lobby code but is not currently needed to host a lobby.");
                return;
            }
            
            var message = Message.Create(MessageSendMode.Reliable, NetworkingMessageIds.ClientHostLobbyId);
            message.Add(CurrentLobbyCode);
            message.Add(hostedLobbyId);

            Client.Send(message);
            NeededToHost = false;
        }


        /// <summary>
        /// Disconnect the client from matchmaking
        /// </summary>
        /// <param name="silent">If enabled, the disconnect method of the <see cref="BaseClientHandler"/> won't be called</param>
        public void Disconnect(bool silent = true)
        {
            Log.Info($"Disconnecting the matchmaking client");
            
            NeededToHost = false;
            ShouldUpdate = false;
            
            Client.Connected -= ClientConnectedHandler;
            Client.MessageReceived -= MessageHandlers.ClientReceivedMessageHandler;
            Client.Disconnected -= ClientDisconnectedHandler;
            
            Client.Disconnect();
            
            if(!silent)
                Handler.Disconnected(DisconnectReason.Disconnected);
        }
    }
}