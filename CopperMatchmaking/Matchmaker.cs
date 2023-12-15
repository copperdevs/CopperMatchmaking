using System.Diagnostics;

namespace CopperMatchmaking;

public static class Matchmaker
{
    public static readonly Dictionary<byte, Rank> Ranks = new();
    
    public static void Initialize()
    {
        RegisterRank(new Rank("Unranked", 0));
    }
    
    public static void Update()
    {
        
    }

    public static void RegisterRanks(params Rank[] ranks)
    {
        ranks.ToList().ForEach(RegisterRank);
    }
    
    public static void RegisterRank(Rank rank)
    {
        if (Ranks.TryAdd(rank, rank))
            Log.Info($"Registered new rank - {rank.DisplayName}");
        else
            Log.Error($"Couldn't register rank");
    }
}