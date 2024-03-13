using System;
using Riptide;

namespace CopperMatchmaking.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Rank : IMessageSerializable
    {
        /// <summary>
        /// Display name of the rank for visual sakes
        /// </summary>
        public string DisplayName;
        
        /// <summary>
        /// Rank id of a client
        /// </summary>
        public byte Id;

        /// <summary>
        /// dont use this stupid
        /// </summary>
        public Rank()
        {
            
        }

        /// <summary>
        /// Creates a new rank id with an enum
        /// </summary>
        /// <param name="displayName">Display name of the rank</param>
        /// <param name="id">Enum id of the rank (Case to byte)</param>
        public Rank(string displayName, Enum id) : this(displayName, Convert.ToByte(id))
        {
        }

        /// <summary>
        /// Creates a new rank id with a byte
        /// </summary>
        /// <param name="displayName">Display name of the rank</param>
        /// <param name="id">Byte id of the rank</param>
        public Rank(string displayName, byte id)
        {
            DisplayName = displayName;
            Id = id;
        }

        /// <summary>
        /// Gets the id of a rank
        /// </summary>
        /// <param name="rank">Target Rank</param>
        /// <returns>Byte id of the rank</returns>
        public static implicit operator byte(Rank rank) => rank.Id;

        /// <summary>
        /// Serializes the rank for riptide purposes
        /// </summary>
        /// <param name="message">Target message</param>
        public void Serialize(Message message)
        {
            message.Add(DisplayName);
            message.Add(Id);
        }

        /// <summary>
        /// Deserializes the rank for riptide purposes
        /// </summary>
        /// <param name="message">Target message</param>
        public void Deserialize(Message message)
        {
            DisplayName = message.GetString();
            Id = message.GetByte();
        }
    }
}