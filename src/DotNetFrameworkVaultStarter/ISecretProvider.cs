using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetFrameworkVaultStarter
{
    /// <summary>
    /// Reads application secrets from a backing store without exposing the backing store to legacy callers.
    /// </summary>
    public interface ISecretProvider
    {
        string GetSecret(string path, string key);

        IReadOnlyDictionary<string, string> GetSecrets(string path);

        Task<string> GetSecretAsync(string path, string key, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyDictionary<string, string>> GetSecretsAsync(string path, CancellationToken cancellationToken = default(CancellationToken));
    }
}
