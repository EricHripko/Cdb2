using System;
using System.Net.Sockets;

namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Represents a node in COMDB2 database cluster.
    /// </summary>
    internal class Cdb2Node : IDisposable
    {
        /// <summary>
        /// Host configured for the given node.
        /// </summary>
        internal string Host;

        /// <summary>
        /// Port that the given database listens on for the given node. Value
        /// of '-1' indicates that the port will be queried from the node. 
        /// </summary>
        internal int Port;
        
        /// <summary>
        /// Specifies which 'room' (i.e., data center, availability zone, etc.)
        /// the host is in.
        /// </summary>
        internal string Room;

        /// <summary>
        /// Client for the TCP connection open to this node.
        /// Null if not currently connected to this node.
        /// </summary>
        internal TcpClient Client;

        /// <summary>
        /// Initializes a new instance of the class with default settings.
        /// </summary>
        internal Cdb2Node()
        {
            SetDefaults();
        }

        /// <summary>
        /// Setup COMDB2 node defaults.
        /// </summary>
        private void SetDefaults()
        {
            Port = -1;
            Room = string.Empty;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client?.Dispose();
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
