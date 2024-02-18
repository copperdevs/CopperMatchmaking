# CopperMatchmaking

> Simple matchmaking server for creating two even player count teams for use in peer to peer

## Use Case

`CopperMatchmaking` was made for games with two teams, each with the same amount of players (5 for example), and for one
of those players to be the host in a peer to peer architecture. It also has ranks built in for pooling players.

## Installation

1. Install [RiptideNetworking](https://github.com/RiptideNetworking/Riptide?tab=readme-ov-file). Instructions for installing riptide can be found [here](https://riptide.tomweiland.net/manual/overview/installation.html).
2. Install matchmaking
   - Unity package manager: `https://github.com/copperdevs/CopperMatchmaking.git?path=/unity`
   - [NuGet](https://www.nuget.org/packages/CopperMatchmaking):  `dotnet add package CopperMatchmaking --version 1.2.0`
   
## Getting Started

### Server

1. Create ranks. The matchmaker creates a pool of players for each rank. Each connected client can only be matched
   into a lobby with players of the same rank.

    ```csharp
    public enum RankIds : byte
    {
        Unranked = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
        Diamond = 5,
        Master = 6,
        Chaos = 7
    }
    ```

2. Create the server. The constructor for `MatchmakerServer` has two parameters both with defaults.
    - `lobbySize`
        - `lobbySize` determines the amount of players in a lobby. This has to be an even number so both teams can have
          the same amount of players. The default is set to 10.
    - `maxClients`
        - `maxClients` determines the max amount of clients that can be connected to the matchmaker server. The default
          is set to 65534.

    ```csharp
    var server = new MatchmakerServer();
    ```

3. Once we have created the server and our ranks, we can register our ranks with the server. You can a max of register
   254 ranks. Once a rank has been registered it can not be removed. Calling `RegisterRanks` again will only add the
   ranks to the list again.
   ```csharp
    server.RegisterRanks(
        new Rank("Unranked", RankIds.Unranked),
        new Rank("Bronze", RankIds.Bronze),
        new Rank("Silver", RankIds.Silver),
        new Rank("Gold", RankIds.Gold),
        new Rank("Platinum", RankIds.Platinum),
        new Rank("Diamond", RankIds.Diamond),
        new Rank("Master", RankIds.Master),
        new Rank("Chaos", RankIds.Chaos));
    ```

4. Now you just have to consistently update the server. This can be done in a `while(true)` loop or in unitys built
   in `FixedUpdate` method.
    ```csharp
        while (true)
        {
            server.Update();
        }
    ```

    ```csharp
        private void FixedUpdate()
        {
            server.Update();
        }
    ```

5. Full example. Can also be found in the [CopperMatchmaking.Example.Server](CopperMatchmaking.Example.Server/Program.cs) project.
    ```csharp
        var server = new MatchmakerServer();
        server.RegisterRanks(
            new Rank("Unranked", RankIds.Unranked),
            new Rank("Bronze", RankIds.Bronze),
            new Rank("Silver", RankIds.Silver),
            new Rank("Gold", RankIds.Gold),
            new Rank("Platinum", RankIds.Platinum),
            new Rank("Diamond", RankIds.Diamond),
            new Rank("Master", RankIds.Master),
            new Rank("Chaos", RankIds.Chaos));
    
        while (true)
        {
            server.Update();
        }
    ```
    ```csharp
    public enum RankIds : byte
    {
        Unranked = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
        Diamond = 5,
        Master = 6,
        Chaos = 7
    }
    ```

### Client

1. The client is a lot easier to setup. The first step is to create a `ClientHandler` script that inherits from
   the `IClientHandler` interface. This example just shows blank information.

   ```csharp
   public class ClientHandler : IClientHandler
   {
       public ulong ClientRequestedToHost()
       {
           var serverJoinCode = 0;
           return serverJoinCode;
       }
   
       public void JoinServer(string serverJoinCode)
       {
       }

       public void Disconnected(DisconnectReason reason)
       {
       }
   }
   ```

2. Once the `ClientHandler` script is created we can create the client. The `MatchmakerClient` has four parameters with no defaults.
   - `ip`
      - `ip` is simple the target ip of the matchmaker server you wish to join. The player should not input this but the developer.. 
   - `clientHandler`
      - `clientHandler` is the script we created in the last step. 
   - `rankId`
      - `rankId` is the byte of the rank you created in the server getting started step one.
   - `playerId`
      - `playerId` is the id of the connecting player. Its intent when created was to use the players SteamId.

   ```csharp
   var client = new MatchmakerClient("127.0.0.1", new ClientHandler(), 0, 76561199083358154);
   ```
   
3. Update the client when needed. This should be done in `FixedUpdate` in unity
    ```csharp
    while (client.ShouldUpdate)
    {
        client.Update();
    }
    ```
    ```csharp
    public void FixedUpdate()
    {
        if (client.ShouldUpdate)
        {
            client.Update();
        }
    }
    ```
   
4. Full example. Can also be found in the [CopperMatchmaking.Example.Client](CopperMatchmaking.Example.Client/Program.cs) project.
    ```csharp
    var client = new MatchmakerClient("127.0.0.1", new ClientHandler(), 0, 76561199083358154);
    
    while (client.ShouldUpdate)
    {
        client.Update();
    }
    ```
