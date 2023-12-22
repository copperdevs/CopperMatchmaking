namespace CopperMatchmaking.Data
{
    // first word is who the message is from/sending it
    public enum MessageIds
    {
        // client telling the server its rank
        ClientRankUpdate = 1,
        
        ServerMatchFound = 2,
        ServerClientHostRequest = 3,
        ServerClientJoinServer = 4,
        
        ClientHostJoinCode = 5,


        ServerClientJoinedWelcomeMessage = 255
    }
}