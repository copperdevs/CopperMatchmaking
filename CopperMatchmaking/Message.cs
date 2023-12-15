using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CopperMatchmaking
{
    public class Message
    {
        private enum MessageType : byte
        {
            Bool = 1,
            Char,
            Double,
            Float,
            Int,
            Long,
            Short,
            Uint,
            Ulong,
            UShort,
            String
        }

        public readonly byte Id;
        public readonly byte Type;
        public readonly byte[] Contents;

        private Message(byte id, byte type, byte[] contents)
        {
            Id = id;
            Type = type;
            Contents = contents;
        }

        public Message(IReadOnlyList<byte> rawMessage)
        {
            Id = rawMessage[0];
            Type = rawMessage[1];
            Contents = rawMessage.Skip(2).ToArray();
        }

        public byte[] CreateMessage()
        {
            var messageLength = 2 + Contents.Length;

            var message = new byte[messageLength];
            message[0] = Id;
            message[1] = Type;
            Array.Copy(Contents, 0, message, 2, Contents.Length);

            return message;
        }

        public static implicit operator byte[](Message message) => message.CreateMessage();
        public static implicit operator ArraySegment<byte>(Message message) => new ArraySegment<byte>(message.CreateMessage());

        #region Constructors

        public Message(byte id, bool content) : this(id, (byte)MessageType.Bool, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, char content) : this(id, (byte)MessageType.Char, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, double content) : this(id, (byte)MessageType.Double, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, float content) : this(id, (byte)MessageType.Float, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, int content) : this(id, (byte)MessageType.Int, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, long content) : this(id, (byte)MessageType.Long, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, short content) : this(id, (byte)MessageType.Short, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, uint content) : this(id, (byte)MessageType.Uint, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, ulong content) : this(id, (byte)MessageType.Ulong, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, ushort content) : this(id, (byte)MessageType.UShort, BitConverter.GetBytes(content))
        {
        }

        public Message(byte id, string content) : this(id, (byte)MessageType.String, Encoding.UTF8.GetBytes(content))
        {
        }

        #endregion
    }
}