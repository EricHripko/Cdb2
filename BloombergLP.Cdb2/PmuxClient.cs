using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("BloombergLP.Cdb2.Tests")]

namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Client for the port multiplexer service.
    /// </summary>
    internal sealed class PmuxClient : IDisposable
    {
        /// <summary>
        /// Default port for port multiplexer service.
        /// </summary>
        internal const int DefaultPort = 5105;

        /// <summary>
        /// Default timeout for the operations.
        /// Specified in milliseconds.
        /// </summary>
        internal const int DefaultTimeout = 100;
        
        /// <summary>
        /// Command used to identify the port.
        /// </summary>
        internal const string Command = "get {0}\n";

        /// <summary>
        /// Host name for the port multiplexer service.
        /// </summary>
        private string _hostName;

        /// <summary>
        /// TCP client for interacting with port multiplexer service.
        /// </summary>
        private TcpClient _tcp;

        /// <summary>
        /// Create a new client for port multiplexer service.
        /// </summary>
        /// <param name="hostName">Host name for the pmux service.</param>
        internal PmuxClient(string hostName)
        {
            _hostName = hostName;
            _tcp = new TcpClient();
            _tcp.SendTimeout = _tcp.ReceiveTimeout = DefaultTimeout;
        }

        /// <summary>
        /// Connect to the port multiplexer service.
        /// </summary>
        internal void Connect()
        {
            _tcp.Connect(_hostName, DefaultPort);
        }

        /// <summary>
        /// Retrieve the port for a given service instance.
        /// </summary>
        /// <param name="app">Application identifier.</param>
        /// <param name="service">Service identifier.</param>
        /// <param name="instance">Instance identifier.</param>
        /// <returns>Port.</returns>
        internal int GetPort(string app, string service, string instance)
        {
            // Prepare
            var name = string.Join("/", app, service, instance);
            var commandText = string.Format(Command, name);
            commandText += '\0';

            // Send
            var stream = _tcp.GetStream();
            var command = Encoding.ASCII.GetBytes(commandText);
            stream.Write(command, 0, command.Length);

            // Receive
            var response = new byte[32];
            stream.Read(response, 0, response.Length);
            var responseText = Encoding.ASCII.GetString(response);

            // Process
            var port = int.Parse(responseText);
            if (port <= 0)
            {
                port = -1;
            }

            return port;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tcp.Dispose();
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
