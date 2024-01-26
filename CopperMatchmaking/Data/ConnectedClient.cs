using RiptideConnection = Riptide.Connection;

namespace CopperMatchmaking.Data
{
    public class ConnectedClient
    {
        public readonly Rank Rank;
        public readonly uint ConnectionId;
        private readonly RiptideConnection riptideConnection;
        public readonly ulong PlayerId;

        internal ConnectedClient(Rank rank, RiptideConnection riptideConnection, ulong playerId)
        {
            Rank = rank;
            this.riptideConnection = riptideConnection;

            ConnectionId = riptideConnection.Id;
            this.PlayerId = playerId;
        }

        public static implicit operator Rank(ConnectedClient client) => client.Rank;
        public static implicit operator uint(ConnectedClient client) => client.ConnectionId;
        public static implicit operator RiptideConnection(ConnectedClient client) => client.riptideConnection;
    }
}