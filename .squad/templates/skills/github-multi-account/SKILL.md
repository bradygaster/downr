---
name: github-multi-account
description: Detect and set up account-locked gh aliases for multi-account GitHub. The AI reads this skill, detects accounts, asks the user which is personal/work, and runs the setup automatically.
confidence: high
source: https://github.com/tamirdresher/squad-skills/tree/main/plugins/github-multi-account
author: tamirdresher
---

# GitHub Multi-Account — AI-Driven Setup

## When to Activate
When the user has multiple GitHub accounts (check with `gh auth status`). If you see 2+ accounts listed, this skill applies.

## What to Do (as the AI agent)

### Step 1: Detect accounts
Run: `gh auth status`
Look for multiple accounts. Note which usernames are listed.

### Step 2: Ask the user
Ask: "I see you have multiple GitHub accounts: {list them}. Which one is your personal account and which is your work/EMU account?"

### Step 3: Run the setup automatically
Once the user confirms, do ALL of this for them:

```powershell
# 1. Define the functions
$personal = "THEIR_PERSONAL_USERNAME"
$work = "THEIR_WORK_USERNAME"

# 2. Add to PowerShell profile
$profilePath = $PROFILE.CurrentUserAllHosts
if (!(Test-Path $profilePath)) { New-Item -Path $profilePath -Force | Out-Null }
$existing = Get-Content $profilePath -Raw -ErrorAction SilentlyContinue
if ($existing -notmatch "gh-personal") {
    $block = @"

# === GitHub Multi-Account Aliases ===
function gh-personal { gh auth switch --user $personal 2>`$null | Out-Null; gh @args }
function gh-work { gh auth switch --user $work 2>`$null | Out-Null; gh @args }
Set-Alias ghp gh-personal
Set-Alias ghw gh-work
"@
    Add-Content -Path $profilePath -Value $block
}

# 3. Create CMD wrappers
$binDir = Join-Path $env:USERPROFILE ".squad\bin"
if (!(Test-Path $binDir)) { New-Item -ItemType Directory -Path $binDir -Force | Out-Null }
"@echo off`ngh auth switch --user $personal >nul 2>&1`ngh %*" | Out-File "$binDir\ghp.cmd" -Encoding ascii
"@echo off`ngh auth switch --user $work >nul 2>&1`ngh %*" | Out-File "$binDir\ghw.cmd" -Encoding ascii

# 4. Add to PATH
$userPath = [Environment]::GetEnvironmentVariable("PATH", "User")
if ($userPath -notmatch [regex]::Escape($binDir)) {
    [Environment]::SetEnvironmentVariable("PATH", "$binDir;$userPath", "User")
    $env:PATH = "$binDir;$env:PATH"
}

# 5. Load in current session
function gh-personal { gh auth switch --user $personal 2>$null | Out-Null; gh @args }
function gh-work { gh auth switch --user $work 2>$null | Out-Null; gh @args }
Set-Alias ghp gh-personal
Set-Alias ghw gh-work
```

### Step 4: Verify for the user
Run these and show the results:
```powershell
ghp api user --jq '.login'   # should show personal username
ghw api user --jq '.login'   # should show work username
```

### Step 5: Tell the user
"All set! `ghp` uses your personal account and `ghw` your work account. For each repo, I'll use whichever one can push to it."

## After Setup — Usage Rules

1. **NEVER** use bare `gh` for repo operations — always `ghp` or `ghw`
2. **NEVER** manually `gh auth switch` — the aliases handle it
3. Pick the alias by which account can write to the repo, not by who owns it; see **Repo-Specific Account Binding** below.

## Repo-Specific Account Binding

Pick the alias by which account can write to the repo, not by the repo's name. Squad itself lives at `github/squad`, which the `github` organization owns, so don't assume your personal account. Check once before the first write and use the alias that prints `true` (gh fills in `{owner}` and `{repo}` from the current checkout):
```powershell
ghp api 'repos/{owner}/{repo}' --jq '.permissions.push'
ghw api 'repos/{owner}/{repo}' --jq '.permissions.push'
```
If neither prints `true`, neither account can push there; work from a fork.

## For Squad Agents
At the TOP of any script touching GitHub, define the aliases with the usernames the user confirmed in Step 2:
```powershell
function gh-personal { gh auth switch --user THEIR_PERSONAL_USERNAME 2>$null | Out-Null; gh @args }
function gh-work { gh auth switch --user THEIR_WORK_USERNAME 2>$null | Out-Null; gh @args }
```
