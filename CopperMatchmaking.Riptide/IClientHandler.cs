namespace CopperMatchmaking
{
    public interface IClientHandler
    {
        public ulong ClientRequestedToHost();
        public void JoinServer(ulong serverJoinCode);
    }
}