using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using CopperMatchmaking.Util;
using Riptide;

namespace CopperMatchmaking.Server
{
    internal class ServerMessageHandlers
    {
        private MatchmakerServer targetServer;

        public ServerMessageHandlers(MatchmakerServer targetServer)
        {
            this.targetServer = targetServer;
        }

        internal void ServerReceivedMessageHandler(object sender, MessageReceivedEventArgs args)
        {
            Log.Info($"Received message of id {args.MessageId}.");
            
            switch (args.MessageId)
            {
                case 1:
                    Log.Info($"Received {nameof(MessageIds.ClientJoined)} message.");
                    ClientJoinedMessageHandler(args.FromConnection.Id, args.Message);
                    break;
                case 3:
                    Log.Info($"Received {nameof(MessageIds.ClientHostLobbyId)} message.");
                    ClientHostLobbyIdMessageHandler(args.FromConnection.Id, args.Message);
                    break;
                default:
                    Log.Warning($"Received unknown message of id {args.MessageId}.");
                    break;
            }
        }

        private void ClientJoinedMessageHandler(ushort sender, Message receivedMessage)
        {
            var playerId = receivedMessage.GetULong();
            var rankId = receivedMessage.GetByte();

            Log.Info($"Received new ClientJoined message. | PlayerId: {playerId} | RankId: {rankId} | Sender: {sender}");

            var connection = targetServer.Server.GetConnection(sender);
            var rank = MatchmakerServer.Ranks[rankId];

            if (connection is null)
            {
                Log.Info($"Disconnecting client due to connection being null | Sender: {sender}");
                return;
            }

            connection.CanQualityDisconnect = false;
            
            targetServer.RegisterClient(new ConnectedClient(rank, connection, playerId));
        }

        private void ClientHostLobbyIdMessageHandler(ushort sender, Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();
            var hostedLobbyId = receivedMessage.GetString();

            Log.Info($"Received new ClientHostLobbyId message. | LobbyId: {lobbyId} | HostedLobbyId: {hostedLobbyId}");

            targetServer.LobbyManager.HandleClientHostResponse(lobbyId, hostedLobbyId);
        }
    }
}