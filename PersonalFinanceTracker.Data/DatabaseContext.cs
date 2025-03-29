using System;
using System.Data;
using System.Data.SqlClient;

namespace PersonalFinanceTracker.Data
{
    /// <summary>
    /// Handles database connections and provides methods for executing database commands
    /// </summary>
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        /// <summary>
        /// Creates a new instance of the DatabaseContext with the specified connection string
        /// </summary>
        public DatabaseContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));

            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets the database connection, creating it if necessary
        /// </summary>
        private SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }

        /// <summary>
        /// Executes a stored procedure that returns a single result set
        /// </summary>
        public DataTable ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            using (var command = CreateCommand(procedureName, parameters))
            {
                using (var adapter = new SqlDataAdapter(command))
                {
                    var resultTable = new DataTable();
                    adapter.Fill(resultTable);
                    return resultTable;
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure that doesn't return a result set
        /// </summary>
        public int ExecuteNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (var command = CreateCommand(procedureName, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a stored procedure that returns a scalar value
        /// </summary>
        public object ExecuteScalar(string procedureName, params SqlParameter[] parameters)
        {
            using (var command = CreateCommand(procedureName, parameters))
            {
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Creates a command for a stored procedure
        /// </summary>
        private SqlCommand CreateCommand(string procedureName, SqlParameter[] parameters)
        {
            var connection = GetConnection();
            var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        /// <summary>
        /// Creates an output parameter to capture the identity value
        /// </summary>
        public SqlParameter CreateReturnParameter(string paramName = "@ReturnValue")
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
        }

        /// <summary>
        /// Creates an output parameter
        /// </summary>
        public SqlParameter CreateOutputParameter(string paramName, SqlDbType sqlType)
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                SqlDbType = sqlType,
                Direction = ParameterDirection.Output
            };
        }

        /// <summary>
        /// Disposes the connection
        /// </summary>
        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}