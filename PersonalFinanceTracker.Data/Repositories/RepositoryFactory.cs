using System;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Factory for creating repositories
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IDatabaseFactory _dbFactory;

        /// <summary>
        /// Creates a new instance of RepositoryFactory
        /// </summary>
        public RepositoryFactory(IDatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        /// <summary>
        /// Creates a user repository
        /// </summary>
        public IUserRepository CreateUserRepository()
        {
            return new UserRepository(_dbFactory);
        }

        /// <summary>
        /// Creates an account repository
        /// </summary>
        public IAccountRepository CreateAccountRepository()
        {
            return new AccountRepository(_dbFactory);
        }

        /// <summary>
        /// Creates a budget category repository
        /// </summary>
        public IBudgetCategoryRepository CreateBudgetCategoryRepository()
        {
            return new BudgetCategoryRepository(_dbFactory);
        }

        /// <summary>
        /// Creates a transaction repository
        /// </summary>
        public ITransactionRepository CreateTransactionRepository()
        {
            return new TransactionRepository(_dbFactory);
        }

        /// <summary>
        /// Creates a financial goal repository
        /// </summary>
        public IFinancialGoalRepository CreateFinancialGoalRepository()
        {
            return new FinancialGoalRepository(_dbFactory);
        }

        /// <summary>
        /// Creates an investment repository
        /// </summary>
        public IInvestmentRepository CreateInvestmentRepository()
        {
            return new InvestmentRepository(_dbFactory);
        }

        /// <summary>
        /// Creates a payment method repository
        /// </summary>
        public IPaymentMethodRepository CreatePaymentMethodRepository()
        {
            return new PaymentMethodRepository(_dbFactory);
        }

        /// <summary>
        /// Creates a report repository
        /// </summary>
        public IReportRepository CreateReportRepository()
        {
            return new ReportRepository(_dbFactory);
        }
    }

    /// <summary>
    /// Interface for repository factory
    /// </summary>
    public interface IRepositoryFactory
    {
        IUserRepository CreateUserRepository();
        IAccountRepository CreateAccountRepository();
        IBudgetCategoryRepository CreateBudgetCategoryRepository();
        ITransactionRepository CreateTransactionRepository();
        IFinancialGoalRepository CreateFinancialGoalRepository();
        IInvestmentRepository CreateInvestmentRepository();
        IPaymentMethodRepository CreatePaymentMethodRepository();
        IReportRepository CreateReportRepository();
    }
}