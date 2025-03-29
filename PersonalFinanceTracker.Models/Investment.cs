using System;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents an investment holding
    /// </summary>
    public class Investment
    {
        /// <summary>
        /// Investment's unique identifier
        /// </summary>
        public int InvestmentID { get; set; }

        /// <summary>
        /// ID of the user who owns this investment
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// ID of the account associated with this investment (optional)
        /// </summary>
        public int? AccountID { get; set; }

        /// <summary>
        /// Name of the asset
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Asset class (Stocks, Bonds, Real Estate, etc.)
        /// </summary>
        public string AssetClass { get; set; }

        /// <summary>
        /// Stock/ETF ticker symbol (if applicable)
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Quantity of units owned
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Initial price per unit when purchased
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Current market price per unit
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Date when the investment was purchased
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Additional notes about the investment
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// ID in Salesforce if synced
        /// </summary>
        public string SalesforceID { get; set; }

        /// <summary>
        /// When the investment data was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Account name for API responses
        /// </summary>
        public string AccountName { get; set; }

        // Calculated properties
        public decimal CurrentValue => CurrentPrice * Quantity;
        public decimal PurchaseValue => PurchasePrice * Quantity;
        public decimal ProfitLoss => CurrentValue - PurchaseValue;
        public decimal ReturnPercentage => PurchaseValue == 0 ? 0 : (ProfitLoss / PurchaseValue) * 100;

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Account Account { get; set; }
    }
}