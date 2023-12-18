using System;
using System.Collections.Generic;
using CopperMatchmaking.Info;
using CopperMatchmaking.Telepathy;

namespace CopperMatchmaking
{
    public class MatchmakerServer
    {
        internal readonly Server server;
        private static List<int> queuedClients = new List<int>();

        public Dictionary<byte, Action<int, Message>> MessageHandlers = new Dictionary<byte, Action<int, Message>>();

        // connection id, rank,
        public Action<int, ConnectedClient> ClientConnected;
        
        // connection id,
        public Action<int> ClientDisconnected;

        public MatchmakerServer()
        {
            MessageHandlers.Add((byte)MessageIds.ClientRankUpdate, ClientRankUpdateMessageHandler);
            
            server = new Server(Matchmaker.MaxMessageSize)
            {
                OnConnected = connectionId =>
                {
                    Log.Info($"Client {connectionId} connected");
                    queuedClients.Add(connectionId);
                },
                
                OnData = (connectionId, message) =>
                {
                    var receivedMessage = new Message(message);
                    
                    if (MessageHandlers.ContainsKey(receivedMessage.Id))
                        MessageHandlers[receivedMessage.Id].Invoke(connectionId, receivedMessage);
                    
                    Log.Info($"Received new data from client {connectionId}. Raw Data: {BitConverter.ToString(message.Array, message.Offset, message.Count)} | Message[{receivedMessage.Id}] receive is of type {((Message.MessageType)receivedMessage.Type).ToString()}. Data: {receivedMessage.GetData()}");
                },
                
                OnDisconnected = connectionId =>
                {
                    Log.Info($"Client {connectionId} Disconnected");
                    
                    ClientDisconnected?.Invoke(connectionId);
                    
                    if (queuedClients.Contains(connectionId))
                        queuedClients.Remove(connectionId);
                }
            };

            server.Start(7777);
        }

        public void Update()
        {
            server.Tick(100);
        }

        public void ClientRankUpdateMessageHandler(int connectionId, Message message)
        {
            if (queuedClients.Contains(connectionId))
                queuedClients.Remove(connectionId);
            else
                return;
            
            ClientConnected?.Invoke(connectionId, new ConnectedClient(connectionId, (short)message.GetData()));
        }

        public bool Send(int connectionId, ArraySegment<byte> message)
        {
            return server.Send(connectionId, message);
        }
    }
}