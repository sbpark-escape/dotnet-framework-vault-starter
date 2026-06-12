using System;
using DotNetFrameworkVaultStarter;

namespace ConsoleApp
{
    internal static class Program
    {
        private static int Main()
        {
            try
            {
                var options = VaultOptions.Load();
                Console.WriteLine(VaultDiagnostics.CreateReport());

                var secrets = VaultSecretProviderFactory.Create(options);

                // For KV v2 mounted at "secret", this reads:
                //   secret/data/legacy-console/database
                var connectionString = secrets.GetSecret("legacy-console/database", "connectionString");

                Console.WriteLine("connectionString: " + VaultDiagnostics.Status(connectionString));
                Console.WriteLine("Do not print the value itself.");
                return 0;
            }
            catch (VaultSecretException ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(VaultDiagnostics.DescribeException(ex));
                return 1;
            }
        }
    }
}
