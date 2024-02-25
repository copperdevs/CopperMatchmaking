using System;
using System.Collections;
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

        private readonly Dictionary<uint, CreatedLobby> lobbies = new Dictionary<uint, CreatedLobby>();

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

        internal void TimeoutCheck()
        {
            foreach (var lobby in lobbies.Values.ToList().Where(lobby => (DateTime.Now - lobby.LobbyCreationTime).Seconds >= server.LobbyTimeoutTime))
            {
                Log.Info($"The host of lobby {lobby.LobbyId} has taken too long to send the join code. Timing out the lobby.");
                lobbies.Remove(lobby.LobbyId);
                
                server.QueueManager.ReturnLobby(lobby);
            }
        }
    }
}