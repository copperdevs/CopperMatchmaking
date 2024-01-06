using CopperMatchmaking.Data;
using Riptide;

namespace CopperMatchmaking.Server
{
    internal static class ServerMessageHandlers
    {
        // blank message handler
        // [MessageHandler((ushort))]
        // internal static void MessageHandler(ushort sender, Message receivedMessage)
        // {
        //     
        // }

        [MessageHandler((ushort)MessageIds.ClientJoined)]
        internal static void ClientJoinedMessageHandler(ushort sender, Message receivedMessage)
        {
            var playerId = receivedMessage.GetUShort();
            var rankId = receivedMessage.GetByte();
        }

        [MessageHandler((ushort)MessageIds.ClientHostLobbyId)]
        internal static void ClientHostLobbyIdMessageHandler(ushort sender, Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();
            var hostedLobbyId = receivedMessage.GetULong();

            MatchmakerServer.Instance.LobbyManager.HandleClientHostResponse(lobbyId, hostedLobbyId);
        }
    }
}