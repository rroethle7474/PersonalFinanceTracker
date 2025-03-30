using Microsoft.Extensions.Logging;
using PersonalFinanceTracker.Salesforce.Client;
using PersonalFinanceTracker.Salesforce.Mapping;
using PersonalFinanceTracker.Salesforce.Models;
using PersonalFinanceTracker.Salesforce.Services.Interfaces;
using PersonalFinanceTracker.Salesforce.Utilities;
using System;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Salesforce.Services
{
    public class SalesforceBudgetCategoryService : ISalesforceBudgetCategoryService
    {
        private readonly ISalesforceClient _client;
        private readonly SalesforceBudgetCategoryMapper _mapper;
        private readonly SalesforceLogger _logger;

        public SalesforceBudgetCategoryService(ISalesforceClient client, SalesforceBudgetCategoryMapper mapper, ILogger<SalesforceBudgetCategoryService> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = new SalesforceLogger(logger, "SalesforceBudgetCategory");
        }

        // Then use the logger throughout your methods
        public async Task<SalesforceBudgetCategory> GetByIdAsync(string salesforceId)
        {
            try
            {
                _logger.LogApiRequest("GET", $"sobjects/Budget_Category__c/{salesforceId}");
                var result = await _client.GetAsync<SalesforceBudgetCategory>($"sobjects/Budget_Category__c/{salesforceId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById({salesforceId})");
                throw;
            }
        }
    }
}
