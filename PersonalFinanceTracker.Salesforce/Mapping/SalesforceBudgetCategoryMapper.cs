using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Salesforce.Models;

namespace PersonalFinanceTracker.Salesforce.Mapping
{
    public class SalesforceBudgetCategoryMapper
    {
        public SalesforceBudgetCategory ToSalesforceModel(BudgetCategory local)
        {
            return new SalesforceBudgetCategory
            {
                Id = local.SalesforceID,
                Name = local.CategoryName,
                Type = local.CategoryType,
                Description = local.Description,
                MonthlyAllocation = local.MonthlyAllocation,
                IsActive = local.IsActive
            };
        }

        public BudgetCategory ToLocalModel(SalesforceBudgetCategory salesforce)
        {
            return new BudgetCategory
            {
                CategoryName = salesforce.Name,
                CategoryType = salesforce.Type,
                Description = salesforce.Description,
                MonthlyAllocation = salesforce.MonthlyAllocation,
                IsActive = salesforce.IsActive,
                SalesforceID = salesforce.Id,
                LastSyncDate = System.DateTime.UtcNow
            };
        }
    }
}
