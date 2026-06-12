# Contributing

Thanks for helping improve this starter.

## Project Goals

- Help legacy .NET Framework teams adopt Vault safely and incrementally.
- Keep the helper small and easy to copy into existing solutions.
- Prefer clear docs and examples over broad abstraction.
- Never include real tokens, role ids, secret ids, Vault addresses, company names, or internal domains.

## Development Environment

Recommended setup:

- Windows
- .NET SDK 8.0 or newer
- .NET Framework 4.7.2 targeting pack
- PowerShell

Clone and restore:

```powershell
git clone https://github.com/YOUR-ORG/dotnet-framework-vault-starter.git
cd dotnet-framework-vault-starter
dotnet restore .\dotnet-framework-vault-starter.sln
```

## Build

```powershell
dotnet build .\dotnet-framework-vault-starter.sln --no-restore
```

## Tests

The default tests do not require a real Vault server.

```powershell
dotnet run --project .\tests\DotNetFrameworkVaultStarter.Tests\DotNetFrameworkVaultStarter.Tests.csproj
```

## Documentation Changes

For docs-only changes, run the markdown link check:

```powershell
.\scripts\Test-MarkdownLinks.ps1
```

When editing docs or examples:

- use dummy values only
- prefer `https://vault.example.invalid` for fake Vault addresses
- use `__DUMMY_...__` style placeholders for tokens and AppRole values
- link related docs when adding a new operational note
- avoid promising behavior the starter does not implement

## Choosing a Good Issue

Good first contributions are usually:

- README or docs clarification
- troubleshooting additions from a sanitized scenario
- example README improvements
- tests for configuration loading or validation
- small sample additions that do not require real Vault credentials

Avoid starting with broad architecture changes. Open an issue first for changes involving auth behavior, caching, package publishing, or new dependencies.

## Pull Requests

Please include:

- what problem the change solves
- how it was tested
- whether it changes security behavior
- any docs updates needed for users

Small focused PRs are easier to review.

Before opening a PR:

- run build when code or project files changed
- run tests when code or behavior changed
- run markdown link validation when docs changed
- check that no real secrets or internal URLs are included
- update README or docs links when adding a new document or example

## Security-Sensitive Changes

Changes touching authentication, logging, masking, diagnostics, or secret handling need extra care. The default rule is simple: secret values must not be logged, printed, committed, or included in test fixtures.

Never include:

- real Vault addresses
- real tokens
- real RoleId or SecretId values
- real connection strings
- internal hostnames or domains
- company-specific deployment details
