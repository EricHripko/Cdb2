using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Provides a simple way to create and manage the contents of connection
    /// strings used by the <see cref="Cdb2Connection"></see> class.
    /// </summary>
    public sealed class Cdb2ConnectionStringBuilder : DbConnectionStringBuilder
    {
        /// <summary>
        /// Gets or sets the name of the database associated with the
        /// connection.
        /// </summary>
        public string DatabaseName
        {
            get => this["Database Name"].ToString();
            set => this["Database Name"] = value;
        }

        /// <summary>
        /// Gets or sets the tier of the database associated with the
        /// connection.
        /// </summary>
        public string Tier
        {
            get => this[nameof(Tier)].ToString();
            set => this[nameof(Tier)] = value;
        }

        /// <summary>
        /// Gets or sets the connection policy for the database associated with
        /// the connection.
        /// </summary>
        public Cdb2ConnectionPolicy Policy
        {
            get => (Cdb2ConnectionPolicy) Enum.Parse(typeof(Cdb2ConnectionPolicy), this[nameof(Policy)].ToString());
            set => this[nameof(Policy)] = value.ToString();
        }

        /// <summary>
        /// Gets or sets the path to the COMDB2 root.
        /// </summary>
        public string Root
        {
            get => this[nameof(Root)].ToString();
            set => this[nameof(Root)] = value;
        }

        //public ReadOnlyCollection<Cdb2Node> Nodes
        //{
        //    get => this["DataSource"].ToString();
        //    set => this["DataSource"] = value;
        //}

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Cdb2ConnectionStringBuilder()
        {}

        /// <summary>
        /// Initializes a new instance of the class. The supplied connection
        /// string provides the data for the instance's internal connection
        /// information.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        public Cdb2ConnectionStringBuilder(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private ReadOnlyCollection<Cdb2Node> ParseNodes(string source)
        {
            var nodes = new List<Cdb2Node>();
            var definitions = source.Split(',');

            return new ReadOnlyCollection<Cdb2Node>(nodes);
        }

    }
}
