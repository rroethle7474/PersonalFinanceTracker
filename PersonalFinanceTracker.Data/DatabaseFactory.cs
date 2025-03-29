using System;

namespace PersonalFinanceTracker.Data
{
    /// <summary>
    /// Factory for creating database contexts
    /// </summary>
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly string _connectionString;

        /// <summary>
        /// Creates a new instance of the DatabaseFactory with the specified connection string
        /// </summary>
        public DatabaseFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));

            _connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new database context
        /// </summary>
        public IDatabaseContext CreateContext()
        {
            return new DatabaseContext(_connectionString);
        }
    }
}