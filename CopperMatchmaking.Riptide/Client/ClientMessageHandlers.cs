using CopperMatchmaking.Data;
using CopperMatchmaking.Utility;
using Riptide;

namespace CopperMatchmaking.Client
{
    public static class ClientMessageHandlers
    {
        // blank message handler
        // [MessageHandler((ushort))]
        // internal static void MessageHandler(Message receivedMessage)
        // {
        //     
        // }

        [MessageHandler((ushort)MessageIds.ServerRequestedClientToHost)]
        internal static void MessageHandler(Message receivedMessage)
        {
            
            var lobbyId = receivedMessage.GetUInt();

            var hostedLobbyId = MatchmakerClient.Instance.Handler.ClientRequestedToHost();

            Log.Info($"Received new ServerRequestedClientToHost message. | LobbyId: {lobbyId} | Hosting new lobby. HostedLobbyId {hostedLobbyId}");
            
            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ClientHostLobbyId);
            message.Add(lobbyId);
            message.Add(hostedLobbyId);

            MatchmakerClient.Instance.Client.Send(message);
        }


        [MessageHandler((ushort)MessageIds.ClientJoinCreatedLobby)]
        internal static void ClientJoinCreatedLobbyMessageHandler(Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetULong();
            Log.Info($"Received new ClientJoinCreatedLobby message. | LobbyId: {lobbyId}");
            MatchmakerClient.Instance.Handler.JoinServer(lobbyId);
        }
    }
}