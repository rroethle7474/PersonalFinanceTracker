using Newtonsoft.Json;

namespace PersonalFinanceTracker.Salesforce.Models
{
    public class SalesforceBudgetCategory
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type__c")]
        public string Type { get; set; }

        [JsonProperty("Description__c")]
        public string Description { get; set; }

        [JsonProperty("Monthly_Allocation__c")]
        public decimal? MonthlyAllocation { get; set; }

        [JsonProperty("Is_Active__c")]
        public bool IsActive { get; set; }

        [JsonProperty("Parent_Category__c")]
        public string ParentCategoryId { get; set; }
    }
}
