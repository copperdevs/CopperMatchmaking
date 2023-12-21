using System;
using System.Collections.Generic;
using System.Text;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;
using CopperMatchmaking.Telepathy;

namespace CopperMatchmaking
{
    public class MatchmakerClient
    {
        public readonly byte RankId;

        internal Client Client;
        
        public readonly Dictionary<byte, Action<Message>> MessageHandlers = new Dictionary<byte, Action<Message>>();

        private IClientHandler handler;


        public MatchmakerClient(Enum id, IClientHandler handler) : this(Convert.ToByte(id), handler)
        {
        }

        public MatchmakerClient(byte rankId, IClientHandler handler)
        {
            RankId = rankId;
            this.handler = handler;
            
            MessageHandlers.Add((byte)MessageIds.ServerClientHostRequest, ServerClientHostRequestMessageHandler);
            
            Client = new Client(Matchmaker.MaxMessageSize)
            {
                OnConnected = () => { Log.Info("Client Connected"); },
                OnData = message =>
                {
                    var receivedMessage = new Message(message);
                    
                    
                    if (MessageHandlers.ContainsKey(receivedMessage.Id))
                        MessageHandlers[receivedMessage.Id].Invoke(receivedMessage);
                    
                    Log.Info($"Received new data from server. Raw Data: {BitConverter.ToString(message.Array, message.Offset, message.Count)} | Message[{receivedMessage.Id}] receive is of type {((Message.MessageType)receivedMessage.Type).ToString()}. Data: {receivedMessage.GetData()}");
                },
                OnDisconnected = () => { Log.Info("Client Disconnected"); }
            };
        }

        public void Connect(string ip, int port)
        {
            Client.Connect(ip, port);

            Client.OnConnected += () =>
            {
                const string welcomeMessageValue = "hello world";

                Log.Info($"Sending welcome message '{welcomeMessageValue}'");
                var welcomeMessage = new Message((byte)MessageIds.ServerClientJoinedWelcomeMessage, welcomeMessageValue);
                Client.Send(welcomeMessage);

                Log.Info("Telling server the clients rank");
                var rankUpdate = new Message((byte)MessageIds.ClientRankUpdate, RankId);
                Client.Send(rankUpdate);
            };
        }

        public void Update()
        {
            Client.Tick(100);
        }
        
        
        // handlers
        private void ServerClientHostRequestMessageHandler(Message message)
        {
            // todo: get code and send to server
        }
    }
}