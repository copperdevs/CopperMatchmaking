using System;
using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Info;
using CopperMatchmaking.Utility;

namespace CopperMatchmaking
{
    public static class Matchmaker
    {
        public static ThreadSafeRandom Random { get; } = new ThreadSafeRandom();

        // rankid, rank
        public static readonly Dictionary<byte, Rank> Ranks = new Dictionary<byte, Rank>();

        // rankid, clients in queue
        public static readonly Dictionary<byte, List<ConnectedClient>> ClientRanks = new Dictionary<byte, List<ConnectedClient>>();

        public const int MaxMessageSize = 16 * 1024;
        public static int MatchSize;

        public static MatchmakerServer Server;
        public static IMatchMaker MatchMaker;

        public static void Initialize(IMatchMaker matchMaker, int matchSize = 10)
        {
            if (Math.Abs(matchSize) % 2 != 0)
                return;

            MatchSize = matchSize;
            MatchMaker = matchMaker;

            RegisterRank(new Rank("Unranked", 0));
        }

        public static void RegisterRanks(params Rank[] ranks)
        {
            ranks.ToList().ForEach(RegisterRank);
        }

        public static void RegisterRank(Rank rank)
        {
            if (Ranks.ContainsKey(rank))
            {
                Log.Error("Couldn't register rank");
                return;
            }

            Log.Info($"Registering new rank - {rank.DisplayName}");

            Ranks.Add(rank, rank);
            ClientRanks.Add(rank, new List<ConnectedClient>());
        }

        public static void Start()
        {
            Server = new MatchmakerServer();
            Server.ClientConnected += (id, client) => { ClientRanks[(byte)client.RankId].Add(client); };
        }

        public static void Update()
        {
            Server?.Update();

            foreach (var rankTier in ClientRanks)
            {
                if (rankTier.Value.Count < MatchSize)
                    continue;

                Log.Info($"Enough people ({rankTier.Value.Count}) to make a match in tier {Ranks[rankTier.Key].DisplayName} ({rankTier.Key})");

                var foundClients = rankTier.Value.ToList().OrderBy(_ => Random.Next()).Take(MatchSize).ToList();
                foreach (var client in foundClients)
                {
                    rankTier.Value.Remove(client);
                }

                MatchMaker.MatchFound(foundClients);
            }
        }
    }
}