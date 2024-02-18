using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using CopperMatchmaking.Util;
using Riptide;

namespace CopperMatchmaking.Server
{
    internal static class ServerMessageHandlers
    {
        internal static void ServerReceivedMessageHandler(object sender, MessageReceivedEventArgs args)
        {
            Log.Info($"Received message of id {args.MessageId}.");
            
            switch (args.MessageId)
            {
                case 1:
                    Log.Info($"Received {nameof(MessageIds.ClientJoined)} message.");
                    ServerMessageHandlers.ClientJoinedMessageHandler(args.FromConnection.Id, args.Message);
                    break;
                case 3:
                    Log.Info($"Received {nameof(MessageIds.ClientHostLobbyId)} message.");
                    ServerMessageHandlers.ClientHostLobbyIdMessageHandler(args.FromConnection.Id, args.Message);
                    break;
                default:
                    Log.Warning($"Received unknown message of id {args.MessageId}.");
                    break;
            }
        }
        
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

            connection.CanQualityDisconnect = true;
            
            MatchmakerServer.Instance.RegisterClient(new ConnectedClient(rank, connection, playerId));
        }

        internal static void ClientHostLobbyIdMessageHandler(ushort sender, Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();
            var hostedLobbyId = receivedMessage.GetString();

            Log.Info($"Received new ClientHostLobbyId message. | LobbyId: {lobbyId} | HostedLobbyId: {hostedLobbyId}");

            MatchmakerServer.Instance.LobbyManager.HandleClientHostResponse(lobbyId, hostedLobbyId);
        }
    }
}