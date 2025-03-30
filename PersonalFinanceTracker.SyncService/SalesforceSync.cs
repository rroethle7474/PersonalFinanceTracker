using PersonalFinanceTracker.Models;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;

namespace PersonalFinanceTracker.SyncService
{
    public partial class SalesforceSync : ServiceBase
    {
        private Timer _syncTimer;
        private HttpClient _httpClient;
        private bool _isSyncRunning;
        private EventLog _eventLog;
        private readonly string _apiBaseUrl;
        private readonly int _syncIntervalMinutes;
        private readonly string _apiKey;

        public SalesforceSync()
        {
            InitializeComponent();

            // Load configuration settings
            _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "https://localhost:44397/api";
            _syncIntervalMinutes = int.Parse(ConfigurationManager.AppSettings["SyncIntervalMinutes"] ?? "1440"); // Default to daily
            _apiKey = ConfigurationManager.AppSettings["ApiKey"] ?? "";

            // Set up event log
            if (!EventLog.SourceExists("SalesforceSync"))
            {
                EventLog.CreateEventSource("SalesforceSync", "Application");
            }
            _eventLog = new EventLog
            {
                Source = "SalesforceSync",
                Log = "Application"
            };
        }

        protected override void OnStart(string[] args)
        {
            _eventLog.WriteEntry("Salesforce Sync Service starting...", EventLogEntryType.Information);

            // Initialize HTTP client
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _apiKey);
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);

            // Set up timer for periodic synchronization
            _syncTimer = new Timer
            {
                Interval = _syncIntervalMinutes * 60 * 1000, // Convert minutes to milliseconds
                AutoReset = true,
                Enabled = true
            };
            _syncTimer.Elapsed += SyncTimerElapsed;

            // Start the first sync immediately
            Task.Run(() => PerformSync());

            _eventLog.WriteEntry($"Salesforce Sync Service started. Sync interval: {_syncIntervalMinutes} minutes", EventLogEntryType.Information);
        }

        private void SyncTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isSyncRunning)
            {
                Task.Run(() => PerformSync());
            }
            else
            {
                _eventLog.WriteEntry("Skipping sync because previous sync is still running", EventLogEntryType.Warning);
            }
        }

        private async Task PerformSync()
        {
            try
            {
                _isSyncRunning = true;
                _eventLog.WriteEntry("Starting Salesforce synchronization...", EventLogEntryType.Information);

                // Get all users that need to be synced
                // In a real app, you might want to get only users with Salesforce integration enabled
                var usersResponse = await _httpClient.GetAsync("users");
                if (!usersResponse.IsSuccessStatusCode)
                {
                    _eventLog.WriteEntry($"Failed to get users: {usersResponse.StatusCode}", EventLogEntryType.Error);
                    return;
                }

                var usersContent = await usersResponse.Content.ReadAsStringAsync();
                var usersApiResponse = JsonConvert.DeserializeObject<ApiResponse<User[]>>(usersContent);

                if (usersApiResponse.Success && usersApiResponse.Data != null)
                {
                    foreach (var user in usersApiResponse.Data)
                    {
                        await SyncUserData(user.UserID);
                    }
                }

                _eventLog.WriteEntry("Salesforce synchronization completed successfully", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry($"Error during Salesforce sync: {ex.Message}", EventLogEntryType.Error);
            }
            finally
            {
                _isSyncRunning = false;
            }
        }

        private async Task SyncUserData(int userId)
        {
            try
            {
                _eventLog.WriteEntry($"Syncing data for user {userId}...", EventLogEntryType.Information);

                // Call each of our sync endpoints
                await SyncCategories(userId);
                await SyncGoals(userId);
                await SyncPaymentMethods(userId);
                await SyncTransactions(userId);
                await SyncInvestments(userId);

                _eventLog.WriteEntry($"Sync completed for user {userId}", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry($"Error syncing user {userId}: {ex.Message}", EventLogEntryType.Error);
            }
        }

        private async Task SyncCategories(int userId)
        {
            var response = await _httpClient.PostAsync($"salesforce/sync-categories?userId={userId}", null);
            if (!response.IsSuccessStatusCode)
            {
                _eventLog.WriteEntry($"Failed to sync categories for user {userId}: {response.StatusCode}", EventLogEntryType.Error);
            }
        }

        private async Task SyncGoals(int userId)
        {
            var response = await _httpClient.PostAsync($"salesforce/sync-goals?userId={userId}", null);
            if (!response.IsSuccessStatusCode)
            {
                _eventLog.WriteEntry($"Failed to sync goals for user {userId}: {response.StatusCode}", EventLogEntryType.Error);
            }
        }

        private async Task SyncPaymentMethods(int userId)
        {
            var response = await _httpClient.PostAsync($"salesforce/sync-payment-methods?userId={userId}", null);
            if (!response.IsSuccessStatusCode)
            {
                _eventLog.WriteEntry($"Failed to sync payment methods for user {userId}: {response.StatusCode}", EventLogEntryType.Error);
            }
        }

        private async Task SyncTransactions(int userId)
        {
            var response = await _httpClient.PostAsync($"salesforce/sync-transactions?userId={userId}", null);
            if (!response.IsSuccessStatusCode)
            {
                _eventLog.WriteEntry($"Failed to sync transactions for user {userId}: {response.StatusCode}", EventLogEntryType.Error);
            }
        }

        private async Task SyncInvestments(int userId)
        {
            var response = await _httpClient.PostAsync($"salesforce/sync-investments?userId={userId}", null);
            if (!response.IsSuccessStatusCode)
            {
                _eventLog.WriteEntry($"Failed to sync investments for user {userId}: {response.StatusCode}", EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            _eventLog.WriteEntry("Salesforce Sync Service stopping...", EventLogEntryType.Information);

            _syncTimer?.Stop();
            _syncTimer?.Dispose();
            _httpClient?.Dispose();

            _eventLog.WriteEntry("Salesforce Sync Service stopped", EventLogEntryType.Information);
        }
    }
}
