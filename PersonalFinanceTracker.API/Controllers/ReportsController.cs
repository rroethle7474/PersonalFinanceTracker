using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models.Reports;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for generating financial reports
    /// </summary>
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        private readonly IReportRepository _reportRepository;

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
        }

        /// <summary>
        /// Get monthly spending by category
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <returns>List of category spending</returns>
        [HttpGet]
        [Route("category-spending")]
        public IHttpActionResult GetMonthlyCategorySpending(int userId, int year, int month)
        {
            try
            {
                var spending = _reportRepository.GetMonthlyCategorySpending(userId, year, month);
                return Ok(ApiResponse<CategorySpending[]>.CreateSuccess(spending.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get monthly income vs expenses
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="monthsBack">Number of months to look back (default: 12)</param>
        /// <returns>List of monthly financial summaries</returns>
        [HttpGet]
        [Route("income-expenses")]
        public IHttpActionResult GetIncomeVsExpenses(int userId, int monthsBack = 12)
        {
            try
            {
                var summaries = _reportRepository.GetIncomeVsExpenses(userId, monthsBack);
                return Ok(ApiResponse<MonthlyFinancialSummary[]>.CreateSuccess(summaries.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get net worth trend over time
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="monthsBack">Number of months to look back (default: 12)</param>
        /// <returns>List of net worth summaries</returns>
        [HttpGet]
        [Route("net-worth")]
        public IHttpActionResult GetNetWorthTrend(int userId, int monthsBack = 12)
        {
            try
            {
                var netWorth = _reportRepository.GetNetWorthTrend(userId, monthsBack);
                return Ok(ApiResponse<NetWorthSummary[]>.CreateSuccess(netWorth.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get asset allocation by asset class
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of asset allocations</returns>
        [HttpGet]
        [Route("asset-allocation")]
        public IHttpActionResult GetAssetAllocation(int userId)
        {
            try
            {
                var allocation = _reportRepository.GetAssetAllocation(userId);
                return Ok(ApiResponse<AssetAllocation[]>.CreateSuccess(allocation.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
} 