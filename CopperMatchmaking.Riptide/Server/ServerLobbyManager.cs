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

            {
                var message = Message.Create(MessageSendMode.Reliable, MessageIds.ServerRequestedClientToHost);
                message.Add(lobbyId);
                
                server.SendMessage(message, connectedClients[0]);
            }

            foreach (var client in connectedClients.Where(client => !(connectedClients.IndexOf(client) is 0)))
            {
                var message = Message.Create(MessageSendMode.Reliable);
                server.SendMessage(message, client);
            }
        }

        public void HandleClientHostResponse(uint lobbyId, ulong hostedLobbyId)
        {
            // Lobbies[lobbyId]
        }
    }
}