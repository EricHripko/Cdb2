using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using BloombergLP.Cdb2.IO;
using BloombergLP.Cdb2.ProtoBuf;

namespace BloombergLP.Cdb2
{
    public sealed class Cdb2Command : IDbCommand
    {
        /// <summary>
        /// Default timezone used by the driver.
        /// </summary>
        private const string DefaultTimeZone = "America/New_York";

        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }

        private Cdb2Connection _connection;
        public IDbConnection Connection
        {
            get => _connection;
            set => _connection = (Cdb2Connection) value;
        }

        public IDataParameterCollection Parameters { get; private set; }

        public IDbTransaction Transaction { get; set; }
        public UpdateRowSource UpdatedRowSource { get; set; }

        /// <summary>
        /// Whether this command is safe or not.
        /// Unsafe commands are only issued by the driver itself and cannot
        /// originate from the user.
        /// </summary>
        internal bool IsSafe { get; set; }

        public Cdb2Command() : this(true)
        { }

        public Cdb2Command(string cmdText) : this()
        {
            CommandText = cmdText;
        }

        public Cdb2Command(string cmdText, Cdb2Connection connection)
            : this(cmdText)
        {
            Connection = connection;
        }

        public Cdb2Command(string cmdText, Cdb2Connection connection, Cdb2Transaction transaction)
            : this(cmdText, connection)
        {
            Transaction = transaction;
        }

        internal Cdb2Command(bool isSafe)
        {
            Parameters = new Cdb2ParameterCollection();
        }

        public void Cancel()
        {
            throw new System.NotImplementedException();
        }

        public IDbDataParameter CreateParameter()
        {
            throw new System.NotImplementedException();
        }

        private void ExecuteImpl()
        {
            if (Connection == null || Connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection must valid and open");
            }

            var sqlquery = new Cdb2Sqlquery();
            sqlquery.Dbname = Connection.Database;
            sqlquery.SqlQuery = CommandText;
            sqlquery.LittleEndian = true;
            sqlquery.Types = new int[0];
            sqlquery.Tzname = DefaultTimeZone;
            sqlquery.MachClass = "local";

            if (!_connection.SentClientInfo)
            {
                sqlquery.ClientInfo = new Cdb2Sqlquery.Cinfo
                {
                    Pid = Process.GetCurrentProcess().Id,
                    ThId = (ulong) Thread.CurrentThread.ManagedThreadId,
                    Argv0 = Assembly.GetExecutingAssembly().GetName().Name
                };
                _connection.SentClientInfo = true;
            }

            var packet = new Cdb2Packet
            {
                RequestType = CDB2RequestType.Cdb2query,
                IsCompressed = false
            };
            packet.Attach(new Cdb2Query { Sqlquery = sqlquery });
            _connection.Send(packet);
        }

        public int ExecuteNonQuery()
        {
            ExecuteImpl();
            return 0;
        }

        public IDataReader ExecuteReader()
        {
            ExecuteImpl();

            return new Cdb2DataReader(_connection);
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new System.NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new System.NotImplementedException();
        }

        public void Prepare()
        {
            throw new System.NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Cdb2Command() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
