using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for Transaction data operations
    /// </summary>
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        /// <summary>
        /// Creates a new instance of TransactionRepository
        /// </summary>
        public TransactionRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets a transaction by ID
        /// </summary>
        public Transaction GetById(int transactionId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@TransactionID", transactionId);
                var dt = db.ExecuteStoredProcedure("usp_GetTransactionById", parameter);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToTransaction(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets transactions by user ID with optional filters
        /// </summary>
        public List<Transaction> GetByUserId(int userId, DateTime? startDate = null, DateTime? endDate = null,
            int? categoryId = null, int? accountId = null, bool? isIncome = null)
        {
            using (var db = CreateContext())
            {
                var parameters = new List<SqlParameter>
                {
                    CreateParameter("@UserID", userId),
                    CreateNullableParameter("@StartDate", startDate),
                    CreateNullableParameter("@EndDate", endDate),
                    CreateNullableParameter("@CategoryID", categoryId),
                    CreateNullableParameter("@AccountID", accountId),
                    CreateNullableParameter("@IsIncome", isIncome)
                };

                var dt = db.ExecuteStoredProcedure("usp_GetUserTransactions", parameters.ToArray());

                var transactions = new List<Transaction>();
                foreach (DataRow row in dt.Rows)
                {
                    transactions.Add(MapDataRowToTransaction(row));
                }

                return transactions;
            }
        }

        /// <summary>
        /// Creates a new transaction
        /// </summary>
        public int Create(Transaction transaction)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", transaction.UserID),
                    CreateParameter("@AccountID", transaction.AccountID),
                    CreateNullableParameter("@CategoryID", transaction.CategoryID),
                    CreateParameter("@TransactionDate", transaction.TransactionDate),
                    CreateParameter("@Amount", transaction.Amount),
                    CreateParameter("@MerchantName", transaction.MerchantName),
                    CreateParameter("@Description", transaction.Description),
                    CreateParameter("@TransactionType", transaction.TransactionType),
                    CreateParameter("@IsIncome", transaction.IsIncome)
                };

                var result = db.ExecuteScalar("usp_AddTransaction", parameters);
                return ToInt(result);
            }
        }

        /// <summary>
        /// Updates an existing transaction
        /// </summary>
        public bool Update(Transaction transaction)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@TransactionID", transaction.TransactionID),
                    CreateParameter("@CategoryID", transaction.CategoryID),
                    CreateParameter("@TransactionDate", transaction.TransactionDate),
                    CreateParameter("@Amount", transaction.Amount),
                    CreateParameter("@MerchantName", transaction.MerchantName),
                    CreateParameter("@Description", transaction.Description),
                    CreateParameter("@TransactionType", transaction.TransactionType)
                };

                return db.ExecuteNonQuery("usp_UpdateTransaction", parameters) > 0;
            }
        }

        /// <summary>
        /// Deletes a transaction
        /// </summary>
        public bool Delete(int transactionId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@TransactionID", transactionId);
                return db.ExecuteNonQuery("usp_DeleteTransaction", parameter) > 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to a Transaction object
        /// </summary>
        private Transaction MapDataRowToTransaction(DataRow row)
        {
            return new Transaction
            {
                TransactionID = ToInt(row["TransactionID"]),
                UserID = ToInt(row["UserID"]),
                AccountID = ToInt(row["AccountID"]),
                CategoryID = ToNullableInt(row["CategoryID"]),
                TransactionDate = ToDateTime(row["TransactionDate"]),
                Amount = ToDecimal(row["Amount"]),
                MerchantName = ToString(row["MerchantName"]),
                Description = ToString(row["Description"]),
                TransactionType = ToString(row["TransactionType"]),
                IsIncome = ToBoolean(row["IsIncome"]),
                AccountName = row.Table.Columns.Contains("AccountName") ? ToString(row["AccountName"]) : null,
                CategoryName = row.Table.Columns.Contains("CategoryName") ? ToString(row["CategoryName"]) : null,
                CreatedDate = row.Table.Columns.Contains("CreatedDate") ? ToDateTime(row["CreatedDate"]) : DateTime.MinValue,
                ModifiedDate = row.Table.Columns.Contains("ModifiedDate") ? ToDateTime(row["ModifiedDate"]) : DateTime.MinValue,
                SalesforceID = row.Table.Columns.Contains("SalesforceID") ? ToString(row["SalesforceID"]) : null
            };
        }
    }
}