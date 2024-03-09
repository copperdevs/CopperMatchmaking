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

        internal readonly Dictionary<uint, CreatedLobby> lobbies = new Dictionary<uint, CreatedLobby>();

        internal ServerLobbyManager(MatchmakerServer server)
        {
            this.server = server;
        }

        internal void PotentialLobbyFound(List<ConnectedClient> connectedClients, byte rank)
        {
            var host = connectedClients[server.Handler.ChooseLobbyHost(connectedClients)];

            var lobbyId = host.ConnectionId;

            lobbies.Add(lobbyId, new CreatedLobby(lobbyId, connectedClients, rank));

            Log.Info($"Potential Lobby Found. Creating lobby with ConnectedClient[{host.ConnectionId}] as host.");

            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ServerRequestedClientToHost);
            message.Add(lobbyId);

            server.SendMessage(message, host);

            server.Handler.LobbyCreated(lobbies[lobbyId]);
        }

        internal void HandleClientHostResponse(uint lobbyId, string hostedLobbyId)
        {
            if (!lobbies.ContainsKey(lobbyId))
            {
                Log.Info($"Client has seen join code for lobby {lobbyId}. However there is no lobby with id '{lobbyId}'. It might have timed out or the client is lying.");
                return;
            }

            Log.Info(
                $"ConnectedClient[{lobbies[lobbyId][0].ConnectionId}] has responded with the join code of {hostedLobbyId}. Telling all clients of their lobby, and disconnecting them from the matchmaking server.");

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

        internal void ClientDisconnected(object sender, ServerDisconnectedEventArgs args)
        {
            if (args.Reason != DisconnectReason.Kicked)
                DisconnectClient(args.Client);
        }

        private void DisconnectClient(Connection connection)
        {
            foreach (var lobby in lobbies)
            {
                if(lobby.Key == connection.Id)
                    RemoveLobby(connection, lobby.Value);

                foreach (var client in lobby.Value.LobbyClients.Where(client => client.RiptideConnection.Id == connection.Id))
                {
                    RemoveLobby(client.RiptideConnection, lobby.Value);
                }
            }
            
            return;

            void RemoveLobby(Connection clientConnection, CreatedLobby lobby)
            {
                Log.Info($"Removing lobby due to someone disconnect. | Client: {clientConnection.Id} | Lobby: {lobby.LobbyId}");
                
                lobbies.Remove(lobby.LobbyId);
                
                lobby.Skip(1).ToList().ForEach(server.QueueManager.RegisterPlayer);
                server.QueueManager.RegisterPlayer(lobby[0]);
            }
        }
    }
}