using CopperMatchmaking.Server;
using Riptide;
using RiptideConnection = Riptide.Connection;

namespace CopperMatchmaking.Data
{
    /// <summary>
    /// Data for all connected clients
    /// </summary>
    public class ConnectedClient : IMessageSerializable
    {
        /// <summary>
        /// Clients rank
        /// </summary>
        public Rank Rank { get; private set; }
        
        /// <summary>
        /// Riptides ConnectionId  
        /// </summary>
        public uint ConnectionId { get; private set; }
        
        /// <summary>
        /// Riptide Connection
        /// </summary>
        internal readonly RiptideConnection RiptideConnection;
        
        
        /// <summary>
        /// Player Id sent from the client 
        /// </summary>
        public ulong PlayerId { get; private set; }

        internal ConnectedClient(Rank rank, RiptideConnection riptideConnection, ulong playerId)
        {
            Rank = rank;
            this.RiptideConnection = riptideConnection;

            ConnectionId = riptideConnection.Id;
            PlayerId = playerId;
        }
        
        /// <summary>
        /// dont use this
        /// </summary>
        public ConnectedClient()
        {
        }

        /// <summary>
        /// Get a clients rank
        /// </summary>
        /// <param name="client">Target client</param>
        /// <returns>Clients rank</returns>
        public static implicit operator Rank(ConnectedClient client) => client.Rank;
        
        /// <summary>
        /// Get a clients riptide connection id
        /// </summary>
        /// <param name="client">Target client</param>
        /// <returns>Clients riptide connection id</returns>
        public static implicit operator uint(ConnectedClient client) => client.ConnectionId;
        
        /// <summary>
        /// Get a clients riptide connect
        /// </summary>
        /// <param name="client">Target client</param>
        /// <returns>Clients riptide connection</returns>
        public static implicit operator RiptideConnection(ConnectedClient client) => client.RiptideConnection;

        /// <summary>
        /// Update a clients rank from a 
        /// </summary>
        /// <param name="id">Rank id</param>
        public void UpdateRank(byte id)
        {
            Rank = MatchmakerServer.Instance.Ranks[id];
        }

        /// <summary>
        /// Riptide message serializing crap
        /// </summary>
        public void Serialize(Message message)
        {
            message.Add(ConnectionId);
            message.Add(Rank);
            message.Add(PlayerId);
        }

        /// <summary>
        /// Riptide message serializing crap
        /// </summary>
        public void Deserialize(Message message)
        {
            ConnectionId = message.GetUInt();
            Rank = message.GetSerializable<Rank>();
            PlayerId = message.GetULong();
        }
    }
}