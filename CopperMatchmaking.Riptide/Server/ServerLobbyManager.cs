using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using Riptide;

namespace CopperMatchmaking.Server
{
    internal class ServerLobbyManager
    {
        private readonly IServerHandler handler;
        private readonly MatchmakerServer server;

        internal Dictionary<uint, List<ConnectedClient>> Lobbies = new Dictionary<uint, List<ConnectedClient>>();

        public ServerLobbyManager(IServerHandler handler, MatchmakerServer server)
        {
            this.handler = handler;
            this.server = server;
        }

        public void PotentialLobbyFound(List<ConnectedClient> connectedClients)
        {
            var lobbyId = connectedClients[0].ConnectionId;

            Lobbies.Add(lobbyId, connectedClients);


            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ServerRequestedClientToHost);
            message.Add(lobbyId);

            server.SendMessage(message, connectedClients[0]);
        }

        public void HandleClientHostResponse(uint lobbyId, ulong hostedLobbyId)
        {
            foreach (var client in Lobbies[lobbyId].Where(client => !(Lobbies[lobbyId].IndexOf(client) is 0)))
            {
                var message = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoinCreatedLobby);
                message.Add(hostedLobbyId);
                server.SendMessage(message, client);
            }
        }
    }
}