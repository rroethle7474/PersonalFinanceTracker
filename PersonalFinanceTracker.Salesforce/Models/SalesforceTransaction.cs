using Newtonsoft.Json;

namespace PersonalFinanceTracker.Salesforce.Models
{
    public class SalesforceTransaction
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("OwnerId")]
        public string UserId { get; set; }

        [JsonProperty("Payment_Method__c")]
        public string PaymentMethod { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

    }
}