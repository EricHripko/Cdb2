using System.Data;

namespace BloombergLP.Cdb2
{
    public sealed class Cdb2Transaction : IDbTransaction
    {
        /// <summary>
        /// Command issued to start a transaction.
        /// </summary>
        private const string CommandBegin = "begin";

        public IsolationLevel IsolationLevel => throw new System.NotImplementedException();

        public IDbConnection Connection { get; private set; }

        public Cdb2Transaction(Cdb2Connection connection)
        {
            Connection = connection;
            Begin();
        }

        private void Begin()
        {
            var command = Connection.CreateCommand();
            command.CommandText = CommandBegin;
            command.Transaction = this;
            
            using (command)
            {
                command.ExecuteNonQuery();
            }
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Rollback()
        {
            throw new System.NotImplementedException();
        }
    }
}
