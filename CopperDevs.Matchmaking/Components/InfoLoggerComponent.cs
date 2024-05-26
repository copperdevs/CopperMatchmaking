using System;
using System.Linq;
using CopperDevs.Matchmaking.Info;
using CopperDevs.Matchmaking.Server;

namespace CopperDevs.Matchmaking.Components
{
    /// <summary>
    /// Component for logging major info about the server
    /// </summary>
    [ServerOnlyComponent]
    public class InfoLoggerComponent : BaseComponent
    {
        /// <summary>
        /// Time in seconds between each group of logs
        /// </summary>
        public float LogDistance = 1;
        private DateTime lastLogTime;

        /// <summary>
        /// Start
        /// </summary>
        protected internal override void Start()
        {
            lastLogTime = DateTime.Now;
        }

        /// <summary>
        /// Update
        /// </summary>
        protected internal override void Update()
        {
            if (!((DateTime.Now - lastLogTime).Seconds >= LogDistance))
                return;

            lastLogTime = DateTime.Now;

            var ranks = MatchmakerServer.GetAllRanks();
            var currentLobbies = Server?.GetCurrentLobbies();
            var currentQueues = Server?.GetRankQueues();
            
            Log.Info($"Server Ranks | Current Ranks: {ranks?.Aggregate("", (current, rank) => current + $"{rank.DisplayName}[{rank.Id}], ")}");

            foreach (var lobby in currentLobbies!)
            {
                Log.Info($"Created Lobby | Lobby {lobby.LobbyId} | Creation Time: {lobby.LobbyCreationTime} | Lobby Rank: {lobby.LobbyRank} | Lobby Clients: <{lobby.LobbyClients.Aggregate("", (current, client) => current + $"(Client Id: {client.PlayerId} | Connection Id: {client.ConnectionId} | Rank: {client.Rank.DisplayName}[{client.Rank.Id}]), ")}>");
            }

            for (byte i = 0; i < currentQueues?.Count; i++)
            {
                Log.Info($"Rank Queue | Queue Rank: {ranks?[i].DisplayName}[{ranks?[i].Id}] | Queue Clients: <{currentQueues[i].Aggregate("", (current, client) => current + $"(Client Id: {client.PlayerId} | Connection Id: {client.ConnectionId} | Rank: {client.Rank.DisplayName}[{client.Rank.Id}]), ")}>");
            }
        }
    }
}