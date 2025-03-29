using PersonalFinanceTracker.Models;
using System.Collections.Generic;
using System.Data;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for BudgetCategory data operations
    /// </summary>
    public class BudgetCategoryRepository : BaseRepository, IBudgetCategoryRepository
    {
        /// <summary>
        /// Creates a new instance of BudgetCategoryRepository
        /// </summary>
        public BudgetCategoryRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets a budget category by ID
        /// </summary>
        public BudgetCategory GetById(int categoryId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@CategoryID", categoryId);
                var dt = db.ExecuteStoredProcedure("usp_GetCategoryById", parameter);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToCategory(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets budget categories by user ID with optional filter by type
        /// </summary>
        public List<BudgetCategory> GetByUserId(int userId, string categoryType = null)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", userId),
                    CreateParameter("@CategoryType", categoryType)
                };

                var dt = db.ExecuteStoredProcedure("usp_GetBudgetCategories", parameters);

                var categories = new List<BudgetCategory>();
                foreach (DataRow row in dt.Rows)
                {
                    categories.Add(MapDataRowToCategory(row));
                }

                return categories;
            }
        }

        /// <summary>
        /// Creates a new budget category
        /// </summary>
        public int Create(BudgetCategory category)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", category.UserID),
                    CreateParameter("@CategoryName", category.CategoryName),
                    CreateParameter("@CategoryType", category.CategoryType),
                    CreateParameter("@Description", category.Description),
                    CreateParameter("@MonthlyAllocation", category.MonthlyAllocation),
                    CreateNullableParameter("@ParentCategoryID", category.ParentCategoryID),
                    CreateParameter("@SalesforceID", category.SalesforceID)
                };

                var result = db.ExecuteScalar("usp_AddBudgetCategory", parameters);
                return ToInt(result);
            }
        }

        /// <summary>
        /// Updates an existing budget category
        /// </summary>
        public bool Update(BudgetCategory category)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@CategoryID", category.CategoryID),
                    CreateParameter("@CategoryName", category.CategoryName),
                    CreateParameter("@CategoryType", category.CategoryType),
                    CreateParameter("@Description", category.Description),
                    CreateParameter("@MonthlyAllocation", category.MonthlyAllocation),
                    CreateNullableParameter("@ParentCategoryID", category.ParentCategoryID),
                    CreateParameter("@IsActive", category.IsActive)
                };

                return db.ExecuteNonQuery("usp_UpdateBudgetCategory", parameters) > 0;
            }
        }

        /// <summary>
        /// Deletes a budget category
        /// </summary>
        public bool Delete(int categoryId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@CategoryID", categoryId);
                return db.ExecuteNonQuery("usp_DeleteBudgetCategory", parameter) > 0;
            }
        }

        /// <summary>
        /// Syncs a budget category from Salesforce
        /// </summary>
        public bool SyncFromSalesforce(BudgetCategory category)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", category.UserID),
                    CreateParameter("@SalesforceID", category.SalesforceID),
                    CreateParameter("@CategoryName", category.CategoryName),
                    CreateParameter("@CategoryType", category.CategoryType),
                    CreateParameter("@Description", category.Description),
                    CreateParameter("@MonthlyAllocation", category.MonthlyAllocation),
                    CreateParameter("@IsActive", category.IsActive),
                    CreateParameter("@ParentSalesforceID", null) // You'd need to pass the parent's Salesforce ID
                };

                var result = db.ExecuteScalar("usp_SyncBudgetCategoryFromSalesforce", parameters);
                return ToInt(result) > 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to a BudgetCategory object
        /// </summary>
        private BudgetCategory MapDataRowToCategory(DataRow row)
        {
            return new BudgetCategory
            {
                CategoryID = ToInt(row["CategoryID"]),
                UserID = ToInt(row["UserID"]),
                CategoryName = ToString(row["CategoryName"]),
                CategoryType = ToString(row["CategoryType"]),
                Description = ToString(row["Description"]),
                MonthlyAllocation = ToNullableDecimal(row["MonthlyAllocation"]),
                IsActive = ToBoolean(row["IsActive"]),
                ParentCategoryID = ToNullableInt(row["ParentCategoryID"]),
                SalesforceID = ToString(row["SalesforceID"]),
                LastSyncDate = ToNullableDateTime(row["LastSyncDate"])
            };
        }
    }
}