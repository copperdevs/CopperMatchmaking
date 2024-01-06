namespace CopperMatchmaking.Data
{
    public enum MessageIds : ushort
    {
        // client from server to tell them their rank and userid
        ClientJoined = 1,
        // server to client to ask a client to host
        ServerRequestedClientToHost,
        // client to server to give the lobby id after hosting
        ClientHostLobbyId,
        // server to client to say "yo join the lobby"
        ClientJoinCreatedLobby,
    }
}