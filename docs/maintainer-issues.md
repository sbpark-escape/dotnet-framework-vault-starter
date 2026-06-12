# Maintainer Issue Backlog

These issue drafts are designed to be copied into GitHub Issues. Keep the scope small, use dummy values only, and avoid internal environment details.

## 1. README Clarity Improvement

- **Title:** Improve README first-screen clarity for legacy .NET Framework/IIS users
- **Why this matters:** New visitors should understand the repository purpose within 30 seconds.
- **Scope:** Review the opening sections and make the target users, supported environments, auth modes, and safety boundaries easier to scan.
- **Acceptance criteria:** README explains the project purpose, target users, supported environments, quick start, and safety notes without overclaiming.
- **Suggested labels:** `documentation`, `good first issue`
- **Difficulty:** Easy

## 2. IIS Troubleshooting Expansion

- **Title:** Expand IIS App Pool environment variable troubleshooting
- **Why this matters:** IIS environment variable visibility is a common source of Vault adoption failures.
- **Scope:** Add sanitized troubleshooting notes for App Pool identity, process restarts, machine/user variables, and diagnostics output.
- **Acceptance criteria:** Troubleshooting doc includes actionable checks and no real hostnames or secrets.
- **Suggested labels:** `documentation`, `help wanted`
- **Difficulty:** Easy

## 3. WebForms Example Improvement

- **Title:** Improve WebForms-style example documentation
- **Why this matters:** Many target users maintain classic WebForms applications.
- **Scope:** Add a clearer README flow for where to call the helper from pages, handlers, or existing data-access classes.
- **Acceptance criteria:** Example remains Vault-free; README includes environment variables, fallback config, execution flow, and warnings.
- **Suggested labels:** `example`, `documentation`
- **Difficulty:** Easy

## 4. ASMX Example Improvement

- **Title:** Add or improve ASMX service usage example
- **Why this matters:** ASMX services are common in older .NET Framework estates.
- **Scope:** Add a minimal ASMX-style example or expand the existing sample so safe status-only output is obvious.
- **Acceptance criteria:** Example compiles; it does not return real secret values; docs explain dummy configuration only.
- **Suggested labels:** `example`, `good first issue`
- **Difficulty:** Easy

## 5. Windows Service Example Improvement

- **Title:** Improve Windows Service startup and diagnostics example
- **Why this matters:** Windows Services have different process lifetime and environment variable behavior from web apps.
- **Scope:** Add notes or code comments for startup lookup, service restarts, and safe logging.
- **Acceptance criteria:** Example logs only status; README explains service account environment and restart behavior.
- **Suggested labels:** `example`, `documentation`
- **Difficulty:** Easy

## 6. AppRole Diagnostics Improvement

- **Title:** Improve AppRole missing-value diagnostics
- **Why this matters:** AppRole failures often come from a missing or empty `role_id` or `secret_id`.
- **Scope:** Add tests or docs that clarify `SET`, `EMPTY`, and `MISSING` behavior for AppRole settings.
- **Acceptance criteria:** Tests pass without Vault; docs explain how to interpret `VAULT_ROLE_ID=SET` and `VAULT_SECRET_ID=EMPTY`.
- **Suggested labels:** `enhancement`, `documentation`
- **Difficulty:** Medium

## 7. Secret Masking Test Addition

- **Title:** Add more tests for secret masking and diagnostics redaction
- **Why this matters:** Safe logging is one of the main security hygiene goals of this starter.
- **Scope:** Add tests for whitespace values, dictionary masking, and diagnostic reports that include config keys but not values.
- **Acceptance criteria:** Tests do not use real secrets; all new tests pass without a Vault server.
- **Suggested labels:** `good first issue`, `bug`
- **Difficulty:** Easy

## 8. KV v1 and KV v2 Path Clarification

- **Title:** Clarify KV v1 and KV v2 path behavior
- **Why this matters:** KV path shape confusion causes many `path not found` and mount point errors.
- **Scope:** Add a README or troubleshooting section explaining mount point, application path, and policy path differences.
- **Acceptance criteria:** Docs state that this starter assumes KV v2 and show dummy path examples only.
- **Suggested labels:** `documentation`, `good first issue`
- **Difficulty:** Easy

## 9. Local Vault Dev Server Documentation

- **Title:** Add local Vault dev server documentation with dummy data
- **Why this matters:** Users may want a safe way to try the examples without production Vault access.
- **Scope:** Add a doc for local Vault dev mode or docker-compose, using dummy secrets only.
- **Acceptance criteria:** Default tests remain Vault-free; docs warn that dev mode is not for production; examples use dummy values only.
- **Suggested labels:** `documentation`, `example`, `help wanted`
- **Difficulty:** Medium

## 10. Release Checklist Automation

- **Title:** Improve release checklist automation in GitHub Actions
- **Why this matters:** Small automated checks reduce release mistakes in a public repository.
- **Scope:** Add or improve checks for markdown links, build, tests, and possibly secret-pattern scanning.
- **Acceptance criteria:** CI remains simple; checks run on pull requests; documentation explains the command locally.
- **Suggested labels:** `enhancement`, `help wanted`
- **Difficulty:** Medium
