# Squad Team

> downr

## Coordinator

| Name | Role | Notes |
|------|------|-------|
| Squad | Coordinator | Routes work, enforces handoffs and reviewer gates. |

## Members

| Name | Role | Charter | Status |
|------|------|---------|--------|
| Web | Frontend Developer | `.squad/agents/web/charter.md` | ✅ Active |
| Server | Backend Developer | `.squad/agents/server/charter.md` | ✅ Active |
| Content | Content Engineer | `.squad/agents/content/charter.md` | ✅ Active |
| Quality | Test Engineer | `.squad/agents/quality/charter.md` | ✅ Active |
| Lead | Lead / Architect | `.squad/agents/lead/charter.md` | ✅ Active |

## Project Context

- **Project:** downr
- **Created:** 2026-11-17
- **Stack:** ASP.NET Core 3.1 server, Blazor WebAssembly client, and .NET Standard 2.1 shared library. `PROPOSAL-DOTNET10-RAZOR-PAGES.md` is a proposal, not the current implementation.
- **Code:** `Client/` contains the UI; `Server/` hosts APIs, storage indexers, and content; `Shared/` contains models and post services.
- **Content:** `Server/wwwroot/posts/` holds Markdown posts with YAML front matter; Blog mode sorts by date and Workshop mode uses phase and step.
- **Build:** `dotnet build downr.sln` (see `.vscode/tasks.json`). The projects use older SDK dependencies and may require a compatible .NET SDK and package restore.
- **Tests:** No test project or test command is tracked in this repository.
