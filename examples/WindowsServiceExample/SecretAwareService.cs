using System.Diagnostics;
using System.ServiceProcess;
using DotNetFrameworkVaultStarter;

namespace WindowsServiceExample
{
    public sealed class SecretAwareService : ServiceBase
    {
        private string _connectionString;

        protected override void OnStart(string[] args)
        {
            var provider = VaultSecretProviderFactory.Create();
            _connectionString = provider.GetSecret("legacy-service/database", "connectionString");

            Trace.TraceInformation("Vault connectionString status: " + VaultDiagnostics.Status(_connectionString));
        }

        protected override void OnStop()
        {
            _connectionString = null;
        }
    }
}
