# AppRole Auth

Use this document when you are preparing IIS or Windows Server deployments to authenticate to Vault without a long-lived static token in application config. AppRole is the recommended authentication method for server deployments in this starter.

## Why AppRole

Compared with a long-lived static token, AppRole lets operators scope access through Vault policies and rotate `secret_id` values through deployment processes.

This starter expects:

- `VAULT_AUTH_METHOD=AppRole`
- `VAULT_ROLE_ID`
- `VAULT_SECRET_ID`

## Dummy Vault CLI Example

The following is illustrative only. Adjust paths, policies, TTLs, and wrapping based on your Vault operating model.

```bash
vault auth enable approle

vault policy write legacy-app-read - <<'EOF'
path "secret/data/legacy-app/*" {
  capabilities = ["read"]
}
EOF

vault write auth/approle/role/legacy-app \
  token_policies="legacy-app-read" \
  token_ttl="30m" \
  token_max_ttl="4h" \
  secret_id_ttl="24h" \
  secret_id_num_uses="0"
```

Read generated values through your secure deployment process:

```bash
vault read auth/approle/role/legacy-app/role-id
vault write -f auth/approle/role/legacy-app/secret-id
```

Do not commit those values or print them in logs.

## App.config Fallback

Fallback is useful for controlled test environments, but server deployments should normally prefer environment variables or deployment-time secret injection.

```xml
<appSettings>
  <add key="Vault:Address" value="https://vault.example.invalid" />
  <add key="Vault:MountPoint" value="secret" />
  <add key="Vault:AuthMethod" value="AppRole" />
  <add key="Vault:RoleId" value="__ROLE_ID_FROM_SECURE_DEPLOYMENT__" />
  <add key="Vault:SecretId" value="__SECRET_ID_FROM_SECURE_DEPLOYMENT__" />
</appSettings>
```

## Troubleshooting

- `permission denied`: check the policy path and AppRole policy attachment.
- `not found`: check the KV path and mount point.
- auth failure: confirm `role_id` and `secret_id` are from the same AppRole.
- works in console but not IIS: check process identity and environment variable scope.
