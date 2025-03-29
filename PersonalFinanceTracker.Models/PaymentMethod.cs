using System;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents a payment method (credit card, debit card, etc.)
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Payment method's unique identifier
        /// </summary>
        public int PaymentMethodID { get; set; }

        /// <summary>
        /// ID of the user who owns this payment method
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Name of the payment method
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Type of payment method (Credit Card, Debit Card, Bank Account, Cash, etc.)
        /// </summary>
        public string MethodType { get; set; }

        /// <summary>
        /// Current outstanding balance (if applicable)
        /// </summary>
        public decimal? CurrentBalance { get; set; }

        /// <summary>
        /// Whether this payment method is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// ID in Salesforce if synced
        /// </summary>
        public string SalesforceID { get; set; }

        /// <summary>
        /// When this payment method was last synced with Salesforce
        /// </summary>
        public DateTime? LastSyncDate { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
    }
}