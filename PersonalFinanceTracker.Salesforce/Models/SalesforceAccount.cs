using Newtonsoft.Json;

namespace PersonalFinanceTracker.Salesforce.Models
{
    public class SalesforceAccount
    {
        [JsonProperty("Id")]
        public string AccountId { get; set; }

        [JsonProperty("OwnerId")]
        public string AccountOwner { get; set; }

        [JsonProperty("Name")]
        public string AccountName { get; set; }

        [JsonProperty("Type")]
        public string AccountType { get; set; }

        [JsonProperty("Active__c")]
        public bool IsActive { get; set; }

        [JsonProperty("Is_Financial_Institution__c")]
        public bool IsFinancialInstitution { get; set; }


        [JsonProperty("Is_Merchant__c")]
        public bool IsMerchant { get; set; }

        [JsonProperty("Category__c")]
        public string Category { get; set; }
    }
}