## Summary

Describe the change and why it is needed.

Do not include Vault tokens, RoleId, SecretId, internal URLs, connection strings, or any real secrets in issues, pull requests, logs, screenshots, or examples.

## Testing

- [ ] Build checked: `dotnet build .\dotnet-framework-vault-starter.sln`
- [ ] Tests checked: `dotnet run --project .\tests\DotNetFrameworkVaultStarter.Tests\DotNetFrameworkVaultStarter.Tests.csproj`
- [ ] Markdown links checked when docs changed: `.\scripts\Test-MarkdownLinks.ps1`

## Documentation

- [ ] README/docs updated if behavior, examples, or setup changed.
- [ ] README/docs links checked when new files or links were added.
- [ ] Not applicable because this is not a docs-facing change.

## Security Checklist

- [ ] No real secret values, Vault addresses, internal domains, tokens, RoleId values, SecretId values, or connection strings were added.
- [ ] No token, role_id, secret_id, or returned secret values are logged.
- [ ] Docs and examples use dummy values only.
- [ ] Any auth or diagnostics behavior change is explained.
