using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PersonalFinanceTracker.Salesforce.Authentication;
using PersonalFinanceTracker.Salesforce.Configuration;
using PersonalFinanceTracker.Salesforce.Exceptions;

namespace PersonalFinanceTracker.Salesforce.Client
{
    public class SalesforceClient : ISalesforceClient
    {
        private readonly SalesforceAuthenticator _authenticator;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SalesforceSettings _settings;
        private readonly string _apiVersion;

        public SalesforceClient(SalesforceAuthenticator authenticator, IHttpClientFactory httpClientFactory, SalesforceSettings settings)
        {
            _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            // Use configured API version or fall back to default
            _apiVersion = !string.IsNullOrEmpty(_settings.Api?.ApiVersion)
                         ? _settings.Api.ApiVersion
                         : "v55.0";
        }

        private async Task<HttpRequestMessage> CreateAuthenticatedRequestAsync(HttpMethod method, string endpoint)
        {
            var token = await _authenticator.GetAccessTokenAsync();
            var request = new HttpRequestMessage(method, $"{token.InstanceUrl}/services/data/{_apiVersion}/{endpoint}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, endpoint);
            HttpClient httpClient = null;

            try
            {
                httpClient = _httpClientFactory.CreateClient("SalesforceApi");
                var response = await httpClient.SendAsync(request);
                return await HandleResponseAsync<T>(response);
            }
            finally
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var request = await CreateAuthenticatedRequestAsync(HttpMethod.Post, endpoint);
            var json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient httpClient = null;

            try
            {
                httpClient = _httpClientFactory.CreateClient("SalesforceApi");
                var response = await httpClient.SendAsync(request);
                return await HandleResponseAsync<T>(response);
            }
            finally
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
            }
        }

        public async Task<T> PatchAsync<T>(string endpoint, object data)
        {
            var request = await CreateAuthenticatedRequestAsync(new HttpMethod("PATCH"), endpoint);
            var json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient httpClient = null;

            try
            {
                httpClient = _httpClientFactory.CreateClient("SalesforceApi");
                var response = await httpClient.SendAsync(request);
                return await HandleResponseAsync<T>(response);
            }
            finally
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var request = await CreateAuthenticatedRequestAsync(HttpMethod.Delete, endpoint);

            HttpClient httpClient = null;

            try
            {
                httpClient = _httpClientFactory.CreateClient("SalesforceApi");
                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            finally
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
            }
        }

        public async Task<string> QueryAsync(string soql)
        {
            // URL encode the SOQL query
            var encodedQuery = Uri.EscapeDataString(soql);
            var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, $"query?q={encodedQuery}");

            HttpClient httpClient = null;

            try
            {
                httpClient = _httpClientFactory.CreateClient("SalesforceApi");
                var response = await httpClient.SendAsync(request);
                return await HandleResponseAsync<string>(response);
            }
            finally
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
            }
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new SalesforceException($"Salesforce API error: {response.StatusCode} - {content}");
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}