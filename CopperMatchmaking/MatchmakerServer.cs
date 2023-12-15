using System;
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
                OnConnected = (connectionId) => Console.WriteLine(connectionId + " Connected"),
                OnData = (connectionId, message) => Console.WriteLine(connectionId + " Data: " + BitConverter.ToString(message.Array, message.Offset, message.Count)),
                OnDisconnected = (connectionId) => Console.WriteLine(connectionId + " Disconnected")
            };
        
            server.Start(7777);
        }

        public void Update()
        {
            server.Tick(100);
        }
    }
}