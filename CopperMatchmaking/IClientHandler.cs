namespace CopperMatchmaking
{
    public interface IClientHandler
    {
        public ulong ClientRequestedHost();
        public void JoinServer(ulong serverJoinCode);
    }
}