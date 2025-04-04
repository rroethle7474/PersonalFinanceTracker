﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Salesforce.Models
{
    public class SalesforcePaymentMethod // map to PersonalFinanceTracker.Models.PaymentMethod
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Current_Balance__c")]
        public decimal CurrentBalance { get; set; }

        [JsonProperty("OwnerId")]
        public string UserId { get; set; } // Lookup to User by Id in Salesforce

        [JsonProperty("Type__c")]
        public string Type { get; set; }


    }
}
