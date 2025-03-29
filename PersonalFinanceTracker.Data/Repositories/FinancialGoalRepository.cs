using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for FinancialGoal data operations
    /// </summary>
    public class FinancialGoalRepository : BaseRepository, IFinancialGoalRepository
    {
        /// <summary>
        /// Creates a new instance of FinancialGoalRepository
        /// </summary>
        public FinancialGoalRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets a financial goal by ID
        /// </summary>
        public FinancialGoal GetById(int goalId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@GoalID", goalId);
                var dt = db.ExecuteStoredProcedure("usp_GetGoalById", parameter);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToGoal(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets financial goals by user ID with optional filters
        /// </summary>
        public List<FinancialGoal> GetByUserId(int userId, string category = null, string status = null)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", userId),
                    CreateParameter("@Category", category),
                    CreateParameter("@Status", status)
                };

                var dt = db.ExecuteStoredProcedure("usp_GetFinancialGoals", parameters);

                var goals = new List<FinancialGoal>();
                foreach (DataRow row in dt.Rows)
                {
                    goals.Add(MapDataRowToGoal(row));
                }

                return goals;
            }
        }

        /// <summary>
        /// Creates a new financial goal
        /// </summary>
        public int Create(FinancialGoal goal)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", goal.UserID),
                    CreateParameter("@GoalName", goal.GoalName),
                    CreateParameter("@TargetAmount", goal.TargetAmount),
                    CreateParameter("@CurrentAmount", goal.CurrentAmount),
                    CreateParameter("@StartDate", goal.StartDate),
                    CreateParameter("@TargetDate", goal.TargetDate),
                    CreateParameter("@Category", goal.Category),
                    CreateParameter("@Priority", goal.Priority),
                    CreateParameter("@Status", goal.Status),
                    CreateParameter("@Description", goal.Description),
                    CreateParameter("@MonthlyContribution", goal.MonthlyContribution),
                    CreateParameter("@SalesforceID", goal.SalesforceID)
                };

                var result = db.ExecuteScalar("usp_AddFinancialGoal", parameters);
                return ToInt(result);
            }
        }

        /// <summary>
        /// Updates an existing financial goal
        /// </summary>
        public bool Update(FinancialGoal goal)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@GoalID", goal.GoalID),
                    CreateParameter("@GoalName", goal.GoalName),
                    CreateParameter("@TargetAmount", goal.TargetAmount),
                    CreateParameter("@CurrentAmount", goal.CurrentAmount),
                    CreateParameter("@TargetDate", goal.TargetDate),
                    CreateParameter("@Category", goal.Category),
                    CreateParameter("@Priority", goal.Priority),
                    CreateParameter("@Status", goal.Status),
                    CreateParameter("@Description", goal.Description),
                    CreateParameter("@MonthlyContribution", goal.MonthlyContribution)
                };

                return db.ExecuteNonQuery("usp_UpdateFinancialGoal", parameters) > 0;
            }
        }

        /// <summary>
        /// Updates the progress of a financial goal
        /// </summary>
        public bool UpdateProgress(int goalId, decimal newAmount, string newStatus = null)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@GoalID", goalId),
                    CreateParameter("@NewAmount", newAmount),
                    CreateParameter("@NewStatus", newStatus)
                };

                return db.ExecuteNonQuery("usp_UpdateGoalProgress", parameters) > 0;
            }
        }

        /// <summary>
        /// Deletes a financial goal
        /// </summary>
        public bool Delete(int goalId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@GoalID", goalId);
                return db.ExecuteNonQuery("usp_DeleteFinancialGoal", parameter) > 0;
            }
        }

        /// <summary>
        /// Syncs a financial goal from Salesforce
        /// </summary>
        public bool SyncFromSalesforce(FinancialGoal goal)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", goal.UserID),
                    CreateParameter("@SalesforceID", goal.SalesforceID),
                    CreateParameter("@GoalName", goal.GoalName),
                    CreateParameter("@TargetAmount", goal.TargetAmount),
                    CreateParameter("@CurrentAmount", goal.CurrentAmount),
                    CreateParameter("@StartDate", goal.StartDate),
                    CreateParameter("@TargetDate", goal.TargetDate),
                    CreateParameter("@Category", goal.Category),
                    CreateParameter("@Priority", goal.Priority),
                    CreateParameter("@Status", goal.Status),
                    CreateParameter("@Description", goal.Description),
                    CreateParameter("@MonthlyContribution", goal.MonthlyContribution)
                };

                var result = db.ExecuteScalar("usp_SyncFinancialGoalFromSalesforce", parameters);
                return ToInt(result) > 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to a FinancialGoal object
        /// </summary>
        private FinancialGoal MapDataRowToGoal(DataRow row)
        {
            return new FinancialGoal
            {
                GoalID = ToInt(row["GoalID"]),
                UserID = ToInt(row["UserID"]),
                GoalName = ToString(row["GoalName"]),
                TargetAmount = ToDecimal(row["TargetAmount"]),
                CurrentAmount = ToDecimal(row["CurrentAmount"]),
                StartDate = ToDateTime(row["StartDate"]),
                TargetDate = ToDateTime(row["TargetDate"]),
                Category = ToString(row["Category"]),
                Priority = ToString(row["Priority"]),
                Status = ToString(row["Status"]),
                Description = ToString(row["Description"]),
                MonthlyContribution = ToNullableDecimal(row["MonthlyContribution"]),
                SalesforceID = ToString(row["SalesforceID"]),
                LastSyncDate = ToNullableDateTime(row["LastSyncDate"])
            };
        }
    }
}