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
            
            Log.Info($"Received new ServerRequestedClientToHost message. | LobbyId: {lobbyId}");
        }

        private static void ClientJoinCreatedLobbyMessageHandler(Message receivedMessage)
        {
            MatchmakerClient.Instance.NeededToHost = false;
            
            var lobbyId = receivedMessage.GetString();
            Log.Info($"Received new ClientJoinCreatedLobby message. | LobbyId: {lobbyId}");
            MatchmakerClient.Instance.Handler.JoinServer(lobbyId);
        }
    }
}