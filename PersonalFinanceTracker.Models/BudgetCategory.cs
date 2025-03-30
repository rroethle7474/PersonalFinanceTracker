using System;
using System.Collections.Generic;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents a budget category for classifying income and expenses
    /// </summary>
    public class BudgetCategory
    {
        /// <summary>
        /// Category's unique identifier
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// ID of the user who owns this category
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Type of category (Income, Expense)
        /// </summary>
        public string CategoryType { get; set; }

        /// <summary>
        /// Detailed description of the category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Monthly budget allocation for this category
        /// </summary>
        public decimal? MonthlyAllocation { get; set; }

        /// <summary>
        /// Whether this category is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// ID of the parent category for hierarchical categorization
        /// </summary>
        public int? ParentCategoryID { get; set; }

        /// <summary>
        /// ID in Salesforce if synced
        /// </summary>
        public string SalesforceID { get; set; }

        /// <summary>
        /// When this category was last synced with Salesforce
        /// </summary>
        public DateTime? LastSyncDate { get; set; }
    }
}