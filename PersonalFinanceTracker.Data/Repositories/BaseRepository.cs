using System;
using System.Data.SqlClient;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Base class for all repositories
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly IDatabaseFactory _dbFactory;

        /// <summary>
        /// Creates a new instance of BaseRepository
        /// </summary>
        /// <param name="dbFactory">The database factory to use</param>
        protected BaseRepository(IDatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        /// <summary>
        /// Creates a new database context
        /// </summary>
        protected IDatabaseContext CreateContext()
        {
            return _dbFactory.CreateContext();
        }

        /// <summary>
        /// Converts a DataRow to a boolean
        /// </summary>
        protected bool ToBoolean(object value)
        {
            if (value == null || value == DBNull.Value)
                return false;

            return Convert.ToBoolean(value);
        }

        /// <summary>
        /// Converts a DataRow value to a nullable int
        /// </summary>
        protected int? ToNullableInt(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Converts a DataRow value to an int
        /// </summary>
        protected int ToInt(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Converts a DataRow value to a nullable decimal
        /// </summary>
        protected decimal? ToNullableDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Converts a DataRow value to a decimal
        /// </summary>
        protected decimal ToDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0m;

            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Converts a DataRow value to a string
        /// </summary>
        protected string ToString(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToString(value);
        }

        /// <summary>
        /// Converts a DataRow value to a nullable DateTime
        /// </summary>
        protected DateTime? ToNullableDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// Converts a DataRow value to a DateTime
        /// </summary>
        protected DateTime ToDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return DateTime.MinValue;

            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// Creates a SqlParameter with the given name and value
        /// </summary>
        protected SqlParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Creates a SqlParameter with the given name and value for a nullable type
        /// </summary>
        protected SqlParameter CreateNullableParameter<T>(string name, T? value) where T : struct
        {
            return new SqlParameter(name, value.HasValue ? (object)value.Value : DBNull.Value);
        }
    }
}