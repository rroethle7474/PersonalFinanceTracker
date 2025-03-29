using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for Account data operations
    /// </summary>
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        /// <summary>
        /// Creates a new instance of AccountRepository
        /// </summary>
        public AccountRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets an account by ID
        /// </summary>
        public Account GetById(int accountId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@AccountID", accountId);
                var dt = db.ExecuteStoredProcedure("usp_GetAccountById", parameter);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToAccount(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets accounts by user ID
        /// </summary>
        public List<Account> GetByUserId(int userId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@UserID", userId);
                var dt = db.ExecuteStoredProcedure("usp_GetUserAccounts", parameter);

                var accounts = new List<Account>();
                foreach (DataRow row in dt.Rows)
                {
                    accounts.Add(MapDataRowToAccount(row));
                }

                return accounts;
            }
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        public int Create(Account account)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", account.UserID),
                    CreateParameter("@AccountName", account.AccountName),
                    CreateParameter("@AccountType", account.AccountType),
                    CreateParameter("@CurrentBalance", account.CurrentBalance),
                    CreateParameter("@CurrencyCode", account.CurrencyCode),
                    CreateParameter("@SalesforceID", account.SalesforceID)
                };

                var result = db.ExecuteScalar("usp_CreateAccount", parameters);
                return ToInt(result);
            }
        }

        /// <summary>
        /// Updates an existing account
        /// </summary>
        public bool Update(Account account)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@AccountID", account.AccountID),
                    CreateParameter("@AccountName", account.AccountName),
                    CreateParameter("@AccountType", account.AccountType),
                    CreateParameter("@CurrentBalance", account.CurrentBalance),
                    CreateParameter("@CurrencyCode", account.CurrencyCode),
                    CreateParameter("@IsActive", account.IsActive),
                    CreateParameter("@IsFinancialInstitution", account.IsFinancialInstitution),
                    CreateParameter("@IsMerchant", account.IsMerchant),
                    CreateParameter("@Category", account.Category)
                };

                return db.ExecuteNonQuery("usp_UpdateAccount", parameters) > 0;
            }
        }

        /// <summary>
        /// Updates the balance of an account
        /// </summary>
        public bool UpdateBalance(int accountId, decimal newBalance)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@AccountID", accountId),
                    CreateParameter("@NewBalance", newBalance)
                };

                return db.ExecuteNonQuery("usp_UpdateAccountBalance", parameters) > 0;
            }
        }

        /// <summary>
        /// Deletes an account
        /// </summary>
        public bool Delete(int accountId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@AccountID", accountId);
                return db.ExecuteNonQuery("usp_DeleteAccount", parameter) > 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to an Account object
        /// </summary>
        private Account MapDataRowToAccount(DataRow row)
        {
            return new Account
            {
                AccountID = ToInt(row["AccountID"]),
                UserID = ToInt(row["UserID"]),
                AccountName = ToString(row["AccountName"]),
                AccountType = ToString(row["AccountType"]),
                CurrentBalance = ToDecimal(row["CurrentBalance"]),
                CurrencyCode = ToString(row["CurrencyCode"]),
                IsActive = ToBoolean(row["IsActive"]),
                SalesforceID = ToString(row["SalesforceID"]),
                IsFinancialInstitution = row.Table.Columns.Contains("IsFinancialInstitution") ? ToBoolean(row["IsFinancialInstitution"]) : false,
                IsMerchant = row.Table.Columns.Contains("IsMerchant") ? ToBoolean(row["IsMerchant"]) : false,
                Category = row.Table.Columns.Contains("Category") ? ToString(row["Category"]) : null,
                LastUpdated = row.Table.Columns.Contains("LastUpdated") ? ToDateTime(row["LastUpdated"]) : DateTime.MinValue
            };
        }
    }
}