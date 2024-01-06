- [X] clients connects
- [X] client tell server their ranks and player id (steam id for example)
- [ ] server puts them into a `Queue` based off of rank
- [ ] when enough players are in a `Queue`, the server takes a certain amount of players and puts them in a `lobby list`.
  once in a lobby list the players are removed from their bin
- [ ] once a `lobby list` is created is runs an interface function in `IServerHandler` to decide the teams
- [ ] the server asks a client to host, if they can its returns the join code, if not it returns null
- [ ] once the host returns a join code all the rest of the clients in the `lobby list` join said game
- [ ] once all players are in game then yay we good delete that lobby