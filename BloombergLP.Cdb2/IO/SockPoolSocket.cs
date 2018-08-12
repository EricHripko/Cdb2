using System;
using System.IO;

using Microsoft.Win32.SafeHandles;

namespace BloombergLP.Cdb2.IO
{
    internal class SockPoolStream : FileStream
    {
        /// <summary>
        /// Identifier for the connection that this stream belongs to.
        /// </summary>
        public readonly Cdb2ConnectionId ConnectionId;

        internal SockPoolStream(IntPtr handle, Cdb2ConnectionId connectionId)
            : base(
                new SafeFileHandle(handle, false),
                FileAccess.ReadWrite
            )
        {
            ConnectionId = connectionId;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    // Attempt to donate socket back to sockpool
                    using (var sockpool = new Cdb2SockPool())
                    {
                        sockpool.Connect();
                        sockpool.Donate(
                            ConnectionId,
                            SafeFileHandle.DangerousGetHandle()
                        );
                    }
                }
                catch
                {
                    // Failed to donate
                    try
                    {
                        // Attempt to release the handle
                        new SafeFileHandle(
                            SafeFileHandle.DangerousGetHandle(),
                            true
                        ).Dispose();
                    }
                    catch
                    {
                        // Failed to dispose
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}
