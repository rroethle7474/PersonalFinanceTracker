using System;
using System.Collections.Generic;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Models.Reports;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Interface for User data operations
    /// </summary>
    public interface IUserRepository
    {
        User GetById(int userId);
        User GetByUsername(string username);
        User GetByEmail(string email);
        int Create(User user, string passwordHash);
        bool Update(User user);
        bool Authenticate(string username, string passwordHash);
    }

    /// <summary>
    /// Interface for Account data operations
    /// </summary>
    public interface IAccountRepository
    {
        Account GetById(int accountId);
        List<Account> GetByUserId(int userId);
        int Create(Account account);
        bool Update(Account account);
        bool UpdateBalance(int accountId, decimal newBalance);
        bool Delete(int accountId);
    }

    /// <summary>
    /// Interface for BudgetCategory data operations
    /// </summary>
    public interface IBudgetCategoryRepository
    {
        BudgetCategory GetById(int categoryId);
        List<BudgetCategory> GetByUserId(int userId, string categoryType = null);
        int Create(BudgetCategory category);
        bool Update(BudgetCategory category);
        bool Delete(int categoryId);
        bool SyncFromSalesforce(BudgetCategory category);
    }

    /// <summary>
    /// Interface for Transaction data operations
    /// </summary>
    public interface ITransactionRepository
    {
        Transaction GetById(int transactionId);
        List<Transaction> GetByUserId(int userId, DateTime? startDate = null, DateTime? endDate = null,
            int? categoryId = null, int? accountId = null, bool? isIncome = null);
        int Create(Transaction transaction);
        bool Update(Transaction transaction);
        bool Delete(int transactionId);
    }

    /// <summary>
    /// Interface for FinancialGoal data operations
    /// </summary>
    public interface IFinancialGoalRepository
    {
        FinancialGoal GetById(int goalId);
        List<FinancialGoal> GetByUserId(int userId, string category = null, string status = null);
        int Create(FinancialGoal goal);
        bool Update(FinancialGoal goal);
        bool UpdateProgress(int goalId, decimal newAmount, string newStatus = null);
        bool Delete(int goalId);
        bool SyncFromSalesforce(FinancialGoal goal);
    }

    /// <summary>
    /// Interface for Investment data operations
    /// </summary>
    public interface IInvestmentRepository
    {
        Investment GetById(int investmentId);
        List<Investment> GetByUserId(int userId, string assetClass = null);
        int Create(Investment investment);
        bool Update(Investment investment);
        bool UpdatePrice(int investmentId, decimal newPrice);
        bool Delete(int investmentId);
    }

    /// <summary>
    /// Interface for PaymentMethod data operations
    /// </summary>
    public interface IPaymentMethodRepository
    {
        PaymentMethod GetById(int paymentMethodId);
        List<PaymentMethod> GetByUserId(int userId);
        int Create(PaymentMethod paymentMethod);
        bool Update(PaymentMethod paymentMethod);
        bool Delete(int paymentMethodId);
        bool SyncFromSalesforce(PaymentMethod paymentMethod);
    }

    /// <summary>
    /// Interface for report generation operations
    /// </summary>
    public interface IReportRepository
    {
        List<CategorySpending> GetMonthlyCategorySpending(int userId, int year, int month);
        List<MonthlyFinancialSummary> GetIncomeVsExpenses(int userId, int monthsBack = 12);
        List<NetWorthSummary> GetNetWorthTrend(int userId, int monthsBack = 12);
        List<AssetAllocation> GetAssetAllocation(int userId);
    }
}