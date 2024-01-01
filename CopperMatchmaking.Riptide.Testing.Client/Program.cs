
using CopperMatchmaking;

var client = new MatchmakerClient("127.0.0.1");

while (true)
{
    client.Update();
}