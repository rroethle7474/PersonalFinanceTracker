using System;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents a tracking record for Salesforce synchronization
    /// </summary>
    public class SalesforceSync
    {
        /// <summary>
        /// Sync record's unique identifier
        /// </summary>
        public int SyncID { get; set; }

        /// <summary>
        /// Type of object being synced (Budget_Category__c, Financial_Goal__c, etc.)
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// ID of the local object
        /// </summary>
        public int LocalID { get; set; }

        /// <summary>
        /// ID of the object in Salesforce
        /// </summary>
        public string SalesforceID { get; set; }

        /// <summary>
        /// When the sync occurred
        /// </summary>
        public DateTime LastSyncDate { get; set; }

        /// <summary>
        /// Status of the sync operation (Success, Failed, Pending)
        /// </summary>
        public string SyncStatus { get; set; }

        /// <summary>
        /// Error message if sync failed
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}