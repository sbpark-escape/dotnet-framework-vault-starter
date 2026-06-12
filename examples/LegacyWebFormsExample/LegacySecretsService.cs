using System.Web.Services;
using DotNetFrameworkVaultStarter;

namespace LegacyWebFormsExample
{
    [WebService(Namespace = "https://example.invalid/legacy")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public sealed class LegacySecretsService : WebService
    {
        [WebMethod]
        public string GetConnectionStringStatus()
        {
            var connectionString = new LegacyConnectionFactory().GetConnectionString();
            return VaultDiagnostics.Status(connectionString);
        }
    }
}
