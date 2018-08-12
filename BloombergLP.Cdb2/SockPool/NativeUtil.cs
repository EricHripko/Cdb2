using System;
using System.Net.Sockets;

using Mono.Unix.Native;

namespace BloombergLP.Cdb2.SockPool
{
    /// <summary>
    /// Utility for sending and receiving UNIX messages to and receiving
    /// messages from the socket pooling service.
    /// </summary>
    internal static class NativeUtil
    {
        /// <summary>
        /// Send the message to the socket pooling service.
        /// </summary>
        /// <param name="socket">Socket.</param>
        /// <param name="message">Message.</param>
        internal static unsafe void SendMessage(
            Socket socket,
            SockPoolMessage message)
        {
            fixed (byte* bufferPtr = &message.Buffer[0])
            {
                var data = new[]
                {
                    new Iovec
                    {
                        iov_base = (IntPtr) bufferPtr,
                        iov_len = (ulong) message.Buffer.Length
                    }
                };
                var msghdr = new Msghdr
                {
                    msg_iovlen = data.Length,
                    msg_iov = data
                };

                if (message.Handle != IntPtr.Zero)
                {
                    msghdr.msg_control = new byte[Syscall.CMSG_SPACE(sizeof(int))];
                    var control = new Cmsghdr();
                    control.cmsg_len = msghdr.msg_controllen
                        = msghdr.msg_control.Length;
                    control.cmsg_level = UnixSocketProtocol.SOL_SOCKET;
                    control.cmsg_type = UnixSocketControlMessage.SCM_RIGHTS;
                    control.WriteToBuffer(msghdr, 0);

                    var handleBuffer
                        = BitConverter.GetBytes(message.Handle.ToInt32());
                    Buffer.BlockCopy(
                        handleBuffer,
                        0,
                        msghdr.msg_control,
                        (int) Syscall.CMSG_SPACE(0),
                        sizeof(int)
                    );
                }

                var rc = -1L;
                do
                {
                    rc = Syscall.sendmsg(socket.Handle.ToInt32(), msghdr, 0);
                    if (rc == -1)
                    {
                        throw new Exception(
                            "Failed to send a message to the sockpool socket" +
                            $", rc={Syscall.GetLastError()}"
                        );
                    }
                    else if (rc > 0)
                    {
                        data[0].iov_base += (int) rc;
                        data[0].iov_len -= (ulong) rc;

                        // Reach the end of the buffer
                        if (data[0].iov_len == 0)
                        {
                            break;
                        }
                    }
                }
                while(rc != 0);
            }
        }

        /// <summary>
        /// Receive the message from the socket pooling service.
        /// </summary>
        /// <param name="socket">Socket.</param>
        /// <param name="size">Size of the message.</param>
        /// <returns>Message received.</returns>
        internal static unsafe SockPoolMessage ReceiveMessage(
            Socket socket,
            int size)
        {
            var buffer = new byte[size];
            fixed (byte* bufferPtr = &buffer[0])
            {
                var data = new[]
                {
                    new Iovec
                    {
                        iov_base = (IntPtr) bufferPtr,
                        iov_len = (ulong) buffer.Length
                    }
                };
                var controlBuffer = new byte[Syscall.CMSG_SPACE(sizeof(int))];
                var message = new Msghdr
                {
                    msg_iov = data,
                    msg_iovlen = data.Length,
                    msg_control = controlBuffer,
                    msg_controllen = controlBuffer.Length
                };
                Syscall.recvmsg(socket.Handle.ToInt32(), message, 0);

                var result = new SockPoolMessage
                {
                    Buffer = buffer
                };
                // Parse the control part of the message if exists. This may
                // contain a socket handle
                if (message.msg_controllen > 0)
                {
                    var control = Cmsghdr.ReadFromBuffer(message, 0);
                    var offset = (int) Syscall.CMSG_DATA(message, 0);
                    result.Handle =
                        new IntPtr(BitConverter.ToInt32(controlBuffer, offset)
                    );
                }

                return result;
            }
        }
    }

    /// <summary>
    /// Low-level representation of a UNIX message sent to and received from
    /// the socket pooling service.
    /// </summary>
    internal class SockPoolMessage
    {
        /// <summary>
        /// Buffer.
        /// </summary>
        internal byte[] Buffer;

        /// <summary>
        /// Socket handle.
        /// This may not be present if socket has not returned any handles.
        /// </summary>
        internal IntPtr Handle = IntPtr.Zero;
    }
}
