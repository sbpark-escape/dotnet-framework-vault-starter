# dotnet-framework-vault-starter

A practical starter/helper for introducing HashiCorp Vault into legacy .NET Framework applications running on IIS or Windows Server.

This repository is intentionally small. It provides safe defaults, copyable examples, and a migration guide for teams that need to move hardcoded secrets out of older WebForms, ASMX, Windows Service, or IIS-hosted .NET Framework applications.

## At a Glance

- **Problem:** legacy .NET Framework apps often keep connection strings and API keys in config files, transforms, scripts, or helper classes.
- **Target users:** maintainers modernizing existing IIS or Windows Server workloads before a full platform migration.
- **Target environment:** .NET Framework 4.7.2 or 4.8, ASP.NET WebForms, ASMX, Windows Services, scheduled jobs, and IIS-hosted applications.
- **Auth model:** Token for local development, AppRole for server deployments.
- **Configuration model:** environment variables first, `App.config` or `Web.config` fallback second.
- **Logging rule:** never log raw secret values; diagnostics and masking helpers return only `SET`, `EMPTY`, `MISSING`, or `MASKED`.
- **Scope:** practical starter/helper and examples for legacy environments; this does not replace Vault policy design, TLS review, deployment hardening, or a security review.

## Quick Start

Set configuration through environment variables or `App.config`, then request one key from a Vault KV v2 path.

```powershell
$env:VAULT_ADDR = "https://vault.example.invalid"
$env:VAULT_MOUNT_POINT = "secret"
$env:VAULT_AUTH_METHOD = "Token"
$env:VAULT_TOKEN = "__LOCAL_DEV_TOKEN__"
```

```csharp
using DotNetFrameworkVaultStarter;

var provider = VaultSecretProviderFactory.Create();
var connectionString = provider.GetSecret("legacy-app/database", "connectionString");

System.Diagnostics.Trace.TraceInformation(
    "connectionString: " + VaultDiagnostics.Status(connectionString));
```

The log line above prints `SET`, `EMPTY`, or `MISSING`, not the connection string.

## The Problem This Solves

Many long-lived .NET Framework applications still keep database passwords, API keys, or connection strings in:

- `Web.config` or `App.config`
- checked-in transform files
- deployment scripts
- server-local notes
- static helper classes

Moving those systems to .NET Core or ASP.NET Core first is not always realistic. This starter gives teams a small bridge:

- read Vault settings from environment variables first
- fall back to `App.config` or `Web.config`
- use Token auth for local development
- use AppRole auth for IIS or Windows Server
- avoid logging secret values
- provide IIS-focused diagnostics

## Target Users

Use this repo if you maintain a legacy .NET Framework application and need a small, understandable path from hardcoded secrets to Vault-backed lookup.

This is especially useful when:

- the application cannot move to .NET Core or ASP.NET Core yet
- the team needs IIS-specific environment variable guidance
- operators prefer AppRole on Windows Server
- developers need Token auth locally without changing the server auth pattern

## Why .NET Framework and IIS

This project targets .NET Framework 4.7.2 because that is a common baseline for legacy IIS workloads and can be referenced by .NET Framework 4.8 applications. The goal is to help teams improve secret handling before a platform migration is complete.

Target scenarios:

- ASP.NET WebForms
- ASMX services
- Windows Services
- scheduled jobs
- IIS-hosted .NET Framework applications

## Package Compatibility

