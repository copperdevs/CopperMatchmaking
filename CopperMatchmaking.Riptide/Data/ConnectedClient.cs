using Riptide;

using RiptideConnection = Riptide.Connection;

namespace CopperMatchmaking.Data
{
    public class ConnectedClient
    {
        public readonly Rank Rank;
        public readonly uint ConnectionId;
        public readonly RiptideConnection RiptideConnection;
        
        public ConnectedClient(Rank rank, RiptideConnection riptideConnection)
        {
            Rank = rank;
            RiptideConnection = riptideConnection;

            ConnectionId = riptideConnection.Id;
        }
        
        public static implicit operator Rank(ConnectedClient client) => client.Rank;
        public static implicit operator uint(ConnectedClient client) => client.ConnectionId;
        public static implicit operator RiptideConnection(ConnectedClient client) => client.RiptideConnection;
    }
}