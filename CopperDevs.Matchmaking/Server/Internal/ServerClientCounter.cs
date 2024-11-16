using System.Linq;
using CopperDevs.Logger;
using Riptide;

namespace CopperDevs.Matchmaking.Server.Internal
{
    internal class ServerClientCounter
    {
        private readonly MatchmakerServer server;
        private ServerLobbyManager LobbyManager => server.LobbyManager;
        private ServerQueueManager QueueManager => server.QueueManager;
        private ServerHandler Handler => server.Handler;

        private int currentLobbyPlayerCount;
        private int currentQueuePlayerCount;

        public ServerClientCounter(MatchmakerServer server)
        {
            this.server = server;

            server.Server.ClientConnected += ClientConnected;
            server.Server.ClientDisconnected += ClientDisconnected;
        }
        
        ~ServerClientCounter()
        {
            server.Server.ClientConnected -= ClientConnected;
            server.Server.ClientDisconnected -= ClientDisconnected;
        }

        private void ClientConnected(object sender, ServerConnectedEventArgs serverConnectedEventArgs) => PlayerCountUpdateCheck();

        private void ClientDisconnected(object sender, ServerDisconnectedEventArgs serverDisconnectedEventArgs) => PlayerCountUpdateCheck();

        internal void PlayerCountUpdateCheck()
        {
            var lobbyPlayerCount = LobbyManager.Lobbies.Values.Sum(lobby => lobby.Count());
            var queuePlayerCount = QueueManager.RankQueues.Values.Sum(queues => queues.Count);

            if (lobbyPlayerCount == currentLobbyPlayerCount && currentQueuePlayerCount == queuePlayerCount) 
                return;
            
            currentLobbyPlayerCount = lobbyPlayerCount;
            currentQueuePlayerCount = queuePlayerCount;
            
            Handler.PlayerQueueCountUpdated(currentLobbyPlayerCount, currentQueuePlayerCount);
            Log.Info($"Current player count has been updated | Lobby player count: {currentLobbyPlayerCount} | Queue player count: {currentQueuePlayerCount}");
        }
    }
}