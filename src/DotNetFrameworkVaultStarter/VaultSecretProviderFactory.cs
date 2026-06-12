namespace DotNetFrameworkVaultStarter
{
    public static class VaultSecretProviderFactory
    {
        public static ISecretProvider Create()
        {
            return Create(VaultOptions.Load());
        }

        public static ISecretProvider Create(VaultOptions options)
        {
            if (options == null)
            {
                throw new VaultSecretException("Vault options are required.", VaultFailureCategory.MissingConfiguration);
            }

            options.ThrowIfInvalid();
            return new VaultSecretProvider(options);
        }
    }
}
