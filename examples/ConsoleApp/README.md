# ConsoleApp Example

Use this example for a legacy console application, scheduled job, or one-off maintenance executable that runs on .NET Framework 4.7.2 or 4.8. It shows startup-time option loading, safe diagnostics, and a single KV v2 lookup shape.

## Assumed Environment

- .NET Framework 4.7.2 or 4.8
- Console application or scheduled task
- Local development can use Token auth
- Server execution should normally use AppRole auth

## Required Environment Variables

For local Token auth with dummy values:

```powershell
$env:VAULT_ADDR = "https://vault.example.invalid"
$env:VAULT_MOUNT_POINT = "secret"
$env:VAULT_AUTH_METHOD = "Token"
$env:VAULT_TOKEN = "__LOCAL_DEV_TOKEN__"
```

For AppRole:

```powershell
$env:VAULT_ADDR = "https://vault.example.invalid"
$env:VAULT_MOUNT_POINT = "secret"
$env:VAULT_AUTH_METHOD = "AppRole"
$env:VAULT_ROLE_ID = "__ROLE_ID_FROM_SECURE_DEPLOYMENT__"
$env:VAULT_SECRET_ID = "__SECRET_ID_FROM_SECURE_DEPLOYMENT__"
```

## App.config Fallback

The sample includes [App.config](App.config) with dummy Token values. Environment variables override these values.

```xml
<appSettings>
  <add key="Vault:Address" value="https://vault.example.invalid" />
  <add key="Vault:MountPoint" value="secret" />
  <add key="Vault:AuthMethod" value="Token" />
  <add key="Vault:Token" value="__LOCAL_DEV_TOKEN__" />
</appSettings>
```

## Execution Flow

1. `VaultOptions.Load()` reads environment variables first.
2. Missing environment values fall back to `App.config`.
3. `VaultDiagnostics.CreateReport()` prints configuration status only.
4. `VaultSecretProviderFactory.Create(options)` validates required settings.
5. `GetSecret("legacy-console/database", "connectionString")` reads a dummy-shaped KV v2 path.
6. The example logs only `VaultDiagnostics.Status(connectionString)`.

## Notes

- The example compiles without Vault.
- Running it with the checked-in dummy `VAULT_ADDR` will not retrieve a real secret.
- Do not print the returned connection string.
- For scheduled tasks, set variables for the account that runs the task or use machine-level variables and restart the process.
