using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CopperMatchmaking.Info;

namespace CopperMatchmaking
{
    public static class Matchmaker
    {
        public static readonly Dictionary<byte, Rank> Ranks = new Dictionary<byte, Rank>();
        public const int MaxMessageSize = 16 * 1024;

        public static MatchmakerServer Server;
    
        public static void Initialize()
        {
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
            
            Ranks.Add(rank, rank);
        }

        public static void Start()
        {
            Server = new MatchmakerServer();
        }
    
        public static void Update()
        {
            Server?.Update();
        }
    }
}