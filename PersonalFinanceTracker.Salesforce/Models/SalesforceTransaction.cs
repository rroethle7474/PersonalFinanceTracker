using Newtonsoft.Json;
using System;

namespace PersonalFinanceTracker.Salesforce.Models
{
    public class SalesforceTransaction // map to PersonalFinanceTracker.Models.Transaction
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Amount__c")]
        public decimal Amount { get; set; }

        [JsonProperty("Date__c")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty("Is_Income__c")]
        public bool IsIncome { get; set; }

        [JsonProperty("Merchant__c")]
        public string Merchant { get; set; } // Lookup to Account by Id in Salesforce

        [JsonProperty("Notes__c")]
        public string Notes { get; set; } // Not directly mapped to Salesforce field, but included for completeness

        [JsonProperty("OwnerId")]
        public string UserId { get; set; } // Lookup to User by Id in Salesforce

        [JsonProperty("Payment_Method__c")]
        public string PaymentMethod { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Transaction_Type__c")]
        public string TransactionType { get; set; }

    }
}