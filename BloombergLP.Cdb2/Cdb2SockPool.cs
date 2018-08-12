using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

using BloombergLP.Cdb2.SockPool;

using Mono.Unix;
using Mono.Unix.Native;

namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Provides a simple way to read the interact with COMDB2 socket pool
    /// service. This class cannot be inherited.
    /// </summary>
    public sealed class Cdb2SockPool : IDisposable
    {
        /// <summary>
        /// Path to the sockpool socket.
        /// </summary>
        private static readonly EndPoint SockPoolEndPoint
            = new UnixEndPoint("/tmp/sockpool.socket");

        /// <summary>
        /// Default timeout for the donated socket.
        /// </summary>
        private static readonly TimeSpan DefaultTimeout
            = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Socket used to communicate with COMDB2 socket pool service.
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// Create a new client for COMDB2 socket pool service.
        /// </summary>
        public Cdb2SockPool()
        {
            _socket = new Socket(
                AddressFamily.Unix,
                SocketType.Stream,
                ProtocolType.Unspecified
            );
        }

        /// <summary>
        /// Connect to the socket pool service.
        /// </summary>
        public void Connect()
        {
            _socket.Connect(SockPoolEndPoint);

            var hello = new HelloMessage
            {
                ProtocolVersion = 0,
                ProcessId = Process.GetCurrentProcess().Id,
                Slot = 0
            };
            Send(BitUtil.ToArray(hello));
        }

        public IntPtr Request(Cdb2ConnectionId connectionId)
        {
            var message = new V0Message
            {
                Type = V0MessageType.Request,
                TypeString = connectionId.ToString()
            };
            var wrapper = new SockPoolMessage
            {
                Buffer = BitUtil.ToArray(message)
            };
            NativeUtil.SendMessage(_socket, wrapper);

            wrapper = NativeUtil.ReceiveMessage(_socket, 60);
            message = BitUtil.ToMessage<V0Message>(wrapper.Buffer);
            return wrapper.Handle;
        }

        public void Donate(Cdb2ConnectionId connectionId, IntPtr handle)
        {
            var message = new V0Message
            {
                Type = V0MessageType.Donate,
                TypeString = connectionId.ToString(),
                Timeout = DefaultTimeout
            };
            var wrapper = new SockPoolMessage
            {
                Buffer = BitUtil.ToArray(message),
                Handle = handle
            };

            NativeUtil.SendMessage(_socket, wrapper);
        }

        /// <summary>
        /// Write the supplied buffer to the service. 
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        private void Send(byte[] buffer)
        {
            var offset = 0;
            do
            {
                var bytes = _socket.Send(
                    buffer,
                    offset,
                    buffer.Length - offset,
                    SocketFlags.None
                );
                offset += bytes;
            }
            while(offset != buffer.Length);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _socket.Dispose();
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
