using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Utility;
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

        internal void PotentialLobbyFound(List<ConnectedClient> connectedClients)
        {
            var lobbyId = connectedClients[0].ConnectionId;

            Lobbies.Add(lobbyId, connectedClients);

            Log.Info($"Potential Lobby Found. Creating lobby with ConnectedClient[{connectedClients[0].ConnectionId}] as host.");
            
            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ServerRequestedClientToHost);
            message.Add(lobbyId);

            server.SendMessage(message, connectedClients[0]);
        }

        internal void HandleClientHostResponse(uint lobbyId, ulong hostedLobbyId)
        {
            Log.Info($"ConnectedClient[{Lobbies[lobbyId][0].ConnectionId}] has responded with the join code of {hostedLobbyId}. Telling all clients of their lobby, and disconnecting them from the matchmaking server.");
            
            foreach (var client in Lobbies[lobbyId].Where(client => !(Lobbies[lobbyId].IndexOf(client) is 0)))
            {
                var message = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoinCreatedLobby);
                message.Add(hostedLobbyId);
                server.SendMessage(message, client);
            }
            
            foreach (var client in Lobbies[lobbyId])
            {
                server.server.DisconnectClient(client);
            }
            
            Lobbies.Remove(lobbyId);
        }
    }
}