using System;
using CopperMatchmaking.Info;
using CopperMatchmaking.Telepathy;

namespace CopperMatchmaking
{
    public class MatchmakerServer
    {
        private readonly Server server;

        public MatchmakerServer()
        {
            server = new Server(Matchmaker.MaxMessageSize)
            {
                OnConnected = connectionId =>
                {
                    Log.Info($"Client {connectionId} connected");
                },
                OnData = (connectionId, message) =>
                {
                    Log.Info($"New data received from client {connectionId}. Data: {BitConverter.ToString(message.Array, message.Offset, message.Count)}");
                },
                OnDisconnected = connectionId =>
                {
                    Log.Info($"Client {connectionId} Disconnected");
                }
            };
        
            server.Start(7777);
        }

        public void Update()
        {
            server.Tick(100);
        }
    }
}