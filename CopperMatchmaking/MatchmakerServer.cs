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
                    // Log.Info($"New data received from client {connectionId}. Data: {BitConverter.ToString(message.Array, message.Offset, message.Count)}");
                    var receivedMessage = new Message(message);
                    Log.Info($"Received new data from client {connectionId}. Raw Data: {BitConverter.ToString(message.Array, message.Offset, message.Count)} | Message[{receivedMessage.Id}] receive is of type {((Message.MessageType)receivedMessage.Type).ToString()}. Data: {receivedMessage.GetData()}");
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