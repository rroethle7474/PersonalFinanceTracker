using System;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents a financial transaction
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction's unique identifier
        /// </summary>
        public int TransactionID { get; set; }

        /// <summary>
        /// ID of the user who owns this transaction
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// ID of the account associated with this transaction
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// ID of the budget category for this transaction (optional)
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Date and time of the transaction
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Amount of the transaction
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Name of the merchant
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// Detailed description of the transaction
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Type of transaction (Recurring, One-Time, Transfer)
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// Whether this is income (true) or an expense (false)
        /// </summary>
        public bool IsIncome { get; set; }

        /// <summary>
        /// ID in Salesforce if synced
        /// </summary>
        public string SalesforceID { get; set; }

        /// <summary>
        /// When the transaction was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// When the transaction was last modified
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        // Additional properties for API responses
        public string AccountName { get; set; }
        public string CategoryName { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Account Account { get; set; }
        public virtual BudgetCategory Category { get; set; }
    }
}