using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace PersonalFinanceTracker.SyncService
{
    [RunInstaller(true)]
    public class SalesforceSyncInstaller : Installer
    {
        public SalesforceSyncInstaller()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            var serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Manual,
                ServiceName = "PersonalFinanceTrackerSync",
                DisplayName = "Personal Finance Tracker Salesforce Sync",
                Description = "Synchronizes Personal Finance Tracker data with Salesforce CRM"
            };

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
