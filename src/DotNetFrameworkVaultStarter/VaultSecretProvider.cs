using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Token;

namespace DotNetFrameworkVaultStarter
{
    public sealed class VaultSecretProvider : ISecretProvider
    {
        private readonly VaultOptions _options;
        private readonly IVaultClient _client;

        public VaultSecretProvider(VaultOptions options)
            : this(options, CreateClient(options))
        {
        }

        internal VaultSecretProvider(VaultOptions options, IVaultClient client)
        {
            if (options == null)
            {
                throw new VaultSecretException("Vault options are required.", VaultFailureCategory.MissingConfiguration);
            }

            if (client == null)
            {
                throw new VaultSecretException("Vault client is required.", VaultFailureCategory.MissingConfiguration);
            }

            options.ThrowIfInvalid();
            _options = options;
            _client = client;
        }

        public string GetSecret(string path, string key)
        {
            return GetSecretAsync(path, key).GetAwaiter().GetResult();
        }

        public IReadOnlyDictionary<string, string> GetSecrets(string path)
        {
            return GetSecretsAsync(path).GetAwaiter().GetResult();
        }

        public async Task<string> GetSecretAsync(string path, string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Secret key is required.", "key");
            }

            var values = await GetSecretsAsync(path, cancellationToken).ConfigureAwait(false);
            string value;
            if (!values.TryGetValue(key, out value))
            {
                throw new VaultSecretException(
                    "Secret key was not found in the Vault response.",
                    VaultFailureCategory.NotFound);
            }

            return value;
        }

        public async Task<IReadOnlyDictionary<string, string>> GetSecretsAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Vault secret path is required.", "path");
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var secret = await _client.V1.Secrets.KeyValue.V2
                    .ReadSecretAsync(path.Trim(), mountPoint: _options.MountPoint)
                    .ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                if (secret == null || secret.Data == null || secret.Data.Data == null)
                {
                    return data;
                }

                foreach (var item in secret.Data.Data)
                {
                    data[item.Key] = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                }

                return data;
            }
            catch (VaultSecretException)
            {
                throw;
            }
            catch (VaultApiException ex)
            {
                throw WrapVaultException(ex);
            }
            catch (Exception ex)
            {
                throw new VaultSecretException(
                    "Vault secret lookup failed. " + VaultDiagnostics.DescribeException(ex),
                    VaultDiagnostics.ClassifyException(ex),
                    ex);
            }
        }

        private static IVaultClient CreateClient(VaultOptions options)
        {
            if (options == null)
            {
                throw new VaultSecretException("Vault options are required.", VaultFailureCategory.MissingConfiguration);
            }

            options.ThrowIfInvalid();

            IAuthMethodInfo authMethodInfo;
            if (options.AuthMethod == VaultAuthMethod.AppRole)
            {
                authMethodInfo = new AppRoleAuthMethodInfo(options.RoleId, options.SecretId);
            }
            else
            {
                authMethodInfo = new TokenAuthMethodInfo(options.Token);
            }

            var settings = new VaultClientSettings(options.Address, authMethodInfo);
            return new VaultClient(settings);
        }

        private static VaultSecretException WrapVaultException(VaultApiException exception)
        {
            var category = VaultDiagnostics.ClassifyException(exception);
            return new VaultSecretException(
                "Vault API request failed. " + VaultDiagnostics.DescribeException(exception),
                category,
                exception);
        }
    }
}
