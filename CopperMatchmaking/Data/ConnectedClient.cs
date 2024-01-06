using RiptideConnection = Riptide.Connection;

namespace CopperMatchmaking.Data
{
    internal class ConnectedClient
    {
        internal readonly Rank Rank;
        internal readonly uint ConnectionId;
        internal readonly RiptideConnection RiptideConnection;

        internal ConnectedClient(Rank rank, RiptideConnection riptideConnection)
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