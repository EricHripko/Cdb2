using System.Data.Common;

using BloombergLP.Cdb2.ProtoBuf;

namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Exception that indicates a fail result returned by the COMDB2 database.
    /// </summary>
    public sealed class Cdb2Exception : DbException
    {
        /// <summary>
        /// Error returned by the COMDB2 database.
        /// </summary>
        public CDB2ErrorCode Error
        {
            get => (CDB2ErrorCode) HResult;
            private set => HResult = (int) value;
        }

        /// <summary>
        /// Initializes a new instance of the class with the specified error
        /// message and error code.
        /// </summary>
        /// <param name="message">Error message explaining the reason.</param>
        /// <param name="errorCode">The error code for the exception.</param>
        internal Cdb2Exception(string message, CDB2ErrorCode errorCode)
            : base(message)
        {
            Error = errorCode;
        }
    }
}
