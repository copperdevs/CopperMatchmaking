using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using Riptide;

namespace CopperMatchmaking.Server
{
    internal class ServerLobbyManager
    {
        private readonly MatchmakerServer server;

        private readonly Dictionary<uint, List<ConnectedClient>> lobbies = new Dictionary<uint, List<ConnectedClient>>();

        internal ServerLobbyManager(MatchmakerServer server)
        {
            this.server = server;
        }

        internal void PotentialLobbyFound(List<ConnectedClient> connectedClients)
        {
            var lobbyId = connectedClients[0].ConnectionId;

            lobbies.Add(lobbyId, connectedClients);

            Log.Info($"Potential Lobby Found. Creating lobby with ConnectedClient[{connectedClients[0].ConnectionId}] as host.");

            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ServerRequestedClientToHost);
            message.Add(lobbyId);

            server.SendMessage(message, connectedClients[0]);
        }

        internal void HandleClientHostResponse(uint lobbyId, string hostedLobbyId)
        {
            Log.Info($"ConnectedClient[{lobbies[lobbyId][0].ConnectionId}] has responded with the join code of {hostedLobbyId}. Telling all clients of their lobby, and disconnecting them from the matchmaking server.");

            foreach (var client in lobbies[lobbyId].Where(client => !(lobbies[lobbyId].IndexOf(client) is 0)))
            {
                var message = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoinCreatedLobby);
                message.Add(hostedLobbyId);
                server.SendMessage(message, client);
            }

            foreach (var client in lobbies[lobbyId])
            {
                server.Server.DisconnectClient(client);
            }

            lobbies.Remove(lobbyId);
        }
    }
}