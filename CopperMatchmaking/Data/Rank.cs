using System;

namespace CopperMatchmaking.Data
{
    public class Rank
    {
        public readonly string DisplayName;
        public readonly byte Id;

        public Rank(string displayName, Enum id) : this(displayName, Convert.ToByte(id))
        {
        }

        public Rank(string displayName, byte id)
        {
            DisplayName = displayName;
            Id = id;
        }

        public static implicit operator byte(Rank rank) => rank.Id;
    }
}