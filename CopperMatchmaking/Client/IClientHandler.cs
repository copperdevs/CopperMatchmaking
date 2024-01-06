namespace CopperMatchmaking.Client
{
    public interface IClientHandler
    {
        public ulong ClientRequestedToHost();
        public void JoinServer(ulong serverJoinCode);
    }
}