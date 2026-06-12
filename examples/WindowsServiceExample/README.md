# Windows Service Example

Use this example for a .NET Framework Windows Service that reads configuration during service startup. It shows how to keep Vault lookup close to the service boundary and log only secret status.

## Assumed Environment

- .NET Framework 4.7.2 or 4.8
- Windows Service running under a service account
- AppRole auth on Windows Server
- Service restart after changing environment variables

## Required Environment Variables

For server execution with AppRole:

```powershell
[Environment]::SetEnvironmentVariable("VAULT_ADDR", "https://vault.example.invalid", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_MOUNT_POINT", "secret", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_AUTH_METHOD", "AppRole", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_ROLE_ID", "__ROLE_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_SECRET_ID", "__SECRET_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
```

Restart the Windows Service after changing variables. A process that is already running will not automatically see new environment values.

## App.config Fallback

The sample includes [App.config](App.config) with dummy AppRole values.

```xml
<appSettings>
  <add key="Vault:Address" value="https://vault.example.invalid" />
  <add key="Vault:MountPoint" value="secret" />
  <add key="Vault:AuthMethod" value="AppRole" />
  <add key="Vault:RoleId" value="__ROLE_ID_FROM_SECURE_DEPLOYMENT__" />
  <add key="Vault:SecretId" value="__SECRET_ID_FROM_SECURE_DEPLOYMENT__" />
</appSettings>
```

## Execution Flow

1. `OnStart` creates `VaultSecretProvider` through the factory.
2. Required settings are validated before the Vault client is used.
3. `GetSecret("legacy-service/database", "connectionString")` represents a startup secret lookup.
4. The service logs only `VaultDiagnostics.Status(_connectionString)`.
5. `OnStop` clears the in-memory field reference.

## Notes

- Service account environment differs from an interactive administrator shell.
- Machine-level variables require service restart before they are visible.
- Do not write the connection string to Event Log, trace output, exception messages, or crash dumps.
- Consider a small retry policy at the service boundary if Vault may be briefly unavailable during startup.
