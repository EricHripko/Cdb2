using System.IO;
using System.Text;

namespace BloombergLP.Cdb2.SockPool
{
    /// <summary>
    /// Represents a hello message.
    /// </summary>
    internal class HelloMessage : IOutgoingMessage
    {
        /// <summary>
        /// Version of the protocol supported by the client.
        /// </summary>
        internal int ProtocolVersion;

        /// <summary>
        /// Id of the calling process.
        /// </summary>
        internal int ProcessId;
        
        internal int Slot;

        public void Write(BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes("SQLP"));
            writer.Write(ProtocolVersion);
            writer.Write(ProcessId);
            writer.Write(Slot);
        }
    }
}
