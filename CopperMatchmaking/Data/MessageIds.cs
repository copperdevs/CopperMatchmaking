namespace CopperMatchmaking.Data
{
    // first word is who the message is from/sending it
    public enum MessageIds
    {
        // client telling the server its rank
        ClientRankUpdate = 1,
        
        // server
        ServerMatchFound = 2,
        ServerClientHostRequest = 3,
        ClientHostJoinCode = 4,


        ServerClientJoinedWelcomeMessage = 255
    }
}