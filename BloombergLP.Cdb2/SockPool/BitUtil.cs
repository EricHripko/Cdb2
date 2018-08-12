using System.IO;

namespace BloombergLP.Cdb2.SockPool
{
    /// <summary>
    /// Utility for (de)serialising sockpool protocol messages.
    /// </summary>
    internal static class BitUtil
    {
        /// <summary>
        /// Convert the message to a byte buffer.
        /// </summary>
        /// <param name="message">Message to be serialised.</param>
        /// <returns>Message serialised as a byte buffer</returns>
        internal static byte[] ToArray(IOutgoingMessage message)
        {
            using(var stream = new MemoryStream())
            using(var writer = new BinaryWriter(stream))
            {
                message.Write(writer);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Convert byte buffer to a message of specified type.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <typeparam name="T">Requested message type.</typeparam>
        /// <returns>Deserialised message of requested type.</returns>
        internal static T ToMessage<T>(byte[] buffer)
            where T: IIncomingMessage, new()
        {
            using(var stream = new MemoryStream(buffer))
            using(var writer = new BinaryReader(stream))
            {
                var message = new T();
                message.Read(writer);
                return message;
            }
        }
    }
}