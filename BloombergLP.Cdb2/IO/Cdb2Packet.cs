using System.IO;

using BloombergLP.Cdb2.ProtoBuf;

using ProtoBuf;

namespace BloombergLP.Cdb2.IO
{
    /// <summary>
    /// Low-level message passed to communicate with the COMDB2 database.
    /// </summary>
    internal class Cdb2Packet
    {
        /// <summary>
        /// Type of the message.
        /// </summary>
        internal int Type;

        /// <summary>
        /// Type of the message expressed as the request type.
        /// </summary>
        internal CDB2RequestType RequestType
        {
            get => (CDB2RequestType) Type;
            set => Type = (int) value;
        }

        /// <summary>
        /// Type of the message expressed as the response type.
        /// </summary>
        internal ResponseHeader ResponseType
        {
            get => (ResponseHeader) Type;
            set => Type = (int) value;
        }

        /// <summary>
        /// Whether the compression is used.
        /// </summary>
        internal bool IsCompressed;

        /// <summary>
        /// Payload for the packet.
        /// </summary>
        internal byte[] Payload;

        /// <summary>
        /// Attach the payload to this packet.
        /// </summary>
        /// <param name="payload">Payload.</param>
        /// <typeparam name="T">Type of the payload.</typeparam>
        public void Attach<T>(T payload)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, payload);
                Payload = stream.ToArray();
            }
        }

        /// <summary>
        /// Extract the object from the payload of this packet.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <returns>Extracted object.</returns>
        public T Extract<T>()
        {
            using (var stream = new MemoryStream(Payload))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
    }
}
