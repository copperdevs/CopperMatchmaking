using CopperMatchmaking.Data;
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

            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ClientHostLobbyId);
            message.Add(lobbyId);
            message.Add(hostedLobbyId);

            MatchmakerClient.Instance.Client.Send(message);
        }
    }
}