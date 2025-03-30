using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Salesforce.Models
{
    public class SalesforceFinancialGoal // won't be used at this time but would map to PersonalFinanceTracker.Models.FinancialGoal
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Category__c")]
        public string Category { get; set; }

        [JsonProperty("Description__c")]
        public string Description { get; set; }

        [JsonProperty("Current_Amount__c")]
        public decimal? CurrentAmount { get; set; }

        [JsonProperty("Monthly_Contribution__c")]
        public decimal? MonthlyContribution { get; set; }

        [JsonProperty("Target_Amount__c")]
        public decimal? TargetAmount { get; set; }

        [JsonProperty("Start_Date__c")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("Target_Date__c")]
        public DateTime? TargetDate { get; set; }

        [JsonProperty("OwnerId")]
        public string UserId { get; set; } // Assuming this is the ID of the user who owns the goal

        [JsonProperty("Status__c")]
        public string Status { get; set; } // e.g., Not Started, In Progress, Completed

        [JsonProperty("Priority__c")]
        public string Priority { get; set; } // e.g., High, Medium, Low

    }
}
