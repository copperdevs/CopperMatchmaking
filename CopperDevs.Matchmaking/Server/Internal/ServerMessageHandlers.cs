using CopperDevs.Logger;
using CopperDevs.Matchmaking.Data;
using Riptide;

namespace CopperDevs.Matchmaking.Server.Internal
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
            Log.Network($"Received message of id {args.MessageId}.");

            switch (args.MessageId)
            {
                case 1:
                    Log.Success($"Received {nameof(NetworkingMessageIds.ClientJoined)} message.");
                    ClientJoinedMessageHandler(args.FromConnection.Id, args.Message);
                    break;
                case 3:
                    Log.Success($"Received {nameof(NetworkingMessageIds.ClientHostLobbyId)} message.");
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

            Log.Debug($"Received new ClientJoined message. | PlayerId: {playerId} | RankId: {rankId} | Sender: {sender}");

            var connection = targetServer.TryGetClientConnection(sender);
            var rank = MatchmakerServer.Ranks[rankId];

            if (connection is null)
            {
                Log.Warning($"Disconnecting client due to connection being null | Sender: {sender}");
                return;
            }

            connection.CanQualityDisconnect = false;

            targetServer.RegisterClient(new MatchmakingClient(rank, connection, playerId));
        }

        private void ClientHostLobbyIdMessageHandler(ushort sender, Message receivedMessage)
        {
            var lobbyId = receivedMessage.GetUInt();
            var hostedLobbyId = receivedMessage.GetString();

            Log.Debug($"Received new ClientHostLobbyId message. | LobbyId: {lobbyId} | HostedLobbyId: {hostedLobbyId}");

            targetServer.LobbyManager.HandleClientHostResponse(lobbyId, hostedLobbyId);
        }
    }
}