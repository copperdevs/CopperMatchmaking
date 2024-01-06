using System;
using Riptide;

namespace CopperMatchmaking.Data
{
    public class Rank : IMessageSerializable
    {
        public string DisplayName;
        public byte Id;

        public Rank(string displayName, Enum id) : this(displayName, Convert.ToByte(id))
        {
        }

        public Rank(string displayName, byte id)
        {
            DisplayName = displayName;
            Id = id;
        }

        public static implicit operator byte(Rank rank) => rank.Id;

        public void Serialize(Message message)
        {
            message.Add(DisplayName);
            message.Add(Id);
        }

        public void Deserialize(Message message)
        {
            DisplayName = message.GetString();
            Id = message.GetByte();
        }
    }
}