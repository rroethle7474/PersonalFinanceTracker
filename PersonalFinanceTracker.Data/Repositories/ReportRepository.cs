using PersonalFinanceTracker.Models.Reports;
using System.Collections.Generic;
using System.Data;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for generating financial reports
    /// </summary>
    public class ReportRepository : BaseRepository, IReportRepository
    {
        /// <summary>
        /// Creates a new instance of ReportRepository
        /// </summary>
        public ReportRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets monthly spending by category
        /// </summary>
        public List<CategorySpending> GetMonthlyCategorySpending(int userId, int year, int month)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", userId),
                    CreateParameter("@Year", year),
                    CreateParameter("@Month", month)
                };

                var dt = db.ExecuteStoredProcedure("usp_GetMonthlyCategorySpending", parameters);

                var spending = new List<CategorySpending>();
                foreach (DataRow row in dt.Rows)
                {
                    spending.Add(new CategorySpending
                    {
                        CategoryName = ToString(row["CategoryName"]),
                        TotalSpent = ToDecimal(row["TotalSpent"]),
                        MonthlyAllocation = ToNullableDecimal(row["MonthlyAllocation"]),
                        Remaining = ToNullableDecimal(row["Remaining"]),
                        PercentUsed = ToDecimal(row["PercentUsed"])
                    });
                }

                return spending;
            }
        }

        /// <summary>
        /// Gets monthly income vs expenses
        /// </summary>
        public List<MonthlyFinancialSummary> GetIncomeVsExpenses(int userId, int monthsBack = 12)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", userId),
                    CreateParameter("@MonthsBack", monthsBack)
                };

                var dt = db.ExecuteStoredProcedure("usp_GetIncomeVsExpensesByMonth", parameters);

                var summaries = new List<MonthlyFinancialSummary>();
                foreach (DataRow row in dt.Rows)
                {
                    summaries.Add(new MonthlyFinancialSummary
                    {
                        MonthDate = ToDateTime(row["MonthDate"]),
                        MonthYear = ToString(row["MonthYear"]),
                        Income = ToDecimal(row["Income"]),
                        Expenses = ToDecimal(row["Expenses"]),
                        NetSavings = ToDecimal(row["NetSavings"])
                    });
                }

                return summaries;
            }
        }

        /// <summary>
        /// Gets net worth trend over time
        /// </summary>
        public List<NetWorthSummary> GetNetWorthTrend(int userId, int monthsBack = 12)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", userId),
                    CreateParameter("@MonthsBack", monthsBack)
                };

                var dt = db.ExecuteStoredProcedure("usp_GetNetWorthTrend", parameters);

                var netWorth = new List<NetWorthSummary>();
                foreach (DataRow row in dt.Rows)
                {
                    netWorth.Add(new NetWorthSummary
                    {
                        MonthDate = ToDateTime(row["MonthDate"]),
                        MonthYear = ToString(row["MonthYear"]),
                        NetWorth = ToDecimal(row["NetWorth"]),
                        LiquidAssets = ToDecimal(row["LiquidAssets"]),
                        Investments = ToDecimal(row["Investments"]),
                        Debt = ToDecimal(row["Debt"])
                    });
                }

                return netWorth;
            }
        }

        /// <summary>
        /// Gets asset allocation by asset class
        /// </summary>
        public List<AssetAllocation> GetAssetAllocation(int userId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@UserID", userId);
                var dt = db.ExecuteStoredProcedure("usp_GetAssetAllocation", parameter);

                var allocation = new List<AssetAllocation>();
                foreach (DataRow row in dt.Rows)
                {
                    allocation.Add(new AssetAllocation
                    {
                        AssetClass = ToString(row["AssetClass"]),
                        TotalValue = ToDecimal(row["TotalValue"]),
                        Percentage = ToDecimal(row["Percentage"])
                    });
                }

                return allocation;
            }
        }
    }
}