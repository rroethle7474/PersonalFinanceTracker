using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Salesforce.Authentication
{
    public class OAuthConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecurityToken { get; set; }
        public string TokenEndpoint { get; set; } = "https://login.salesforce.com/services/oauth2/token";
        public string InstanceUrl { get; set; }
    }
}
