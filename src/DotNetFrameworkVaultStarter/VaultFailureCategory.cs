namespace DotNetFrameworkVaultStarter
{
    public enum VaultFailureCategory
    {
        Unknown = 0,
        MissingConfiguration = 1,
        Authentication = 2,
        PermissionDenied = 3,
        NotFound = 4,
        MountPointOrPath = 5,
        VaultApiError = 6,
        Network = 7
    }
}
