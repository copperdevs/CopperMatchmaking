# CopperMatchmaking

> Simple matchmaking server for creating two even player count teams for use in peer to peer

## Use Case

`CopperMatchmaking` was made for games with two teams, each with the same amount of players (5 for example), and for one
of those players to be the host in a peer to peer architecture. It also has ranks built in for pooling players.

## ToDo

- [ ] Give `IServerHandler` a use.
    - Its intent is to have it make the lobbies from each pool of players, just unsure of how to add it.

## Getting Started

### Server

1. Create ranks. The matchmaker creates a pool of players for each rank. Each connected client can only be matchmaked
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

2. Create the server. The constructor for `MatchmakerServer` has two paramters both with defaults.
    - `lobbySize`
        - `lobbySize` determines the amount of players in a lobby. This has to be an even number so both teams can have
          the same amount of players. The default is set to 10.
    - `maxClients`
        - `maxClients` determines the max amount of clients that can be connnected to the matchmaker server. The default
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

4. Now you just have to consistenly update the server. This can be done in a `while(true)` loop or in unitys built
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

5. Full example. Can also be found in the `CopperMatchmaking.Example.Server` project.
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