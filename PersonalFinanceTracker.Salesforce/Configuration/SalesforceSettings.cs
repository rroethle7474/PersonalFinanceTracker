namespace PersonalFinanceTracker.Salesforce.Configuration
{
    public class SalesforceSettings
    {
        public AuthSettings Auth { get; set; }
        public ApiSettings Api { get; set; }
        public SyncSettings Sync { get; set; }
    }

    public class AuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecurityToken { get; set; }
        public string TokenEndpoint { get; set; }
    }

    public class ApiSettings
    {
        public string ApiVersion { get; set; }
        public bool UseSandbox { get; set; }
    }

    public class SyncSettings
    {
        public int IntervalMinutes { get; set; }
        public bool EnableAutomatic { get; set; }
        public int MaxItemsPerSync { get; set; }
    }
}