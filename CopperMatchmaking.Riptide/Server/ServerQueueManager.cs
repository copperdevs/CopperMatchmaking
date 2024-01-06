using System;
using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Utility;

namespace CopperMatchmaking.Server
{
    internal class ServerQueueManager
    {
        // byte is used here but its just the rank class
        private readonly Dictionary<byte, List<ConnectedClient>> rankQueues = new Dictionary<byte, List<ConnectedClient>>();

        private ushort lobbySize;

        internal Action<List<ConnectedClient>>? PotentialLobbyFound;

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
        
    }
}