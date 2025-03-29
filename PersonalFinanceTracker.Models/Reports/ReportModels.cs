using System;
using System.Collections.Generic;

namespace PersonalFinanceTracker.Models.Reports
{
    /// <summary>
    /// Represents monthly spending for a category
    /// </summary>
    public class CategorySpending
    {
        public string CategoryName { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal? MonthlyAllocation { get; set; }
        public decimal? Remaining { get; set; }
        public decimal PercentUsed { get; set; }
    }

    /// <summary>
    /// Represents monthly income vs expenses
    /// </summary>
    public class MonthlyFinancialSummary
    {
        public DateTime MonthDate { get; set; }
        public string MonthYear { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal NetSavings { get; set; }
    }

    /// <summary>
    /// Represents net worth data for a specific month
    /// </summary>
    public class NetWorthSummary
    {
        public DateTime MonthDate { get; set; }
        public string MonthYear { get; set; }
        public decimal NetWorth { get; set; }
        public decimal LiquidAssets { get; set; }
        public decimal Investments { get; set; }
        public decimal Debt { get; set; }
    }

    /// <summary>
    /// Represents asset allocation by asset class
    /// </summary>
    public class AssetAllocation
    {
        public string AssetClass { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Represents parameters for report generation
    /// </summary>
    public class ReportParameters
    {
        public int UserID { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? MonthsBack { get; set; }
    }
}