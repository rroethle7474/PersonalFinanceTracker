using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PersonalFinanceTracker.Salesforce.Exceptions;

namespace PersonalFinanceTracker.Salesforce.Authentication
{
    public class SalesforceAuthenticator
    {
        private readonly OAuthConfig _config;
        private TokenResponse _currentToken;
        private readonly IHttpClientFactory _httpClientFactory;

        public SalesforceAuthenticator(OAuthConfig config, IHttpClientFactory httpClientFactory)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<TokenResponse> GetAccessTokenAsync()
        {
            // Check if we have a valid token
            if (_currentToken != null && DateTime.UtcNow < _currentToken.ExpiresAt)
            {
                return _currentToken;
            }

            // Need to get a new token
            var formData = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "client_id", _config.ClientId },
                    { "client_secret", _config.ClientSecret },
                    { "username", _config.Username },
                    { "password", _config.Password + _config.SecurityToken }
                };

            var request = new HttpRequestMessage(HttpMethod.Post, _config.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(formData)
            };

            var httpClient = _httpClientFactory.CreateClient("SalesforceAuth");
            try
            {
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new SalesforceException($"Failed to authenticate: {content}");
                }

                _currentToken = JsonConvert.DeserializeObject<TokenResponse>(content);

                // Set expiration time (tokens typically last for 2 hours)
                _currentToken.ExpiresAt = DateTime.UtcNow.AddHours(2);

                return _currentToken;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}