Vault access is implemented with [VaultSharp](https://www.nuget.org/packages/VaultSharp).

Compatibility check, performed on 2026-06-12:

- NuGet lists VaultSharp `1.17.5.1` as the latest available version checked for this starter.
- The package includes .NET Framework-compatible assets, including `net472` and `net48`.
- This repo uses a lower bound of `[1.17.5.1,2.0.0)` because that version was verified to include .NET Framework assets for the target environments.

For a real application, pin an exact VaultSharp version after testing it against your Vault server, TLS settings, proxy rules, and deployment process.

## Installation

Until this starter is published as a package, use a project reference or copy the helper project into your solution.

```powershell
git clone https://github.com/YOUR-ORG/dotnet-framework-vault-starter.git
cd dotnet-framework-vault-starter
dotnet build .\dotnet-framework-vault-starter.sln
```

From an SDK-style .NET Framework project:

```powershell
dotnet add .\YourLegacyApp.csproj reference .\src\DotNetFrameworkVaultStarter\DotNetFrameworkVaultStarter.csproj
```

For older non-SDK WebForms or ASMX projects, add `src/DotNetFrameworkVaultStarter` as an existing project in Visual Studio, then add a project reference from the legacy application.

## Configuration

Environment variables take priority over `App.config` or `Web.config`.

| Purpose | Environment variable | App.config key |
| --- | --- | --- |
| Vault address | `VAULT_ADDR` | `Vault:Address` |
| KV mount point | `VAULT_MOUNT_POINT` | `Vault:MountPoint` |
| Auth method | `VAULT_AUTH_METHOD` | `Vault:AuthMethod` |
| Local token | `VAULT_TOKEN` | `Vault:Token` |
| AppRole role id | `VAULT_ROLE_ID` | `Vault:RoleId` |
| AppRole secret id | `VAULT_SECRET_ID` | `Vault:SecretId` |

Supported auth method values:

- `Token`
- `AppRole`

Default mount point:

- `secret`

This starter reads Vault KV v2 paths. If the mount point is `secret` and the path is `legacy-app/database`, VaultSharp reads from the KV v2 API behind that mount.

## Local Token Auth

Token auth is acceptable for local development when the token is short-lived and scoped to non-production data.

PowerShell example with dummy values:

```powershell
$env:VAULT_ADDR = "https://vault.example.invalid"
$env:VAULT_MOUNT_POINT = "secret"
$env:VAULT_AUTH_METHOD = "Token"
$env:VAULT_TOKEN = "__LOCAL_DEV_TOKEN__"
```

`App.config` fallback:

```xml
<appSettings>
  <add key="Vault:Address" value="https://vault.example.invalid" />
  <add key="Vault:MountPoint" value="secret" />
  <add key="Vault:AuthMethod" value="Token" />
  <add key="Vault:Token" value="__LOCAL_DEV_TOKEN__" />
</appSettings>
```

Do not commit a real token.

## AppRole Auth for IIS and Windows Server

For server environments, prefer AppRole:

```powershell
[Environment]::SetEnvironmentVariable("VAULT_ADDR", "https://vault.example.invalid", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_MOUNT_POINT", "secret", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_AUTH_METHOD", "AppRole", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_ROLE_ID", "__ROLE_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_SECRET_ID", "__SECRET_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
```

After changing machine-level environment variables, recycle the IIS application pool or restart the Windows Service. In some deployments, a full IIS service restart is required before worker processes see new machine-level variables.

## IIS Environment Variable Notes

IIS applications often fail to see environment variables because of process lifetime or identity differences.

Checklist:

- confirm whether variables were set at User, Machine, or process scope
- recycle the target application pool after changing Machine variables
- check the application pool identity
- avoid relying on an interactive administrator user's environment
- use `VaultDiagnostics.CreateReport()` only in protected diagnostics paths
- never print actual token, role_id, secret_id, or secret values

See [docs/iis-environment-variables.md](docs/iis-environment-variables.md).

Related docs:

- [AppRole auth](docs/approle-auth.md)
- [Local token auth](docs/local-token-auth.md)
- [Troubleshooting](docs/troubleshooting.md)
- [Migration from hardcoded secrets](docs/migration-from-hardcoded-secrets.md)
- [Maintainer setup](docs/maintainer-setup.md)

## Replacing a Hardcoded Connection String

Before:

```csharp
var connectionString = "__HARDCODED_CONNECTION_STRING_DO_NOT_USE__";
```

After:

```csharp
var provider = VaultSecretProviderFactory.Create();
var connectionString = provider.GetSecret("legacy-app/database", "connectionString");
```

Recommended migration shape:

1. add Vault lookup beside the existing setting
2. deploy with diagnostics enabled for configuration status only
3. switch one non-critical secret to Vault
4. remove the hardcoded value after rollback confidence is acceptable

See [docs/migration-from-hardcoded-secrets.md](docs/migration-from-hardcoded-secrets.md).

## Diagnostics

```csharp
var report = VaultDiagnostics.CreateReport();
System.Diagnostics.Trace.TraceInformation(report);
```

Diagnostics include:

- current process user
- whether each supported environment variable is `SET`, `EMPTY`, or `MISSING`
- whether each App.config key is `SET`, `EMPTY`, or `MISSING`
- missing Vault address warning
- missing token, role id, or secret id warning
- sanitized guidance for permission denied, not found, mount point, auth, and network failures

Diagnostics do not include secret values. Helper masking functions return `MASKED` for non-empty secret values.

## Examples

- [Examples overview](examples/README.md)
- [ConsoleApp](examples/ConsoleApp/README.md)
- [Legacy WebForms or ASMX style](examples/LegacyWebFormsExample/README.md)
- [Windows Service style](examples/WindowsServiceExample/README.md)

## Tests

The tests do not call a real Vault server.

```powershell
dotnet run --project .\tests\DotNetFrameworkVaultStarter.Tests\DotNetFrameworkVaultStarter.Tests.csproj
```

Covered:

- option loading
- environment variable priority
- App.config fallback
- secret masking
- diagnostics redaction

## Known Limitations

- KV v2 is assumed.
- No built-in secret caching is included yet.
- No secret rotation watcher is included.
- TLS trust, proxy configuration, and Vault policy design remain application and infrastructure responsibilities.
- AppRole secret_id delivery is intentionally not solved here; use your deployment tooling or a secure bootstrap process.
- This starter does not replace a security review.

## Roadmap

Short version:

- `v0.1.0`: initial starter release.
- `v0.1.1`: documentation and troubleshooting improvements.
- `v0.2.0`: more legacy app examples and testing improvements.
- Future: evaluate NuGet packaging, KV v2 helper improvements, and a local dev Vault docker-compose example.

See [docs/roadmap.md](docs/roadmap.md) for the working roadmap.

## License

MIT. See [LICENSE](LICENSE).
