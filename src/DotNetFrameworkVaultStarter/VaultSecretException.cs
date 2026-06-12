using System;

namespace DotNetFrameworkVaultStarter
{
    public sealed class VaultSecretException : Exception
    {
        public VaultSecretException(string message, VaultFailureCategory category)
            : base(message)
        {
            Category = category;
        }

        public VaultSecretException(string message, VaultFailureCategory category, Exception innerException)
            : base(message, innerException)
        {
            Category = category;
        }

        public VaultFailureCategory Category { get; private set; }
    }
}
