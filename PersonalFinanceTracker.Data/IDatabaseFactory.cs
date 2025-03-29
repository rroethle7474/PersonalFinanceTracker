namespace PersonalFinanceTracker.Data
{
    /// <summary>
    /// Interface for database factory
    /// </summary>
    public interface IDatabaseFactory
    {
        /// <summary>
        /// Creates a new database context
        /// </summary>
        IDatabaseContext CreateContext();
    }
}
