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

        internal readonly Dictionary<uint, CreatedLobby> Lobbies = new Dictionary<uint, CreatedLobby>();

        internal ServerLobbyManager(MatchmakerServer server)
        {
            this.server = server;
        }

        internal void PotentialLobbyFound(List<ConnectedClient> connectedClients, byte rank)
        {
            var host = connectedClients[server.Handler.ChooseLobbyHost(connectedClients)];

            if (Lobbies.ContainsKey(host.ConnectionId))
            {
                Log.Warning($"Potential lobby attempting to be created has an id the same as a lobby already created");
                return;
            }
            
            var lobbyId = host.ConnectionId;

            Lobbies.Add(lobbyId, new CreatedLobby(lobbyId, connectedClients, rank));

            Log.Info($"Potential Lobby Found. Creating lobby with ConnectedClient[{host.ConnectionId}] as host.");

            var message = Message.Create(MessageSendMode.Reliable, MessageIds.ServerRequestedClientToHost);
            message.Add(lobbyId);

            message.Add(connectedClients.Count);

            foreach (var client in connectedClients)
            {
                message.Add(client);
            }

            server.SendMessage(message, host);

            server.Handler.LobbyCreated(Lobbies[lobbyId]);
            server.LobbyCreated?.Invoke(Lobbies[lobbyId]);
        }

        internal void HandleClientHostResponse(uint lobbyId, string hostedLobbyId)
        {
            if (!Lobbies.ContainsKey(lobbyId))
            {
                Log.Info($"Client has seen join code for lobby {lobbyId}. However there is no lobby with id '{lobbyId}'. It might have timed out or the client is lying.");
                return;
            }

            Log.Info($"ConnectedClient[{Lobbies[lobbyId][0].ConnectionId}] has responded with the join code of {hostedLobbyId}. Telling all clients of their lobby, and disconnecting them from the matchmaking server.");

            foreach (var client in Lobbies[lobbyId].Where(client => !(Lobbies[lobbyId].IndexOf(client) is 0)))
            {
                var message = Message.Create(MessageSendMode.Reliable, MessageIds.ClientJoinCreatedLobby);
                message.Add(hostedLobbyId);

                message.Add(Lobbies[lobbyId].LobbyClients.Count);

                foreach (var lobbyClients in Lobbies[lobbyId].LobbyClients) 
                    message.Add(lobbyClients);
                
                server.SendMessage(message, client);
            }

            foreach (var client in Lobbies[lobbyId])
            {
                server.Server.DisconnectClient(client);
            }

            Lobbies.Remove(lobbyId);
        }

        internal void ClientDisconnected(object sender, ServerDisconnectedEventArgs args)
        {
            if (args.Reason != DisconnectReason.Kicked)
                DisconnectClient(args.Client);
        }

        private void DisconnectClient(Connection connection)
        {
            foreach (var lobby in Lobbies)
            {
                if (lobby.Key == connection.Id)
                {
                    RemoveLobby(connection, lobby.Value);
                    return;
                }

                foreach (var client in lobby.Value.LobbyClients.Where(client => client.RiptideConnection.Id == connection.Id))
                {
                    RemoveLobby(client.RiptideConnection, lobby.Value);
                }
            }
        }

        internal void DisconnectedPlayerCheck()
        {
            foreach (var lobby in Lobbies.Values.ToList())
            {
                foreach (var client in lobby)
                {
                    if(!client.RiptideConnection.IsConnected)
                        RemoveLobby(client.RiptideConnection, lobby);
                }
            }
        }
        
        private void RemoveLobby(Connection clientConnection, CreatedLobby lobby)
        {
            Log.Info($"Removing lobby due to someone disconnecting. | Client: {clientConnection.Id} | Lobby: {lobby.LobbyId}");
                
            Lobbies.Remove(lobby.LobbyId);

            foreach (var client in lobby)
            {
                if(client.RiptideConnection.IsConnected)
                    server.QueueManager.RegisterPlayer(client);
            }
        }
    }
}