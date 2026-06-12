# Security Policy

## Supported Versions

This project is pre-1.0. Security fixes will be applied to the latest released version when practical.

## Reporting a Vulnerability

Please do not open a public issue for a vulnerability that could expose secrets or weaken authentication guidance.

Use GitHub Security Advisories when available, or contact the maintainers through the private channel listed on the repository home page.

## What Counts as Security-Sensitive

- logging a secret value
- printing token, role_id, or secret_id values
- unsafe AppRole guidance
- examples that encourage committing secrets
- diagnostics that reveal more than configuration status
- dependency issues that affect Vault authentication or TLS behavior

Do not include Vault tokens, RoleId, SecretId, internal URLs, connection strings, or any real secrets in issues, pull requests, logs, screenshots, or examples.

## Project Scope

This starter helps with safe defaults and practical documentation. It does not replace:

- Vault policy review
- TLS configuration review
- deployment secret handling
- incident response
- application threat modeling
