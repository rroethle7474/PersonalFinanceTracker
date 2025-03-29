using PersonalFinanceTracker.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for Investment data operations
    /// </summary>
    public class InvestmentRepository : BaseRepository, IInvestmentRepository
    {
        /// <summary>
        /// Creates a new instance of InvestmentRepository
        /// </summary>
        public InvestmentRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets an investment by ID
        /// </summary>
        public Investment GetById(int investmentId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@InvestmentID", investmentId);
                var dt = db.ExecuteStoredProcedure("usp_GetInvestmentById", parameter);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToInvestment(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets investments by user ID with optional filter by asset class
        /// </summary>
        public List<Investment> GetByUserId(int userId, string assetClass = null)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", userId),
                    CreateParameter("@AssetClass", assetClass)
                };

                var dt = db.ExecuteStoredProcedure("usp_GetInvestments", parameters);

                var investments = new List<Investment>();
                foreach (DataRow row in dt.Rows)
                {
                    investments.Add(MapDataRowToInvestment(row));
                }

                return investments;
            }
        }

        /// <summary>
        /// Creates a new investment
        /// </summary>
        public int Create(Investment investment)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", investment.UserID),
                    CreateNullableParameter("@AccountID", investment.AccountID),
                    CreateParameter("@AssetName", investment.AssetName),
                    CreateParameter("@AssetClass", investment.AssetClass),
                    CreateParameter("@Ticker", investment.Ticker),
                    CreateParameter("@Quantity", investment.Quantity),
                    CreateParameter("@PurchasePrice", investment.PurchasePrice),
                    CreateParameter("@CurrentPrice", investment.CurrentPrice),
                    CreateParameter("@PurchaseDate", investment.PurchaseDate),
                    CreateParameter("@Notes", investment.Notes),
                    CreateParameter("@SalesforceID", investment.SalesforceID)
                };

                var result = db.ExecuteScalar("usp_AddInvestment", parameters);
                return ToInt(result);
            }
        }

        /// <summary>
        /// Updates an existing investment
        /// </summary>
        public bool Update(Investment investment)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@InvestmentID", investment.InvestmentID),
                    CreateNullableParameter("@AccountID", investment.AccountID),
                    CreateParameter("@AssetName", investment.AssetName),
                    CreateParameter("@AssetClass", investment.AssetClass),
                    CreateParameter("@Ticker", investment.Ticker),
                    CreateParameter("@Quantity", investment.Quantity),
                    CreateParameter("@PurchasePrice", investment.PurchasePrice),
                    CreateParameter("@CurrentPrice", investment.CurrentPrice),
                    CreateParameter("@PurchaseDate", investment.PurchaseDate),
                    CreateParameter("@Notes", investment.Notes)
                };

                return db.ExecuteNonQuery("usp_UpdateInvestment", parameters) > 0;
            }
        }

        /// <summary>
        /// Updates the current price of an investment
        /// </summary>
        public bool UpdatePrice(int investmentId, decimal newPrice)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@InvestmentID", investmentId),
                    CreateParameter("@NewPrice", newPrice)
                };

                return db.ExecuteNonQuery("usp_UpdateInvestmentPrice", parameters) > 0;
            }
        }

        /// <summary>
        /// Deletes an investment
        /// </summary>
        public bool Delete(int investmentId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@InvestmentID", investmentId);
                return db.ExecuteNonQuery("usp_DeleteInvestment", parameter) > 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to an Investment object
        /// </summary>
        private Investment MapDataRowToInvestment(DataRow row)
        {
            var investment = new Investment
            {
                InvestmentID = ToInt(row["InvestmentID"]),
                UserID = ToInt(row["UserID"]),
                AccountID = ToNullableInt(row["AccountID"]),
                AssetName = ToString(row["AssetName"]),
                AssetClass = ToString(row["AssetClass"]),
                Ticker = ToString(row["Ticker"]),
                Quantity = ToDecimal(row["Quantity"]),
                PurchasePrice = ToDecimal(row["PurchasePrice"]),
                CurrentPrice = ToDecimal(row["CurrentPrice"]),
                PurchaseDate = ToDateTime(row["PurchaseDate"]),
                Notes = ToString(row["Notes"]),
                SalesforceID = ToString(row["SalesforceID"]),
                LastUpdated = row.Table.Columns.Contains("LastUpdated") ? ToDateTime(row["LastUpdated"]) : DateTime.MinValue
            };

            // Add account name if available
            if (row.Table.Columns.Contains("AccountName"))
            {
                investment.AccountName = ToString(row["AccountName"]);
            }

            return investment;
        }
    }
}