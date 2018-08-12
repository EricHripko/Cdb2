using System;
using System.IO;
using System.Text;

namespace BloombergLP.Cdb2.SockPool
{
    /// <summary>
    /// Represents the universal message used in sockpool protocol version 0.
    /// </summary>
    internal class V0Message : IOutgoingMessage, IIncomingMessage
    {
        /// <summary>
        /// Size of the message.
        /// </summary>
        private const int Size = 60;

        /// <summary>
        /// Maximum size of type string.
        /// </summary>
        private const int TypeSize = 48;

        /// <summary>
        /// Type of the message.
        /// </summary>
        internal V0MessageType Type;

        /// <summary>
        /// Port allocated for the attached database.
        /// </summary>
        internal short Port;

        /// <summary>
        /// Database number for the attached database.
        /// </summary>
        public int DatabaseNumber;

        public TimeSpan Timeout;

        /// <summary>
        /// Type string describing the connection to the attached database.
        /// </summary>
        public string TypeString;

        public void Write(BinaryWriter writer)
        {
            writer.Write((byte) Type);
            writer.Write((byte) 0);
            writer.Write(Port);
            writer.Write(DatabaseNumber);
            writer.Write((int) Timeout.TotalMilliseconds);
            writer.Write(Encoding.ASCII.GetBytes(TypeString));

            writer.Seek(59, SeekOrigin.Begin);
            writer.Write((byte) 0);
        }

        public void Read(BinaryReader reader)
        {
            Type = (V0MessageType) reader.ReadByte();
            reader.BaseStream.Seek(1, SeekOrigin.Current);
            Port = reader.ReadInt16();
            DatabaseNumber = reader.ReadInt32();
            Timeout = TimeSpan.FromMilliseconds(reader.ReadInt32());
            var buffer = reader.ReadBytes(TypeSize);
            TypeString = Encoding.ASCII.GetString(buffer);
        }
    }

    /// <summary>
    /// Type of the sockpool message in protocol version 0. 
    /// </summary>
    internal enum V0MessageType : byte
    {
        /// <summary>
        /// Donate a socket to the pool.
        /// </summary>
        Donate = 0,

        /// <summary>
        /// Request a socket from the pool.
        /// </summary>
        Request = 1
    }
}
