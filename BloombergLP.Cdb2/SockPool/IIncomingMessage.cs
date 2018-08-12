using System.IO;

namespace BloombergLP.Cdb2.SockPool
{
    /// <summary>
    /// Interface for an incoming socket pool message.
    /// </summary>
    internal interface IIncomingMessage 
    {
        /// <summary>
        /// Deserialise the message with the given binary reader.
        /// </summary>
        /// <param name="reader">Reader to be used.</param>
        void Read(BinaryReader reader);
    }
}
