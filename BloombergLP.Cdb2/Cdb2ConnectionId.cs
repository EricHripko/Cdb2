namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Provides an identifier for the connection to the COMDB2 database.
    /// </summary>
    public struct Cdb2ConnectionId
    {
        /// <summary>
        /// Name of the database associated with the connection.
        /// </summary>
        public readonly string DatabaseName;

        /// <summary>
        /// Tier of the database associated with the connection.
        /// </summary>
        public readonly string Tier;

        /// <summary>
        /// Connection policy for the database connections.
        /// </summary>
        public readonly Cdb2ConnectionPolicy Policy;

        /// <summary>
        /// Initializes a new connection identifier.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="tier">Tier of the database.</param>
        /// <param name="policy">Database policy.</param>
        public Cdb2ConnectionId(
            string databaseName,
            string tier,
            Cdb2ConnectionPolicy policy)
        {
            DatabaseName = databaseName;
            Tier = tier;
            Policy = policy;
        }

        public override string ToString()
        {
            var policy = string.Empty;
            switch (Policy)
            {
                case Cdb2ConnectionPolicy.Direct:
                    policy = "direct";
                    break;

                case Cdb2ConnectionPolicy.Random:
                    policy = "random";
                    break;

                case Cdb2ConnectionPolicy.RandomRoom:
                    policy = "random_room";
                    break;

                case Cdb2ConnectionPolicy.Room:
                    policy = "room";
                    break;
            }

            return string.Join("/",
                "comdb2",
                DatabaseName,
                Tier,
                "newsql",
                policy
            );
        }
    }


}