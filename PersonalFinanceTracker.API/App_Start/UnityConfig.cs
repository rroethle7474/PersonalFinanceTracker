using PersonalFinanceTracker.Data.Repositories;
using PersonalFinanceTracker.Data;
using System.Configuration;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.WebApi;

namespace PersonalFinanceTracker.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            string connectionString = ConfigurationManager.ConnectionStrings["PersonalFinanceTrackerDB"].ConnectionString;

            // Register database factory
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor(connectionString));

            // Register repository factory
            container.RegisterType<IRepositoryFactory, RepositoryFactory>(
                new HierarchicalLifetimeManager());

            // Register individual repositories
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IBudgetCategoryRepository, BudgetCategoryRepository>();
            container.RegisterType<ITransactionRepository, TransactionRepository>();
            container.RegisterType<IFinancialGoalRepository, FinancialGoalRepository>();
            container.RegisterType<IInvestmentRepository, InvestmentRepository>();
            container.RegisterType<IPaymentMethodRepository, PaymentMethodRepository>();
            container.RegisterType<IReportRepository, ReportRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}