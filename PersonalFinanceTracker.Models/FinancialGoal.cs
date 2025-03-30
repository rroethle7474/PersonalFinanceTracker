using System;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents a financial goal or savings target
    /// </summary>
    public class FinancialGoal
    {
        /// <summary>
        /// Goal's unique identifier
        /// </summary>
        public int GoalID { get; set; }

        /// <summary>
        /// Category of the goal (Emergency Fund, Retirement, etc.)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Current progress toward the goal
        /// </summary>
        public decimal CurrentAmount { get; set; }


        /// <summary>
        /// Detailed description of the goal
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of the goal
        /// </summary>
        public string GoalName { get; set; }

        /// <summary>
        /// Planned monthly contribution
        /// </summary>
        public decimal? MonthlyContribution { get; set; }

        /// <summary>
        /// ID of the user who owns this goal
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Priority level (High, Medium, Low)
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// When the goal was started
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Current status (Not Started, In Progress, Completed)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Target amount to save/achieve
        /// </summary>
        public decimal TargetAmount { get; set; }

        /// <summary>
        /// Deadline for achieving the goal
        /// </summary>
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// ID in Salesforce if synced
        /// </summary>
        public string SalesforceID { get; set; }

        /// <summary>
        /// When this goal was last synced with Salesforce
        /// </summary>
        public DateTime? LastSyncDate { get; set; }

        // Calculated properties
        public decimal PercentComplete => (CurrentAmount / TargetAmount) * 100;
        public int DaysRemaining => (TargetDate - DateTime.Now).Days;
    }
}