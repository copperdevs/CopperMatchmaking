using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CopperMatchmaking.Info;

namespace CopperMatchmaking
{
    public static class Matchmaker
    {
        // rankid, rank
        public static readonly Dictionary<byte, Rank> Ranks = new Dictionary<byte, Rank>();
        
        // rankid, clients in queue
        public static readonly Dictionary<byte, List<ConnectedClient>> ClientRanks = new Dictionary<byte, List<ConnectedClient>>();
        
        public const int MaxMessageSize = 16 * 1024;

        public static MatchmakerServer Server;

        public static int MatchSize;

        public static void Initialize(int matchSize = 10)
        {
            if (Math.Abs(matchSize) % 2 != 0)
                return;

            MatchSize = matchSize;

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
            Server.ClientConnected += (id, client) =>
            {
                ClientRanks[(byte)client.RankId].Add(client);
            };
        }

        public static void Update()
        {
            Server?.Update();

            foreach (var rankTier in ClientRanks)
            {
                // Log.Info($"Rank Tier - {rankTier.Key}");
                if (rankTier.Value.Count >= MatchSize)
                {
                    Log.Info($"Enough people ({ClientRanks.Values.Count}) to make a match in tier {Ranks[rankTier.Key].DisplayName} ({rankTier.Key})");
                }
            }
        }
    }
}