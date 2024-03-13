using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using Riptide;

namespace CopperMatchmaking.Client
{
    internal static class ClientMessageHandlers
    {
        internal static void ClientReceivedMessageHandler(object sender, MessageReceivedEventArgs args)
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

        private static void ServerRequestedClientToHostMessageHandler(Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();
            MatchmakerClient.Instance.CurrentLobbyCode = lobbyId;
            
            MatchmakerClient.Instance.NeededToHost = true;
            MatchmakerClient.Instance.Handler.ClientRequestedToHost();

            var clientCount = receivedMessage.GetInt();

            var clients = new List<ConnectedClient>();

            for (var i = 0; i < clientCount; i++) 
                clients.Add(receivedMessage.GetSerializable<ConnectedClient>());
            
            Log.Info($"Received new ServerRequestedClientToHost message. | LobbyId: {lobbyId} | Lobby Clients: <{clients.Aggregate("", (current, client) => current + $"(Client Id: {client.PlayerId} | Connection Id: {client.ConnectionId} | Rank: {client.Rank.DisplayName}[{client.Rank.Id}]), ")}>");
        }

        private static void ClientJoinCreatedLobbyMessageHandler(Message receivedMessage)
        {
            MatchmakerClient.Instance.NeededToHost = false;
            
            var lobbyId = receivedMessage.GetString();
            
            var clientCount = receivedMessage.GetInt();

            var clients = new List<ConnectedClient>();

            for (var i = 0; i < clientCount; i++) 
                clients.Add(receivedMessage.GetSerializable<ConnectedClient>());
            
            Log.Info($"Received new ClientJoinCreatedLobby message. | LobbyId: {lobbyId} | Lobby Clients: <{clients.Aggregate("", (current, client) => current + $"(Client Id: {client.PlayerId} | Connection Id: {client.ConnectionId} | Rank: {client.Rank.DisplayName}[{client.Rank.Id}]), ")}>");
            MatchmakerClient.Instance.Handler.JoinServer(lobbyId);
        }
    }
}