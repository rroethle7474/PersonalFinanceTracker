using System;
using System.Data;
using System.Data.SqlClient;

namespace PersonalFinanceTracker.Data
{
    /// <summary>
    /// Interface for database context operations
    /// </summary>
    public interface IDatabaseContext : IDisposable
    {
        /// <summary>
        /// Executes a stored procedure that returns a single result set
        /// </summary>
        DataTable ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters);

        /// <summary>
        /// Executes a stored procedure that doesn't return a result set
        /// </summary>
        int ExecuteNonQuery(string procedureName, params SqlParameter[] parameters);

        /// <summary>
        /// Executes a stored procedure that returns a scalar value
        /// </summary>
        object ExecuteScalar(string procedureName, params SqlParameter[] parameters);

        /// <summary>
        /// Creates an output parameter to capture the return value
        /// </summary>
        SqlParameter CreateReturnParameter(string paramName = "@ReturnValue");

        /// <summary>
        /// Creates an output parameter
        /// </summary>
        SqlParameter CreateOutputParameter(string paramName, SqlDbType sqlType);
    }
}