using System.Collections.Generic;
using System.Linq;
using CopperDevs.Matchmaking.Data;
using CopperDevs.Matchmaking.Info;
using Riptide;

namespace CopperDevs.Matchmaking.Client
{
    internal class ClientMessageHandlers
    {
        private MatchmakerClient targetClient;

        public ClientMessageHandlers(MatchmakerClient targetClient)
        {
            this.targetClient = targetClient;
        }

        internal void ClientReceivedMessageHandler(object sender, MessageReceivedEventArgs args)
        {
            Log.Info($"Received message of id {args.MessageId}.");
            
            switch (args.MessageId)
            {
                case 2:
                    Log.Info($"Received {nameof(MessageIds.ServerRequestedClientToHost)} message.");
                    ServerRequestedClientToHostMessageHandler(args.Message);
                    break;
                case 4:
                    Log.Info($"Received {nameof(MessageIds.ClientJoinCreatedLobby)} message.");
                    ClientJoinCreatedLobbyMessageHandler(args.Message);
                    break;
                default:
                    Log.Warning($"Received unknown message of id {args.MessageId}.");
                    break;
            }
        }

        private void ServerRequestedClientToHostMessageHandler(Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();
            targetClient.CurrentLobbyCode = lobbyId;
            
            targetClient.NeededToHost = true;
            targetClient.Handler.ClientRequestedToHost();

            var clientCount = receivedMessage.GetInt();

            var clients = new List<ConnectedClient>();

            for (var i = 0; i < clientCount; i++) 
                clients.Add(receivedMessage.GetSerializable<ConnectedClient>());
            
            Log.Info($"Received new ServerRequestedClientToHost message. | LobbyId: {lobbyId} | Lobby Clients: <{clients.Aggregate("", (current, client) => current + $"(Client Id: {client.PlayerId} | Connection Id: {client.ConnectionId} | Rank: {client.Rank.DisplayName}[{client.Rank.Id}]), ")}>");
        }

        private void ClientJoinCreatedLobbyMessageHandler(Message receivedMessage)
        {
            targetClient.NeededToHost = false;
            
            var lobbyId = receivedMessage.GetString();
            
            var clientCount = receivedMessage.GetInt();

            var clients = new List<ConnectedClient>();

            for (var i = 0; i < clientCount; i++) 
                clients.Add(receivedMessage.GetSerializable<ConnectedClient>());
            
            Log.Info($"Received new ClientJoinCreatedLobby message. | LobbyId: {lobbyId} | Lobby Clients: <{clients.Aggregate("", (current, client) => current + $"(Client Id: {client.PlayerId} | Connection Id: {client.ConnectionId} | Rank: {client.Rank.DisplayName}[{client.Rank.Id}]), ")}>");
            targetClient.Handler.JoinServer(lobbyId);
        }
    }
}