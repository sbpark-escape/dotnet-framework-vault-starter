# OSS Application Draft Notes

This file contains factual notes for a future OSS support application. It is not a submission draft. Do not invent usage metrics, adoption, downloads, stars, or contributors.

## 1. Repository Summary

`dotnet-framework-vault-starter` is a practical starter/helper for legacy .NET Framework applications running on IIS or Windows Server that need to start using HashiCorp Vault.

It focuses on Token auth for local development, AppRole auth for server deployments, environment-variable-first configuration, App.config fallback, safe diagnostics, and examples for legacy application shapes.

## 2. Maintainer Role

The primary maintainer is responsible for:

- keeping examples and docs accurate
- reviewing issues and pull requests
- maintaining release notes
- improving tests and CI
- preventing real secrets from entering public docs, examples, logs, or test fixtures

## 3. Why This Repository Qualifies

The repository addresses a practical gap for legacy .NET Framework/IIS teams adopting Vault. It is public, documented, tested without a real Vault dependency, and has a maintenance plan with small issues and releases.

It should be described as early-stage and practical, not as a mature security platform.

## 4. Ecosystem Importance

Many organizations still maintain .NET Framework applications on IIS and Windows Server. These applications may need safer secret handling before they can be migrated to newer frameworks. Practical starter examples can help teams move away from hardcoded secrets incrementally.

## 5. Current Limitations

- No NuGet package yet.
- No default real Vault integration tests.
- KV v2 is assumed.
- AppRole `secret_id` delivery is not automated.
- No caching or rotation helper is included.
- More classic WebForms and Windows Server troubleshooting examples are needed.

## 6. Evidence of Active Maintenance

Use real facts only:

- initial public release: `v0.1.0` once created
- CI exists for build, tests, and markdown link validation
- issue templates and PR template exist
- roadmap and release plan exist
- good first issue backlog exists
- changelog tracks unreleased work

Add real numbers later:

- stars: `TBD, use real value at submission time`
- forks: `TBD, use real value at submission time`
- releases: `TBD, use real value at submission time`
- contributors: `TBD, use real value at submission time`

## 7. How Codex/API Credits Would Be Used

Credits could support routine maintenance:

- issue triage
- PR review
- docs improvement
- release note generation
- troubleshooting guide expansion
- test coverage improvement
- example expansion
- security hygiene review

They would not replace human review for production security decisions.

## 8. Anything Else to Mention

The repository is intentionally scoped. Its value is not broad adoption yet; its value is a clear, practical path for a common legacy environment that is often underrepresented in modern examples.

## 9. Overstated Phrases to Avoid

Avoid:

- `production-ready`
- `widely used`
- `critical infrastructure`
- `enterprise-grade`
- `complete security solution`
- `trusted by many companies`
- any fake stars, downloads, or contributor counts

## 10. 500-Word Draft Direction 1

Focus the application around the practical maintenance burden of legacy .NET Framework/IIS systems. Explain that many teams still operate WebForms, ASMX, and Windows Service applications, and that Vault adoption in those environments has specific problems around App Pool identity, environment variable visibility, App.config fallback, AppRole setup, and secret logging. Describe the repository as a starter/helper with examples and safe documentation. Emphasize that Codex would help maintain docs, triage issues, review PRs, generate release notes, and expand tests and examples. Avoid claiming broad adoption or production readiness.

## 11. 500-Word Draft Direction 2

Focus the application around active maintenance quality rather than popularity. Explain that the repository is early but structured like a maintainable OSS project: README, examples, tests, CI, issue templates, changelog, security policy, roadmap, and release plan. Describe the target users as maintainers of legacy .NET Framework/IIS systems adopting Vault incrementally. Explain that Codex/API credits would reduce the maintainer burden for routine upkeep: issue triage, docs cleanup, troubleshooting improvements, PR review, and test coverage. Use real metrics only at submission time.
