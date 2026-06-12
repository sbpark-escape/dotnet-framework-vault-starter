# Codex for OSS Readiness

This document is a maintainer note for evaluating whether this repository is ready to be described as an actively maintained public OSS project. It is not an application draft and should not overstate adoption, security maturity, or ecosystem impact.

## Repository Purpose

`dotnet-framework-vault-starter` is a practical starter/helper for introducing HashiCorp Vault into legacy .NET Framework applications that run on IIS or Windows Server.

The repository focuses on small, copyable patterns for teams that need to reduce hardcoded secrets before a full platform migration is possible.

## Practical Problem

Many organizations still operate applications built with:

- .NET Framework 4.7.2 or 4.8
- ASP.NET WebForms
- ASMX services
- Windows Services
- IIS-hosted applications
- scheduled jobs or console utilities on Windows Server

These applications often keep connection strings, API keys, or service credentials in `Web.config`, `App.config`, transforms, deployment scripts, or helper classes. Vault adoption guidance often assumes newer application stacks, while legacy IIS environments have practical issues around App Pool identity, Windows environment variables, AppRole bootstrap, VaultSharp compatibility, and safe diagnostics.

This repository addresses that gap with a starter approach rather than a broad security framework.

## Target Users

- .NET Framework maintainers reducing hardcoded secrets in existing applications.
- IIS and Windows Server operators supporting legacy application deployments.
- Teams introducing HashiCorp Vault into systems that cannot migrate to .NET Core or ASP.NET Core yet.
- Developers who need local Token auth examples and server-side AppRole examples.
- Maintainers who need safe diagnostics without exposing secret values.

## Ecosystem Value

The .NET ecosystem still includes many long-lived .NET Framework applications. Those systems may not be highly visible in modern package metrics, but they remain common in internal business software. A small starter focused on Vault usage in IIS and Windows Server can help teams make incremental security improvements without requiring a rewrite.

The value is practical:

- concrete examples for legacy hosting models
- configuration guidance for environment variables and App.config fallback
- safe logging and diagnostics language
- troubleshooting notes for IIS-specific failure modes
- a narrow path for moving away from hardcoded secrets

## Current Repository Capabilities

- `net472` helper library usable from .NET Framework 4.7.2 and 4.8 applications.
- Token auth for local development.
- AppRole auth for server-oriented deployments.
- Environment-variable-first configuration with App.config/Web.config fallback.
- Safe diagnostics using `SET`, `EMPTY`, `MISSING`, and `MASKED`.
- Console, WebForms/ASMX-style, and Windows Service-style examples.
- Vault-free tests for configuration loading, validation, unsupported auth methods, and diagnostics redaction.
- GitHub Actions CI for build, tests, and markdown link validation.
- OSS basics: README, LICENSE, SECURITY, CONTRIBUTING, CODE_OF_CONDUCT, issue templates, PR template, changelog, roadmap.

## Current Gaps

- No published NuGet package yet.
- No default CI integration test against a real Vault server.
- KV v2 is assumed; KV v1 is documented as a limitation.
- No AppRole `secret_id` delivery mechanism is implemented.
- No caching or rotation helper is included.
- TLS/proxy/certificate troubleshooting can be expanded further.
- Classic non-SDK WebForms and `packages.config` examples can be improved.

## Maintenance Plan Through July

The maintenance plan should remain small and realistic:

- `v0.1.0`: initial public starter release.
- `v0.1.1`: documentation and troubleshooting cleanup.
- `v0.1.2`: example and test improvements.
- `v0.2.0`: expanded legacy app scenarios and local Vault dev guide.

Useful maintenance activity includes:

- opening and triaging good first issues
- improving docs based on first-reader questions
- adding narrowly scoped examples
- adding tests for config and diagnostics behavior
- keeping release notes clear and factual
- reviewing security hygiene before each release

## Codex-Suitable Maintenance Tasks

Codex can be useful for:

- issue triage
- documentation improvement
- pull request review
- release note generation
- troubleshooting guide expansion
- test coverage improvement
- examples expansion
- security hygiene review

Codex should not replace human judgment for Vault policy design, production security review, incident response, or deployment approval.

## Expressions to Avoid in an Application

Avoid:

- `production-ready`
- `enterprise-grade`
- `perfect security`
- `complete Vault integration`
- `drop-in security solution`
- `widely used`
- `critical infrastructure`
- invented stars, downloads, or user counts

## Factual Phrases That Are Safe to Emphasize

Prefer:

- `practical starter/helper`
- `migration guide for legacy environments`
- `examples for .NET Framework/IIS teams`
- `safe defaults for logging and diagnostics`
- `addresses a practical gap for legacy .NET Framework/IIS teams adopting Vault`
- `early public repository with an active maintenance plan`
- `default tests do not require a real Vault server`
- `maintainer is using Codex to improve docs, tests, examples, and release hygiene`
