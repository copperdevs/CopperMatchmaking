using System;
using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using Riptide;

namespace CopperMatchmaking.Server
{
    internal class ServerQueueManager
    {
        // byte is used here but its just the rank class
        private readonly Dictionary<byte, List<ConnectedClient>> rankQueues = new Dictionary<byte, List<ConnectedClient>>();

        private readonly ushort lobbySize;

#nullable enable
        internal Action<List<ConnectedClient>>? PotentialLobbyFound;
#nullable disable

        public ServerQueueManager(ushort lobbySize)
        {
            this.lobbySize = lobbySize;
        }

        internal void RegisterRanks(List<Rank> ranks)
        {
            rankQueues.Clear();

            foreach (var rank in ranks)
            {
                rankQueues.Add(rank, new List<ConnectedClient>());
            }
        }

        internal void CheckForLobbies()
        {
            foreach (var queue in rankQueues)
            {
                if (queue.Value.Count < lobbySize)
                    continue;

                // takes a lobby size quantity of players and removes them from their queue
                var connectedClients = queue.Value.Take(lobbySize).ToList();
                connectedClients.ForEach(client => queue.Value.Remove(client));

                PotentialLobbyFound?.Invoke(connectedClients.ToList());
            }
        }

        internal void RegisterPlayer(ConnectedClient client)
        {
            rankQueues[client.Rank].Add(client);
            Log.Info($"Registered new client to {GetType().Name}");
        }

        // This function is used instead of the DisconnectClient below so it can be connected to riptide servers client disconnected callback
        internal void ClientDisconnected(object sender, ServerDisconnectedEventArgs args)
        {
            Log.Info($"Client disconnected");
            DisconnectClient(args.Client);
        }
        
        internal void DisconnectClient(Connection connection)
        {
            for (var i = 0; i < rankQueues.Values.ToList().Count; i++)
            {
                var connectedClients = rankQueues.Values.ToList()[i];
                
                for (var ii = 0; ii < connectedClients.Count; ii++)
                {
                    var client = connectedClients[ii];

                    if (client.RiptideConnection != connection) 
                        continue;
                    
                    Log.Info($"Removing client {connection.Id} due to being disconnected");
                    rankQueues[(byte)i].RemoveAt(ii);
                }
            }
        }
    }
}