using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using CopperMatchmaking.Util;
using Riptide;

namespace CopperMatchmaking.Server
{
    internal static class ServerMessageHandlers
    {
        internal static void ClientJoinedMessageHandler(ushort sender, Message receivedMessage)
        {
            var playerId = receivedMessage.GetULong();
            var rankId = receivedMessage.GetByte();

            Log.Info($"Received new ClientJoined message. | PlayerId: {playerId} | RankId: {rankId} | Sender: {sender}");

            var connection = MatchmakerServer.Instance.Server.GetConnection(sender);
            var rank = MatchmakerServer.Instance.Ranks[rankId];

            if (connection is null)
            {
                Log.Info($"Disconnecting client due to connection being null | Sender: {sender}");
                return;
            }

            connection.CanQualityDisconnect = false;
            
            MatchmakerServer.Instance.RegisterClient(new ConnectedClient(rank, connection, playerId));
        }

        internal static void ClientHostLobbyIdMessageHandler(ushort sender, Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();
            var hostedLobbyId = receivedMessage.GetULong();

            Log.Info($"Received new ClientHostLobbyId message. | LobbyId: {lobbyId} | HostedLobbyId: {hostedLobbyId}");

            MatchmakerServer.Instance.LobbyManager.HandleClientHostResponse(lobbyId, hostedLobbyId);
        }
    }
}