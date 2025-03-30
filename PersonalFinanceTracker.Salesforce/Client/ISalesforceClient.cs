using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Salesforce.Client
{
    public interface ISalesforceClient
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<T> PatchAsync<T>(string endpoint, object data);
        Task<bool> DeleteAsync(string endpoint);
        Task<string> QueryAsync(string soql);
    }
}
