namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Represents a connection policy for the COMDB2 database.
    /// </summary>
    public enum Cdb2ConnectionPolicy
    {
        /// <summary>
        /// Connect to the supplied database node directly.
        /// </summary>
        Direct,

        /// <summary>
        /// Connect to the randomly chosen database node.
        /// </summary>
        Random,

        /// <summary>
        /// Connect to the database node randomly chosen from the same 'room'
        /// (i.e., data center, availability zone, etc.).
        /// </summary>
        RandomRoom,

        /// <summary>
        /// Connect to the configured database node from the same 'room'
        /// (i.e., data center, availability zone, etc.).
        /// </summary>
        Room
    }
}
