using System;
using Microsoft.Extensions.Logging;

namespace PersonalFinanceTracker.Salesforce.Utilities
{
    /// <summary>
    /// Provides logging functionality for Salesforce integration.
    /// </summary>
    public class SalesforceLogger
    {
        private readonly ILogger _logger;
        private readonly string _component;

        /// <summary>
        /// Creates a new SalesforceLogger.
        /// </summary>
        /// <param name="logger">The ILogger implementation to use.</param>
        /// <param name="component">The component name for contextual logging.</param>
        public SalesforceLogger(ILogger logger, string component)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _component = component ?? throw new ArgumentNullException(nameof(component));
        }

        /// <summary>
        /// Logs an authentication attempt.
        /// </summary>
        public void LogAuthenticationAttempt(string username)
        {
            _logger.LogInformation("[Salesforce:{Component}] Authentication attempt for user: {Username}",
                _component, MaskUsername(username));
        }

        /// <summary>
        /// Logs a successful authentication.
        /// </summary>
        public void LogAuthenticationSuccess(string username)
        {
            _logger.LogInformation("[Salesforce:{Component}] Authentication successful for user: {Username}",
                _component, MaskUsername(username));
        }

        /// <summary>
        /// Logs an API request.
        /// </summary>
        public void LogApiRequest(string method, string endpoint)
        {
            _logger.LogDebug("[Salesforce:{Component}] {Method} request to: {Endpoint}",
                _component, method, endpoint);
        }

        /// <summary>
        /// Logs a successful API response.
        /// </summary>
        public void LogApiResponse(string method, string endpoint, int statusCode)
        {
            _logger.LogDebug("[Salesforce:{Component}] {Method} request to {Endpoint} completed with status: {StatusCode}",
                _component, method, endpoint, statusCode);
        }

        /// <summary>
        /// Logs a sync operation start.
        /// </summary>
        public void LogSyncStart(string objectType, int userId)
        {
            _logger.LogInformation("[Salesforce:{Component}] Starting sync of {ObjectType} for user {UserId}",
                _component, objectType, userId);
        }

        /// <summary>
        /// Logs a sync operation completion.
        /// </summary>
        public void LogSyncComplete(string objectType, int userId, int itemsProcessed)
        {
            _logger.LogInformation("[Salesforce:{Component}] Completed sync of {ObjectType} for user {UserId}: {ItemCount} items processed",
                _component, objectType, userId, itemsProcessed);
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        public void LogError(Exception ex, string context)
        {
            _logger.LogError(ex, "[Salesforce:{Component}] Error in {Context}: {Message}",
                _component, context, ex.Message);
        }

        /// <summary>
        /// Masks a username for logging by showing only the first few characters.
        /// </summary>
        private string MaskUsername(string username)
        {
            if (string.IsNullOrEmpty(username) || username.Length <= 4)
            {
                return "****";
            }

            return username.Substring(0, 3) + "***";
        }
    }
}