# Legacy WebForms or ASMX Example

Use this example for classic ASP.NET WebForms or ASMX applications where secret access is often hidden behind helper classes or `ConfigurationManager` calls. It demonstrates a small factory that can be called from pages, handlers, ASMX services, or existing data-access code.

## Assumed Environment

- .NET Framework 4.7.2 or 4.8
- IIS-hosted WebForms or ASMX application
- AppRole auth for deployed IIS environments
- Optional protected diagnostics handler during rollout

## Required Environment Variables

For IIS or Windows Server with AppRole:

```powershell
[Environment]::SetEnvironmentVariable("VAULT_ADDR", "https://vault.example.invalid", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_MOUNT_POINT", "secret", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_AUTH_METHOD", "AppRole", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_ROLE_ID", "__ROLE_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_SECRET_ID", "__SECRET_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
```

Recycle the target application pool after changing machine-level variables.

## App.config or Web.config Fallback

The sample includes [App.config](App.config). In a real WebForms or ASMX app, the same keys can live under `Web.config` `appSettings`.

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

1. `LegacyConnectionFactory.GetConnectionString()` creates the provider.
2. The provider loads environment variables first, then config fallback values.
3. `GetSecret("legacy-webforms/database", "connectionString")` represents the existing connection string replacement point.
4. `LegacySecretsService.GetConnectionStringStatus()` returns only status, not the value.
5. `LegacyDiagnosticsHandler` shows safe diagnostics and should only be exposed behind authentication or temporary operational controls.

## Notes

- Do not expose diagnostics publicly.
- Do not return raw secrets from ASMX methods or HTTP handlers.
- IIS may need an app pool recycle, and sometimes an IIS restart, before machine-level environment changes are visible.
- Prefer environment variables or deployment-time secret injection over checked-in config values.
