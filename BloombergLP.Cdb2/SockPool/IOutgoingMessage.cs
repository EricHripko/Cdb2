using System.IO;

namespace BloombergLP.Cdb2.SockPool
{
    /// <summary>
    /// Interface for an outgoing socket pool message.
    /// </summary>
    internal interface IOutgoingMessage 
    {
        /// <summary>
        /// Serialise the message with the given binary writer.
        /// </summary>
        /// <param name="writer">Writer to be used.</param>
        void Write(BinaryWriter writer);
    }
}
