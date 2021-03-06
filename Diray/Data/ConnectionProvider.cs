using System;
using System.Data;
using System.Data.SQLite;

namespace Diray.Data
{
    class ConnectionProvider : IDbConnectionProvider, IDisposable
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public ConnectionProvider(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));

            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                if(_connection == null)
                {
                    _connection = new SQLiteConnection(_connectionString);
                    _connection.Open();
                }

                return _connection;
            }
        }

        public void Dispose()
        {

            if(_connection != null)
            {
                using var diskConnection = new SQLiteConnection(_connectionString);
                diskConnection.Open();
                _connection.Dispose();
            }
        }
    }
}
