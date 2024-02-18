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
        }
        
        internal static void ServerRequestedClientToHostMessageHandler(Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();

            var hostedLobbyId = MatchmakerClient.Instance.Handler.ClientRequestedToHost();

            Log.Info($"Received new ServerRequestedClientToHost message. | LobbyId: {lobbyId} | Hosting new lobby. HostedLobbyId {hostedLobbyId}");

            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ClientHostLobbyId);
            message.Add(lobbyId);
            message.Add(hostedLobbyId);

            MatchmakerClient.Instance.Client.Send(message);
        }
        
        internal static void ClientJoinCreatedLobbyMessageHandler(Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetString();
            Log.Info($"Received new ClientJoinCreatedLobby message. | LobbyId: {lobbyId}");
            MatchmakerClient.Instance.Handler.JoinServer(lobbyId);
        }
    }
}