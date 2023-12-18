using System;

namespace CopperMatchmaking.Utility
{
    public class ThreadSafeRandom
    {
        private static readonly Random Global = new Random();
        [ThreadStatic] private static Random local;

        public int Next()
        {
            if (local != null) 
                return local.Next();
            
            int seed;
            lock (Global)
            {
                seed = Global.Next();
            }

            local = new Random(seed);

            return local.Next();
        }
    }
}