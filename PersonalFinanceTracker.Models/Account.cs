using System;
using System.Collections.Generic;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents a financial account (bank account, credit card, etc.)
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Account's unique identifier
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// ID of the user who owns this account
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Descriptive name of the account
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Type of account (Checking, Savings, Credit Card, Investment)
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Current balance of the account
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// Currency code (USD, EUR, etc.)
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Whether the account is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// ID in Salesforce if synced
        /// </summary>
        public string SalesforceID { get; set; }

        /// <summary>
        /// Whether this is a financial institution entity
        /// </summary>
        public bool IsFinancialInstitution { get; set; }

        /// <summary>
        /// Whether this is a merchant entity
        /// </summary>
        public bool IsMerchant { get; set; }

        /// <summary>
        /// Category classification
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// When the account was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}