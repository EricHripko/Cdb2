using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using BloombergLP.Cdb2.IO;
using BloombergLP.Cdb2.ProtoBuf;

namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Represents a connection to the COMDB2 database.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class Cdb2Connection : IDbConnection
    {
        /// <summary>
        /// Tier that represents a single local database.
        /// </summary>
        private const string TierLocal = "local";

        /// <summary>
        /// Host name for the local machine.
        /// </summary>
        private const string HostLocal = "localhost";

        /// <summary>
        /// Application identifier for COMDB2.
        /// </summary>
        private const string AppId = "comdb2";

        /// <summary>
        /// Service identifier for COMDB2 replication.
        /// </summary>
        private const string ServiceIdReplication = "replication";

        /// <summary>
        /// Command used to initialise a new SQL connection.
        /// </summary>
        private static readonly byte[] CommandInit
            = Encoding.ASCII.GetBytes("newsql\n");
        
        /// <summary>
        /// COMDB2 connection string.
        /// </summary>
        private Cdb2ConnectionStringBuilder _connectionString = new Cdb2ConnectionStringBuilder();

        /// <summary>
        /// Nodes discovered for the given database.
        /// </summary>
        private List<Cdb2Node> _nodes;

        /// <summary>
        /// Stream used for this connection.
        /// </summary>
        private Stream _stream;

        /// <summary>
        /// Whether the connection has sent the client information to the
        /// database or not.
        /// </summary>
        internal bool SentClientInfo;

        public string ConnectionString {
            get => _connectionString.ConnectionString;
            set
            {
                _connectionString.ConnectionString = value;
            }
        }

        public int ConnectionTimeout => throw new System.NotImplementedException();

        public string Database => _connectionString.DatabaseName;

        public ConnectionState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Cdb2Connection()
        {}

        /// <summary>
        /// Initializes a new instance of the class when given a string that
        /// contains the connection string.
        /// </summary>
        /// <param name="connectionString">The connection used to open the COMDB2 database.</param>
        public Cdb2Connection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbTransaction BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            _stream?.Close();
        }

        public IDbCommand CreateCommand()
        {
            throw new System.NotImplementedException();
        }

        private void DiscoverNodes()
        {
            if (_connectionString.Tier.Equals(
                    TierLocal,
                    StringComparison.InvariantCultureIgnoreCase
                ))
            {
                var nodeLocal = new Cdb2Node { Host = HostLocal };
                using (var pmux = new PmuxClient(HostLocal))
                {
                    pmux.Connect();
                    nodeLocal.Port = pmux.GetPort(
                        AppId,
                        ServiceIdReplication,
                        _connectionString.DatabaseName
                    );
                }

                _nodes = new List<Cdb2Node> { nodeLocal };
                _connectionString.Policy = Cdb2ConnectionPolicy.Direct;
                return;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Send the packet using the given stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="packet">Packet.</param>
        private void Send(Stream stream, Cdb2Packet packet)
        {
            using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                writer.Write(
                    IPAddress.NetworkToHostOrder(packet.Type));
                writer.Write(
                    IPAddress.NetworkToHostOrder(packet.IsCompressed ? 1 : 0));
                writer.Write(0);
                writer.Write(
                    IPAddress.NetworkToHostOrder(packet.Payload.Length));
                writer.Write(packet.Payload);
            }
        }

        /// <summary>
        /// Send the packet to this COMDB2 connection.
        /// </summary>
        /// <param name="packet">Packet to be sent.</param>
        internal void Send(Cdb2Packet packet)
        {
            Send(_stream, packet);
        }

        /// <summary>
        /// Receive a packet from this COMDB2 connection.
        /// </summary>
        /// <returns>Packet received.</returns>
        internal Cdb2Packet Receive()
        {
            var packet = new Cdb2Packet();
            using (var reader = new BinaryReader(_stream, Encoding.ASCII, true))
            {
                packet.Type =
                    IPAddress.HostToNetworkOrder(reader.ReadInt32());
                packet.IsCompressed =
                    IPAddress.HostToNetworkOrder(reader.ReadInt32()) == 1;
                reader.ReadInt32();
                var payloadLength =
                    IPAddress.HostToNetworkOrder(reader.ReadInt32());
                packet.Payload = reader.ReadBytes(payloadLength);
            }

            return packet;
        }

        private Stream OpenSockPool(Cdb2ConnectionId id)
        {
            var handle = IntPtr.Zero;
            using (var sockpool = new Cdb2SockPool())
            {
                sockpool.Connect();
                handle = sockpool.Request(id);
            }

            if (handle == IntPtr.Zero)
            {
                return null;
            }

            var stream = new SockPoolStream(handle, id);
            var packet = new Cdb2Packet
            {
                RequestType = CDB2RequestType.Reset,
                IsCompressed = false,
                Payload = new byte[0]
            };
            Send(stream, packet);

            return stream;
        }

        private Stream OpenNode(Cdb2Node node)
        {
            node.Client = new TcpClient(node.Host, node.Port);
            //TODO Set node.Client.ReceiveBufferSize according to config
            var stream = node.Client.GetStream();
            stream.Write(CommandInit, 0, CommandInit.Length);
            
            return stream;
        }

        public void Open()
        {
            if (!_connectionString.ContainsKey("Database Name") || string.IsNullOrWhiteSpace(_connectionString.DatabaseName))
            {
                throw new InvalidOperationException("Database Name needs is required to open a connection");
            }
            if (!_connectionString.ContainsKey("Tier") || string.IsNullOrWhiteSpace(_connectionString.Tier))
            {
                throw new InvalidOperationException("Tier is required to open a connection");
            }

            if (!_connectionString.ContainsKey("Policy"))
            {
                _connectionString.Policy = Cdb2ConnectionPolicy.RandomRoom;
            }

            var id = new Cdb2ConnectionId(
                _connectionString.DatabaseName,
                _connectionString.Tier,
                _connectionString.Policy
            );
            
            // First, attempt a sockpool connection
            var stream = OpenSockPool(id);
            if (stream != null)
            {
                _stream = stream;
                return;
            }

            // Then, connect directly
            DiscoverNodes();
            switch (_connectionString.Policy)
            {
                default:
                    throw new NotImplementedException($"{_connectionString.Policy} is not supported yet");
                case Cdb2ConnectionPolicy.Direct:
                    _stream = OpenNode(_nodes.First());
                    break;
            }

            // First query to the connection will need to include client
            // information
            SentClientInfo = false;
            State = ConnectionState.Open;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stream?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
