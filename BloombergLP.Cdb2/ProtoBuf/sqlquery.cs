// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: sqlquery.proto

#pragma warning disable CS0612, CS1591, CS3021, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace BloombergLP.Cdb2.ProtoBuf
{

    [global::ProtoBuf.ProtoContract(Name = @"CDB2_FLAG")]
    public partial class Cdb2Flag : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"option", IsRequired = true)]
        public int Option { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"value", IsRequired = true)]
        public int Value { get; set; }

    }

    [global::ProtoBuf.ProtoContract(Name = @"CDB2_SQLQUERY")]
    public partial class Cdb2Sqlquery : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"dbname", IsRequired = true)]
        public string Dbname { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"sql_query", IsRequired = true)]
        public string SqlQuery { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"flag")]
        public global::System.Collections.Generic.List<Cdb2Flag> Flags { get; } = new global::System.Collections.Generic.List<Cdb2Flag>();

        [global::ProtoBuf.ProtoMember(4, Name = @"little_endian", IsRequired = true)]
        public bool LittleEndian { get; set; }

        [global::ProtoBuf.ProtoMember(5, Name = @"bindvars")]
        public global::System.Collections.Generic.List<Bindvalue> Bindvars { get; } = new global::System.Collections.Generic.List<Bindvalue>();

        [global::ProtoBuf.ProtoMember(6, Name = @"tzname")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Tzname
        {
            get { return __pbn__Tzname ?? ""; }
            set { __pbn__Tzname = value; }
        }
        public bool ShouldSerializeTzname() => __pbn__Tzname != null;
        public void ResetTzname() => __pbn__Tzname = null;
        private string __pbn__Tzname;

        [global::ProtoBuf.ProtoMember(7, Name = @"set_flags")]
        public global::System.Collections.Generic.List<string> SetFlags { get; } = new global::System.Collections.Generic.List<string>();

        [global::ProtoBuf.ProtoMember(8, Name = @"types")]
        public int[] Types { get; set; }

        [global::ProtoBuf.ProtoMember(9, Name = @"mach_class")]
        [global::System.ComponentModel.DefaultValue(@"unknown")]
        public string MachClass
        {
            get { return __pbn__MachClass ?? @"unknown"; }
            set { __pbn__MachClass = value; }
        }
        public bool ShouldSerializeMachClass() => __pbn__MachClass != null;
        public void ResetMachClass() => __pbn__MachClass = null;
        private string __pbn__MachClass;

        [global::ProtoBuf.ProtoMember(10, Name = @"cnonce")]
        public byte[] Cnonce
        {
            get { return __pbn__Cnonce; }
            set { __pbn__Cnonce = value; }
        }
        public bool ShouldSerializeCnonce() => __pbn__Cnonce != null;
        public void ResetCnonce() => __pbn__Cnonce = null;
        private byte[] __pbn__Cnonce;

        [global::ProtoBuf.ProtoMember(11, Name = @"snapshot_info")]
        public Snapshotinfo SnapshotInfo { get; set; }

        [global::ProtoBuf.ProtoMember(12, Name = @"skip_rows")]
        public long SkipRows
        {
            get { return __pbn__SkipRows.GetValueOrDefault(); }
            set { __pbn__SkipRows = value; }
        }
        public bool ShouldSerializeSkipRows() => __pbn__SkipRows != null;
        public void ResetSkipRows() => __pbn__SkipRows = null;
        private long? __pbn__SkipRows;

        [global::ProtoBuf.ProtoMember(13, Name = @"retry")]
        [global::System.ComponentModel.DefaultValue(0)]
        public int Retry
        {
            get { return __pbn__Retry ?? 0; }
            set { __pbn__Retry = value; }
        }
        public bool ShouldSerializeRetry() => __pbn__Retry != null;
        public void ResetRetry() => __pbn__Retry = null;
        private int? __pbn__Retry;

        [global::ProtoBuf.ProtoMember(14, Name = @"features")]
        public int[] Features { get; set; }

        [global::ProtoBuf.ProtoMember(15, Name = @"client_info")]
        public Cinfo ClientInfo { get; set; }

        [global::ProtoBuf.ProtoMember(16, Name = @"context")]
        public global::System.Collections.Generic.List<string> Contexts { get; } = new global::System.Collections.Generic.List<string>();

        [global::ProtoBuf.ProtoMember(17, Name = @"req_info")]
        public Reqinfo ReqInfo { get; set; }

        [global::ProtoBuf.ProtoContract(Name = @"bindvalue")]
        public partial class Bindvalue : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"varname", IsRequired = true)]
            public string Varname { get; set; }

            [global::ProtoBuf.ProtoMember(2, Name = @"type", IsRequired = true)]
            public int Type { get; set; }

            [global::ProtoBuf.ProtoMember(3, Name = @"value", IsRequired = true)]
            public byte[] Value { get; set; }

            [global::ProtoBuf.ProtoMember(4, Name = @"isnull")]
            [global::System.ComponentModel.DefaultValue(false)]
            public bool Isnull
            {
                get { return __pbn__Isnull ?? false; }
                set { __pbn__Isnull = value; }
            }
            public bool ShouldSerializeIsnull() => __pbn__Isnull != null;
            public void ResetIsnull() => __pbn__Isnull = null;
            private bool? __pbn__Isnull;

            [global::ProtoBuf.ProtoMember(5, Name = @"index")]
            public int Index
            {
                get { return __pbn__Index.GetValueOrDefault(); }
                set { __pbn__Index = value; }
            }
            public bool ShouldSerializeIndex() => __pbn__Index != null;
            public void ResetIndex() => __pbn__Index = null;
            private int? __pbn__Index;

        }

        [global::ProtoBuf.ProtoContract(Name = @"snapshotinfo")]
        public partial class Snapshotinfo : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"file", IsRequired = true)]
            public int File { get; set; }

            [global::ProtoBuf.ProtoMember(2, Name = @"offset", IsRequired = true)]
            public int Offset { get; set; }

        }

        [global::ProtoBuf.ProtoContract(Name = @"cinfo")]
        public partial class Cinfo : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"pid", IsRequired = true)]
            public int Pid { get; set; }

            [global::ProtoBuf.ProtoMember(2, Name = @"th_id", IsRequired = true)]
            public ulong ThId { get; set; }

            [global::ProtoBuf.ProtoMember(3, Name = @"host_id", IsRequired = true)]
            public int HostId { get; set; }

            [global::ProtoBuf.ProtoMember(4, Name = @"argv0")]
            [global::System.ComponentModel.DefaultValue("")]
            public string Argv0
            {
                get { return __pbn__Argv0 ?? ""; }
                set { __pbn__Argv0 = value; }
            }
            public bool ShouldSerializeArgv0() => __pbn__Argv0 != null;
            public void ResetArgv0() => __pbn__Argv0 = null;
            private string __pbn__Argv0;

            [global::ProtoBuf.ProtoMember(5, Name = @"stack")]
            [global::System.ComponentModel.DefaultValue("")]
            public string Stack
            {
                get { return __pbn__Stack ?? ""; }
                set { __pbn__Stack = value; }
            }
            public bool ShouldSerializeStack() => __pbn__Stack != null;
            public void ResetStack() => __pbn__Stack = null;
            private string __pbn__Stack;

        }

        [global::ProtoBuf.ProtoContract(Name = @"reqinfo")]
        public partial class Reqinfo : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"timestampus", IsRequired = true)]
            public long Timestampus { get; set; }

            [global::ProtoBuf.ProtoMember(2, Name = @"num_retries", IsRequired = true)]
            public int NumRetries { get; set; }

        }

    }

    [global::ProtoBuf.ProtoContract(Name = @"CDB2_DBINFO")]
    public partial class Cdb2Dbinfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"dbname", IsRequired = true)]
        public string Dbname { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"little_endian", IsRequired = true)]
        public bool LittleEndian { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"want_effects")]
        public bool WantEffects
        {
            get { return __pbn__WantEffects.GetValueOrDefault(); }
            set { __pbn__WantEffects = value; }
        }
        public bool ShouldSerializeWantEffects() => __pbn__WantEffects != null;
        public void ResetWantEffects() => __pbn__WantEffects = null;
        private bool? __pbn__WantEffects;

    }

    [global::ProtoBuf.ProtoContract(Name = @"CDB2_QUERY")]
    public partial class Cdb2Query : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"sqlquery")]
        public Cdb2Sqlquery Sqlquery { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"dbinfo")]
        public Cdb2Dbinfo Dbinfo { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"spcmd")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Spcmd
        {
            get { return __pbn__Spcmd ?? ""; }
            set { __pbn__Spcmd = value; }
        }
        public bool ShouldSerializeSpcmd() => __pbn__Spcmd != null;
        public void ResetSpcmd() => __pbn__Spcmd = null;
        private string __pbn__Spcmd;

    }

    [global::ProtoBuf.ProtoContract()]
    public enum CDB2RequestType
    {
        [global::ProtoBuf.ProtoEnum(Name = @"CDB2QUERY")]
        Cdb2query = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"SQLQUERY")]
        Sqlquery = 2,
        [global::ProtoBuf.ProtoEnum(Name = @"DBINFO")]
        Dbinfo = 3,
        [global::ProtoBuf.ProtoEnum(Name = @"RESET")]
        Reset = 108,
        [global::ProtoBuf.ProtoEnum(Name = @"SSLCONN")]
        Sslconn = 121,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum CDB2ClientFeatures
    {
        [global::ProtoBuf.ProtoEnum(Name = @"SKIP_INTRANS_RESULTS")]
        SkipIntransResults = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"ALLOW_MASTER_EXEC")]
        AllowMasterExec = 2,
        [global::ProtoBuf.ProtoEnum(Name = @"ALLOW_MASTER_DBINFO")]
        AllowMasterDbinfo = 3,
        [global::ProtoBuf.ProtoEnum(Name = @"ALLOW_QUEUING")]
        AllowQueuing = 4,
        [global::ProtoBuf.ProtoEnum(Name = @"SSL")]
        Ssl = 5,
    }

}

#pragma warning restore CS0612, CS1591, CS3021, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192