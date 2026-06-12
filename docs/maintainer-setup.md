# Maintainer Setup

Use this document when preparing a local development environment, checking release readiness, or looking for small contributor-friendly issues. It is written for maintainers and contributors, not application operators.

## Local Build

Requirements:

- Windows
- .NET SDK 8.0 or newer
- .NET Framework 4.7.2 targeting pack

Commands:

```powershell
dotnet restore .\dotnet-framework-vault-starter.sln
dotnet build .\dotnet-framework-vault-starter.sln --no-restore
dotnet run --project .\tests\DotNetFrameworkVaultStarter.Tests\DotNetFrameworkVaultStarter.Tests.csproj
.\scripts\Test-MarkdownLinks.ps1
```

## Release Checklist for v0.1.0

- verify README quick start
- run local build and tests
- run markdown link validation
- check examples compile
- check all docs use dummy values only
- update `CHANGELOG.md`
- tag `v0.1.0`
- create GitHub release notes from the changelog

See [roadmap.md](roadmap.md) for the current release plan through July.

## GitHub Issue Candidates

### 1. Add XML Documentation for Public APIs

- **Title:** Add XML documentation comments to public helper APIs
- **Description:** Add concise XML comments for public types and methods in `src/DotNetFrameworkVaultStarter` so generated package docs are easier to browse.
- **Acceptance criteria:** All public APIs have useful XML comments; comments do not repeat obvious code; `dotnet build` remains warning-free.
- **Suggested labels:** `good first issue`, `documentation`
- **Difficulty:** Easy

### 2. Add Classic packages.config WebForms Sample

- **Title:** Add a classic WebForms sample using packages.config
- **Description:** Many legacy apps are not SDK-style projects. Add a minimal sample or guide showing how to reference the helper from a classic WebForms project.
- **Acceptance criteria:** Sample or doc explains project reference setup; includes dummy config only; includes IIS notes; does not require a real Vault server.
- **Suggested labels:** `example`, `help wanted`
- **Difficulty:** Medium

### 3. Add Optional In-Memory Secret Cache

- **Title:** Add optional in-memory cache wrapper with TTL
- **Description:** Add a small optional wrapper around `ISecretProvider` that caches retrieved values for a configurable TTL.
- **Acceptance criteria:** Cache is opt-in; no secret values are logged; expiration behavior is covered by tests; README explains when not to use caching.
- **Suggested labels:** `enhancement`, `help wanted`
- **Difficulty:** Medium

### 4. Document KV v1 Support Strategy

- **Title:** Document KV v1 support options
- **Description:** The starter assumes KV v2. Add documentation that explains this clearly and outlines possible approaches for KV v1 users.
- **Acceptance criteria:** Troubleshooting and README link to the new section; examples remain KV v2; no unsupported code path is implied.
- **Suggested labels:** `documentation`, `good first issue`
- **Difficulty:** Easy

### 5. Add Local Vault Integration Test Workflow

- **Title:** Add optional local Vault integration tests
- **Description:** Add integration tests that run against a local Vault dev server or container while keeping the default test suite Vault-free.
- **Acceptance criteria:** Default CI still runs without real secrets; integration workflow uses dummy data; docs explain how to run it locally.
- **Suggested labels:** `enhancement`, `help wanted`
- **Difficulty:** Hard

### 6. Add Windows Task Scheduler Example

- **Title:** Add Windows Task Scheduler usage example
- **Description:** Add documentation or a small example for scheduled jobs that run under a service account.
- **Acceptance criteria:** Covers environment variable scope; includes dummy values; logs only status; links from `examples/README.md`.
- **Suggested labels:** `example`, `documentation`
- **Difficulty:** Easy

### 7. Expand TLS and Certificate Troubleshooting

- **Title:** Expand Windows Server TLS and certificate troubleshooting
- **Description:** Add deeper guidance for certificate trust, proxy settings, and service account certificate store behavior.
- **Acceptance criteria:** Adds actionable checks; avoids unsafe certificate validation bypass guidance; links from `docs/troubleshooting.md`.
- **Suggested labels:** `documentation`, `help wanted`
- **Difficulty:** Medium

### 8. Add Multi-Key Secret Reading Example

- **Title:** Add example for reading multiple keys from one Vault path
- **Description:** Show how to call `GetSecrets` and safely use several related values without logging them.
- **Acceptance criteria:** Example compiles; uses dummy keys; logs only status or masked output; includes README notes.
- **Suggested labels:** `example`, `good first issue`
- **Difficulty:** Easy

## Maintainer Notes

This project should stay small. Prefer documentation, examples, and safe defaults over broad abstraction. Avoid adding dependencies unless they make legacy adoption materially easier.
