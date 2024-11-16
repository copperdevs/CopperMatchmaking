using System;
using CopperDevs.Matchmaking.Server;
using Riptide;
using RiptideConnection = Riptide.Connection;

namespace CopperDevs.Matchmaking.Data
{
    /// <summary>
    /// Data for all connected clients
    /// </summary>
    public class MatchmakingClient : IMessageSerializable
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

        internal MatchmakingClient(Rank rank, RiptideConnection riptideConnection, ulong playerId)
        {
            Rank = rank;
            RiptideConnection = riptideConnection;

            ConnectionId = riptideConnection.Id;
            PlayerId = playerId;
        }

        /// <summary>
        /// This is here for internal usage and passing a client through Riptide.
        /// It should not be used.
        /// </summary>
        [Obsolete]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MatchmakingClient()
        {
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        /// <summary>
        /// Get a clients rank
        /// </summary>
        /// <param name="client">Target client</param>
        /// <returns>Clients rank</returns>
        public static implicit operator Rank(MatchmakingClient client) => client.Rank;

        /// <summary>
        /// Get a clients riptide connection id
        /// </summary>
        /// <param name="client">Target client</param>
        /// <returns>Clients riptide connection id</returns>
        public static implicit operator uint(MatchmakingClient client) => client.ConnectionId;

        /// <summary>
        /// Get a clients riptide connect
        /// </summary>
        /// <param name="client">Target client</param>
        /// <returns>Clients riptide connection</returns>
        public static implicit operator RiptideConnection(MatchmakingClient client) => client.RiptideConnection;

        /// <summary>
        /// Update a clients rank from a 
        /// </summary>
        /// <param name="id">Rank id</param>
        public void UpdateRank(byte id)
        {
            Rank = MatchmakerServer.Ranks[id];
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