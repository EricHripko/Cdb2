using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using BloombergLP.Cdb2.ProtoBuf;

namespace BloombergLP.Cdb2
{
    public sealed class Cdb2DataReader : IDataReader
    {
        /// <summary>
        /// Connection associated with this data reader.
        /// </summary>
        public Cdb2Connection Connection { get; private set; }

        /// <summary>
        /// Columns in the result being currently processed by this data
        /// reader.
        /// </summary>
        private Cdb2Column[] _columns;

        /// <summary>
        /// Values of the row being currently held in this data reader.
        /// </summary>
        private byte[][] _values;

        public int Depth => 0; // Nesting is not supported

        public bool IsClosed { get; private set; }

        public int RecordsAffected => throw new NotImplementedException();

        public int FieldCount => _columns.Length;

        public object this[int i] => throw new NotImplementedException();

        public object this[string name] => throw new NotImplementedException();

        internal Cdb2DataReader(Cdb2Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Throws an exception if there is no row present in the reader.
        /// </summary>
        private void EnsureRowAvailable()
        {
            if (_values == null)
            {
                throw new InvalidOperationException(
                    "No row is available to process, call Read() first"
                );
            }
        }

        /// <summary>
        /// Throws an exception if column is not compatible with the requested
        /// type or if there is no row present in the reader.
        /// </summary>
        /// <param name="i">Column index.</param>
        /// <param name="expectedType">Expected column type.</param>
        /// <returns>Column value as a byte buffer.</returns>
        private byte[] EnsureColumnCompatible(
            int i,
            CDB2ColumnType expectedType)
        {
            EnsureRowAvailable();
            var column = _columns[i];
            if (column.Type != expectedType)
            {
                throw new InvalidCastException(
                    $"Cannot process column #{i} due to type mismatch, " +
                    $"expected: {expectedType}, " +
                    $"actual: {column.Type}"
                );
            }

            return _values[i];
        }

        public string GetName(int i)
        {
            if (_columns == null)
            {
                throw new InvalidOperationException(
                    "Columns are not loaded yet, call Read() first"
                );
            }

            return _columns[i].Name;
        }

        public int GetOrdinal(string name)
        {
            for (var i = 0; i < _columns.Length; i++)
            {
                if (_columns[i].Name.Equals(
                        name, 
                        StringComparison.CurrentCultureIgnoreCase
                    ))
                {
                    return i;
                }
            }

            throw new IndexOutOfRangeException(
                $"Column '{name}' was not found in the current result"
            );
        }

        public IDataReader GetData(int i)
        {
            // This is typically used to expose nested tables and other
            // hierarchical data. However, COMDB2 doesn't allow complex data
            // structure
            throw new NotSupportedException(
                $"{nameof(GetData)} is not supported"
            );
        }

        public string GetDataTypeName(int i)
        {
            return _columns[i].Type.ToString().ToLower();
        }

        public Type GetFieldType(int i)
        {
            switch (_columns[i].Type)
            {
                case CDB2ColumnType.Blob:
                    return typeof(byte[]);
                
                case CDB2ColumnType.Cstring:
                    return typeof(string);
                
                case CDB2ColumnType.Datetime:
                case CDB2ColumnType.Datetimeus:
                    return typeof(DateTimeOffset);
                
                case CDB2ColumnType.Integer:
                    return typeof(long);
                
                case CDB2ColumnType.Intervalds:
                case CDB2ColumnType.Intervaldsus:
                case CDB2ColumnType.Intervalym:
                    return typeof(TimeSpan);
                
                case CDB2ColumnType.Real:
                    return typeof(double);
            }

            throw new NotSupportedException(
                $"Column #{i} is of type {_columns[i].Type}, which is not " +
                "yet supported"
            );
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            var value = EnsureColumnCompatible(i, CDB2ColumnType.Cstring);
            return Encoding.UTF8.GetString(value).TrimEnd('\0');
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        #region Floating-point types
        public float GetFloat(int i)
        {
            return (float) GetDouble(i);
        }

        public double GetDouble(int i)
        {
            var value = EnsureColumnCompatible(i, CDB2ColumnType.Real);
            return BitConverter.ToDouble(value, 0);
        }

        public decimal GetDecimal(int i)
        {
            return (decimal) GetDouble(i);
        }
        #endregion

        #region Integral types
        public bool GetBoolean(int i)
        {
            return GetInt64(i) != 0;
        }

        public byte GetByte(int i)
        {
            var value = GetInt64(i);

            return checked((byte) value);
        }

        public short GetInt16(int i)
        {
            var value = GetInt64(i);

            return checked((short) value);
        }

        public int GetInt32(int i)
        {
            var value = GetInt64(i);

            return checked((int) value);
        }

        public long GetInt64(int i)
        {
            var value = EnsureColumnCompatible(i, CDB2ColumnType.Integer);
            return BitConverter.ToInt64(value, 0);
        }
        #endregion

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public bool Read()
        {
            var packet = Connection.Receive();
            if (packet.ResponseType != ResponseHeader.SqlResponse)
            {
                throw new InvalidOperationException($"Unexpected response of type {packet.ResponseType}");
            }

            var response = packet.Extract<Cdb2Sqlresponse>();
            if (response.ErrorCode != CDB2ErrorCode.Ok)
            {
                throw new Cdb2Exception(
                    response.ErrorString,
                    response.ErrorCode
                );
            }

            // Reached the end of result
            if (response.ResponseType == ResponseType.LastRow)
            {
                return false;
            }

            // Read column names
            if (response.ResponseType == ResponseType.ColumnNames)
            {
                _columns = new Cdb2Column[response.Values.Count];
                for (var i = 0; i < _columns.Length; i++)
                {
                    var column = new Cdb2Column();
                    column.Name = Encoding.ASCII
                        .GetString(response.Values[i].Value)
                        .TrimEnd('\0');
                    column.Type = response.Values[i].Type;

                    _columns[i] = column;
                }

                // Processed column names, now go to the first record
                return Read();
            }

            // Read column values
            if (response.ResponseType == ResponseType.ColumnValues)
            {
                _values = response.Values.Select(x => x.Value).ToArray();
            }

            return true;
        }

        public bool NextResult()
        {
            return false;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Utility object for storing COMDB2 column information.
        /// </summary>
        private class Cdb2Column
        {
            /// <summary>
            /// Name of the column.
            /// </summary>
            internal string Name;

            /// <summary>
            /// Type of the column.
            /// </summary>
            internal CDB2ColumnType Type;
        }
    }
}
