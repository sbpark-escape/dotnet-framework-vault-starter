using DotNetFrameworkVaultStarter;

namespace LegacyWebFormsExample
{
    public sealed class LegacyConnectionFactory
    {
        public string GetConnectionString()
        {
            var provider = VaultSecretProviderFactory.Create();
            return provider.GetSecret("legacy-webforms/database", "connectionString");
        }
    }
}
