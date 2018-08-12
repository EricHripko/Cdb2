using System;
using System.IO;

namespace BloombergLP.Cdb2
{
    /// <summary>
    /// Provides a simple way to read the local COMDB2 configuration on the
    /// host.
    /// </summary>
    internal sealed class Cdb2Config
    {
        /// <summary>
        /// Path to the configuration files in non-BBENV environment.
        /// </summary>
        private const string NoBBEnvPath = "etc/cdb2/config.d";

        /// <summary>
        /// Hosts configured for the given database.
        /// </summary>
        public string[] Hosts { get; private set; }

        /// <summary>
        /// Establishes the tier for the current host.
        /// </summary>
        public string Tier { get; private set; }

        /// <summary>
        /// Specifies which 'room' (i.e., data center, availability zone, etc.)
        /// the host is in.
        /// </summary>
        public string Room { get; private set; }

        /// <summary>
        /// Driver needs to learn what port the database listens on. To do
        /// that, it talks to port multiplexer service that runs on database
        /// machines. This configures what port the service is listening on.
        /// </summary>
        public int PmuxPort { get; private set; }

        /// <summary>
        /// Allow routing via port multiplexer service.
        /// </summary>
        public bool AllowPmuxRoute { get; private set; }

        /// <summary>
        /// Sets the timeout for connecting to databases. The value is
        /// specified in milliseconds.
        /// </summary>
        public int ConnectTimeout { get; private set; }

        /// <summary>
        /// Sets the timeout on querying comdb2db for cluster information.
        /// Driver will try other available machines in turn if the cluster
        /// request fails. The value is specified in milliseconds.
        /// </summary>
        public int Comdb2DbTimeout { get; private set; }

        /// <summary>
        /// Specifies the name of the meta database that contains information
        /// about locations of other databases.
        /// </summary>
        public string Comdb2DbName { get; private set; }

        /// <summary>
        /// Configures the location of COMDB2 metadatabase via DNS. Alternative
        /// to specifying the location of in a configuration file.
        /// </summary>
        public string Comdb2DbDnsSuffix { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class with default settings.
        /// </summary>
        internal Cdb2Config()
        {
            SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the class from the stream that
        /// contains COMDB2 configuration.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="stream">Stream that contains COMDB2 configuration.</param>
        internal Cdb2Config(string databaseName, Stream stream) : this()
        {
            using (var reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)  
                {
                    // Comments
                    if (line.StartsWith("#"))
                    {
                        continue;
                    }

                    // Database hosts
                    if (line.StartsWith(databaseName))
                    {
                        Hosts = line
                            .Substring(databaseName.Length + 1)
                            .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    }

                    // COMDB2 config
                    var prefix = "comdb2_config:";
                    if (line.StartsWith(prefix))
                    {
                        var arguments = line
                            .Substring(prefix.Length + 1)
                            .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        var optionName = arguments[0].ToLower();
                        var optionValue = arguments[1];
                        switch(optionName)
                        {
                            case "default_type":
                                Tier = optionValue;
                                break;
                            case "room":
                                Room = optionValue;
                                break;
                            case "portmuxport":
                            case "pmuxport":
                                PmuxPort = int.Parse(optionValue);
                                break;
                            case "connect_timeout":
                                ConnectTimeout = int.Parse(optionValue);
                                break;
                            case "comdb2db_timeout":
                                Comdb2DbTimeout = int.Parse(optionValue);
                                break;
                            case "comdb2dbname":
                                Comdb2DbName = optionValue;
                                break;
                            case "dnssuffix":
                                Comdb2DbDnsSuffix = optionValue;
                                break;
                            case "allow_pmux_route":
                                AllowPmuxRoute = bool.Parse(optionValue);
                                break;
                        }
                    }
                } 
            }
        }

        /// <summary>
        /// Setup COMDB2 configuration defaults.
        /// </summary>
        private void SetDefaults()
        {
            Tier = string.Empty;
            Room = string.Empty;
            PmuxPort = 5105;
            AllowPmuxRoute = false;
            ConnectTimeout = 100;
            Comdb2DbTimeout = 500;
            Comdb2DbName = string.Empty;
            Comdb2DbDnsSuffix = string.Empty;
        }

        /// <summary>
        /// Identify the path to the configuration file for given environment.
        /// </summary>
        /// <param name="root">COMDB2 root for the current host.</param>
        /// <param name="databaseName">COMDB2 database name.</param>
        /// <returns>Path to the configuration file.</returns>
        internal static string GetPath(string root, string databaseName)
        {
            return Path.Combine(root, NoBBEnvPath, databaseName + ".cfg");
        }
    }
}
