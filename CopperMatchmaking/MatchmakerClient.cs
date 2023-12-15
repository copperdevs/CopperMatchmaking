using System;
using System.Text;
using CopperMatchmaking.Telepathy;

namespace CopperMatchmaking
{
    public class MatchmakerClient
    {
        public readonly byte RankId;

        internal Client Client;

        public MatchmakerClient(Enum id) : this(Convert.ToByte(id))
        {
        }
    
        public MatchmakerClient(byte rankId)
        {
            RankId = rankId;
            Client = new Client(Matchmaker.MaxMessageSize)
            {
                OnConnected = () => Console.WriteLine("Client Connected"),
                OnData = message => Console.WriteLine("Client Data: " + BitConverter.ToString(message.Array, message.Offset, message.Count)),
                OnDisconnected = () => Console.WriteLine("Client Disconnected")
            };
        }

        public void Connect(string ip, int port)
        {
            Client.Connect(ip, port);

            Client.OnConnected += () =>
            {
                var message = Encoding.UTF8.GetBytes("hello world");
                Client.Send(new ArraySegment<byte>(message));
            };
        }

        public void Update()
        {
            Client.Tick(100);
        }
    }
}