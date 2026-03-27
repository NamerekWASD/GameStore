# .NET Upgrade Plan: Upgrade to .NET 10.0 (All-At-Once)

## Table of contents
- Executive Summary
- Migration Strategy
- Detailed Dependency Analysis
- Project-by-Project Plans
- Package Update Reference
- Breaking Changes Catalog
- Testing & Validation Strategy
- Risk Management
- Complexity & Effort Assessment
- Source Control Strategy
- Success Criteria

---

## Executive Summary
### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in a single atomic operation.

Rationale:
- Solution size: 6 projects (small/medium)
- Current targets: net7.0 (primary) and one legacy net472 project
- Assessment shows the majority of packages have clear target versions for net10.0 and no binary-incompatible APIs reported
- Test projects exist and are available to validate behavior after upgrade

Scope:
- Projects: `API`, `BLL`, `DAL`, `Exceptions`, `Tests`, `ClientApp`
- Target framework: `net10.0` for all projects per assessment recommendations
- Package updates: Apply all suggested package updates from assessment, prioritize those with security vulnerabilities

Critical issues discovered (must be addressed during upgrade):
- Source-incompatible APIs identified for `API` (JwtBearer, Identity EF extensions, some TimeSpan usage)
- Several NuGet packages require updates; some packages flagged with security vulnerabilities (AutoMapper, MimeKit)

Deliverable (post-upgrade):
- Unified solution targeting `net10.0` where appropriate
- All NuGet package updates applied as per assessment
- Solution builds with 0 compilation errors
- All automated tests run and pass


## Migration Strategy
Selected approach: **All-At-Once** (atomic upgrade of all projects simultaneously).

Why All-At-Once:
- Solution is small (6 projects) and dependency graph is straightforward.
- Faster overall time-to-completion and simpler dependency resolution when all projects target the same framework.
- Assessment indicates package updates are available for the target framework and test projects exist for validation.

Key strategy points:
- Perform prerequisite checks first: ensure .NET 10 SDK available and `global.json` (if present) aligns with SDK requirement.
- Update TargetFramework properties and any shared MSBuild imports (e.g., `Directory.Build.props`, `Directory.Packages.props`) in a single atomic change.
- Update all NuGet package references to the suggested versions from the assessment in the same atomic change.
- Restore, build and fix compilation issues as part of the same upgrade pass.
- Run the test suite after the atomic upgrade and address test failures as a separate step after the atomic commit.

Parallelization and ordering:
- The upgrade is an atomic change across the repository; internal fixes discovered during build may be batched but remain part of the single upgrade operation.
- Test execution and fixing test failures occurs after the atomic upgrade is committed.

Source control model:
- Create and switch to branch `upgrade-to-NET10` (already suggested by assessment tool).
- Prefer a single commit that contains the atomic upgrade (project TFMs + package updates). Smaller follow-ups to address discovered compilation errors are acceptable but should be kept minimal and linked to the same PR.

## Detailed Dependency Analysis

Summary:
- Total projects: 6
- Topology: shallow dependency graph; no cycles detected by assessment
- Leaf nodes (no project dependencies): `Exceptions`, `DAL`, `clientapp.esproj`
- Root nodes (applications / entrypoints): `API` (Razor Pages) and `Tests` (test runner)

Project dependency overview (textual):
- `Exceptions` → used by `BLL`
- `DAL` → used by `BLL`
- `BLL` → used by `API` and `Tests`
- `clientapp.esproj` → referenced by `API`
- `API` → referenced by `Tests`

Migration ordering rationale for All-At-Once:
- Because this plan uses an All-At-Once strategy, all projects will be updated simultaneously. However dependency awareness is still required to:
  - Identify where source-incompatible APIs appear (notably in `API`)
  - Ensure shared packages and imported MSBuild props are updated consistently

Critical path:
- `Exceptions` / `DAL` → `BLL` → `API` → `Tests`

Circular dependencies:
- None detected

## Project-by-Project Plans

### Project: API (API\API.csproj)
**Current State**: `net7.0`, AspNetCore Razor Pages project, 47 files, 2167 LOC

**Target State**: `net10.0`

**Migration Steps (high level)**:
1. Update `TargetFramework` to `net10.0` in `API\API.csproj` (or add `TargetFrameworks` if multi-targeting required).
2. Update package references to the suggested versions from assessment (see Package Update Reference below).
3. Review code areas flagged as source-incompatible (JwtBearer setup, Identity EF extensions, TimeSpan factory usages) and prepare replacements or API adaptations.
4. Update any `Program.cs`/`Startup` patterns if required by framework changes.
5. Build solution and address compilation errors discovered.
6. Run API-related tests from `Tests` project and address failing tests.

**Risk level**: Medium — source-incompatible API usage found and multiple package updates required

**Validation**:
- Builds without compilation errors
- Tests referencing API pass
- No security-vulnerable packages remain


### Project: BLL (BLL\BLL.csproj)
**Current State**: `net7.0`, ClassLibrary, 42 files, 1599 LOC

**Target State**: `net10.0`

**Migration Steps (high level)**:
1. Update `TargetFramework` to `net10.0`.
2. Update NuGet packages per assessment (AutoMapper, EF packages if present)
3. Build and resolve compilation errors

**Risk level**: Low

**Validation**:
- Builds cleanly
- API and Tests that depend on BLL build and run


### Project: DAL (DAL\DAL.csproj)
**Current State**: `net7.0`, ClassLibrary, 22 files, 640 LOC

**Target State**: `net10.0`

**Migration Steps (high level)**:
1. Update `TargetFramework` to `net10.0`.
2. Update EF Core packages to `10.0.5` per assessment.
3. Validate EF migrations and usage of Identity.EntityFrameworkCore APIs.

**Risk level**: Low-Medium (EF package updates may surface API changes)

**Validation**:
- DAL builds and unit tests pass
- EF usages compile and migrations (if any) are compatible


### Project: Exceptions (Exceptions\Exceptions.csproj)
**Current State**: `net7.0`, ClassLibrary, 5 files, 44 LOC

**Target State**: `net10.0`

**Migration Steps (high level)**:
1. Update `TargetFramework` to `net10.0`.
2. Build and validate dependent projects (BLL)

**Risk level**: Low

**Validation**:
- Builds cleanly and dependent projects build


### Project: ClientApp (ClientApp\clientapp.esproj)
**Current State**: `net472`, DotNetCoreApp, legacy project referenced by API

**Target State**: `net10.0` (assessment proposed)

**Migration Steps (high level)**:
1. Convert or update `clientapp.esproj` to target `net10.0` as appropriate. Verify whether this project is required to be migrated or can be isolated (it may be an external tool or packaging project).
2. Update references or interop points used by `API`.

**Risk level**: Low (legacy target but small impact)

**Validation**:
- Builds with `API`


### Project: Tests (Tests\Tests.csproj)
**Current State**: `net7.0`, Test runner, 4 files, 446 LOC

**Target State**: `net10.0`

**Migration Steps (high level)**:
1. Update `TargetFramework` to `net10.0`.
2. Update test SDK packages if necessary.
3. Run test suite and address failures

**Risk level**: Low-Medium

**Validation**:
- All tests pass


## Package Update Reference

### Common Package Updates (affecting multiple projects)
- `AutoMapper`: 12.0.1 → 16.1.1 (affects `API`, `BLL`) — security vulnerability
- `MimeKit`: 3.6.0 → 4.15.1 (affects `API`, `BLL`) — security vulnerability
- `Microsoft.EntityFrameworkCore`: 7.x → 10.0.5 (affects `API`, `DAL`, `BLL`, `Tests`) — framework alignment
- `Microsoft.AspNetCore.Authentication.JwtBearer`: 7.0.4 → 10.0.5 (affects `API`) — ensure TokenValidationParameters usage conforms to new ABI
- `Microsoft.AspNetCore.Authentication.Google`: 7.0.4 → 10.0.5 (affects `API`)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`: 7.0.x → 10.0.5 (affects `DAL`, `API`, `BLL`)
- `Microsoft.AspNetCore.SpaProxy`: 7.0.5 → 10.0.5 (affects `API`)
- `Microsoft.EntityFrameworkCore.SqlServer`: 7.0.4 → 10.0.5 (affects `API`, `Tests`)
- `Microsoft.EntityFrameworkCore.Proxies`: 7.0.4 → 10.0.5 (affects `API`)
- `Microsoft.Extensions.Configuration`: 7.0.0 → 10.0.5 (affects `DAL`)
- `Microsoft.Extensions.*` packages should be aligned to `10.0.5` where present

### Project-specific package notes
- `Tests`: verify `Microsoft.NET.Test.Sdk` and `coverlet.collector` compatibility with `net10.0` and update if recommended
- `clientapp.esproj`: check if packages exist or if it's a packaging project; upgrade only if used by runtime code


## Breaking Changes Catalog
- JwtBearer extensions: review `AddJwtBearer` usage and `TokenValidationParameters` mapping. Some option types or default behaviors may have changed; adapt delegate signatures if necessary.
- Identity EF extensions: `AddEntityFrameworkStores` may require updated using/imports or overload changes. Verify generic type arguments and ensure Identity packages align with EF Core version.
- TimeSpan factory methods: usage of `TimeSpan.FromDays`/`FromMinutes` flagged as source-incompatible in some contexts; ensure calls specify correct numeric types and recompile to reveal exact issues.
- Console logging: `AddConsole` behavior changed; validate logging configuration to ensure console logging works as expected.


## Testing & Validation Strategy
- Prerequisite validation: Confirm .NET 10 SDK installed and `global.json` updated if present.
- Atomic upgrade validation (after TFMs and package updates):
  - `dotnet restore`
  - `dotnet build` - solution should compile with 0 errors
- Test execution (after build):
  - Run unit tests in `Tests` project
  - Fix test failures
- Post-upgrade runtime checks (manual): run the application in a staging environment and verify Razor Pages render (note: manual checks are not converted to executable tasks)


## Risk Management

### High-Risk Items
- `API`: multiple source-incompatible API usages and package updates; medium risk. Mitigation: assign senior engineer to resolve authentication and Identity-related changes.
- `AutoMapper`, `MimeKit`: security vulnerabilities — upgrade immediately as part of atomic change.

### Mitigation Strategies
- Keep upgrade atomic but allow small follow-up commits to address compilation fixes discovered during build; group them in the same PR and document reason.
- Use feature branch `upgrade-to-NET10` and open a single PR for review.
- Run tests and scan for vulnerabilities after upgrade.


## Complexity & Effort Assessment
- Overall solution complexity: Simple (6 projects, shallow graph)
- Per-project complexity: `API` Medium; others Low


## Source Control Strategy
- Branch: `upgrade-to-NET10`
- Commit scope: single atomic commit containing:
  - All `TargetFramework` changes
  - Updated `Directory.Build.props` or `Directory.Packages.props` if applicable
  - All NuGet package version updates
- PR: one PR that contains the atomic upgrade; subsequent small commits to fix discovered compilation/test failures are acceptable and should be documented in the PR description


## Success Criteria
- All projects target `net10.0` (or remain netstandard if assessment advised)
- All package updates from assessment applied
- Solution builds with 0 compilation errors
- All tests pass
- No flagged security-vulnerable packages remain


[Plan draft complete — continuing to fill per-project detailed steps and exact package tables]

## Project-by-Project Detailed Plans

### API (API\API.csproj)

Packages to update (from assessment):
- `AutoMapper` 12.0.1 → 16.1.1 (security)
- `MimeKit` 3.6.0 → 4.15.1 (security)
- `Microsoft.AspNetCore.Authentication.Google` 7.0.4 → 10.0.5
- `Microsoft.AspNetCore.Authentication.JwtBearer` 7.0.4 → 10.0.5
- `Microsoft.AspNetCore.SpaProxy` 7.0.5 → 10.0.5
- `Microsoft.EntityFrameworkCore` 7.0.4 → 10.0.5
- `Microsoft.EntityFrameworkCore.Proxies` 7.0.4 → 10.0.5
- `Microsoft.EntityFrameworkCore.SqlServer` 7.0.4 → 10.0.5

Code areas to inspect and adjust:
- Authentication startup code (JwtBearerOptions and TokenValidationParameters usage)
- Identity/EF registration calls (`AddEntityFrameworkStores`)
- Any calls relying on old behavior of `AddConsole` logging

Validation checklist:
- Project file `API\API.csproj` has `TargetFramework` set to `net10.0`.
- All package references updated to target versions.
- Solution builds with 0 errors.
- Tests referencing API pass.


### BLL (BLL\BLL.csproj)

Packages to update:
- `AutoMapper` 12.0.1 → 16.1.1 (security)
- `Microsoft.EntityFrameworkCore` 7.0.x → 10.0.5 (if referenced)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` 7.0.4 → 10.0.5 (if referenced)

Validation checklist:
- `TargetFramework` set to `net10.0`.
- Build successful.


### DAL (DAL\DAL.csproj)

Packages to update:
- `Microsoft.EntityFrameworkCore` 7.0.3 → 10.0.5
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` 7.0.3 → 10.0.5
- `Microsoft.Extensions.Configuration` 7.0.0 → 10.0.5

Code areas to inspect and adjust:
- EF Core API usages and migrations

Validation checklist:
- `TargetFramework` set to `net10.0`.
- Build successful and migrations compatible.


### Exceptions (Exceptions\Exceptions.csproj)

Packages to update: none suggested

Validation checklist:
- `TargetFramework` set to `net10.0`.
- Build successful.


### ClientApp (ClientApp\clientapp.esproj)

Notes:
- The project currently targets `net472`. Confirm if it must be retargeted to `net10.0` or excluded from build artifacts that run on server.
- If migrating to `net10.0`, update project file accordingly and ensure any packaging or tooling steps are updated.

Validation checklist:
- If migrated: `TargetFramework` set to `net10.0` and builds with `API`.
- If excluded: ensure `API` references are adjusted or protected behind runtime checks.


### Tests (Tests\Tests.csproj)

Packages to update:
- `Microsoft.EntityFrameworkCore` 7.0.4 → 10.0.5 (if referenced)
- Test SDKs: verify `Microsoft.NET.Test.Sdk`, `coverlet.collector` compatibility

Validation checklist:
- `TargetFramework` set to `net10.0`.
- All tests run and pass.


## Breaking Changes Catalog (detailed)

1. Authentication / JwtBearer
- Review `AddJwtBearer` usage. The delegate signature for options hasn't changed broadly but inner types (TokenValidationParameters defaults/behaviors) may be adjusted. Specific compiler errors will reveal required code changes. Typical fixes:
  - Ensure `JwtBearerDefaults.AuthenticationScheme` is used where required.
  - Update claims mapping or token validation parameter configuration to be explicit where previously implicit defaults changed.

2. Identity / EF
- `AddEntityFrameworkStores<TContext>` may require explicit generic type arguments or different extension availability depending on package versions. Ensure Identity packages and EF Core are on the matching 10.0.5 versions.

3. TimeSpan factory usage
- Some code flagged for `TimeSpan.FromDays`/`FromMinutes` may use integer literals in ways that the analyzer marks as source-incompatible; recompile to see the exact messages and update numeric types if necessary (e.g., `TimeSpan.FromDays(1.0)` instead of `TimeSpan.FromDays(1)`).

4. Logging behavior
- `AddConsole` behavior changed; verify `ILoggingBuilder.AddConsole()` usage and configuration values for formatters and scope settings.


## Outstanding Questions & Unknowns
- `clientapp.esproj` current role in runtime — confirm whether it must be migrated or excluded.
- Any `global.json` files specifying SDK version — need to ensure they match the required SDK for net10.0.


## Next Steps (for execution stage)
1. Ensure .NET 10 SDK is installed on build agents and `global.json` updated as needed.
2. Create and switch to branch `upgrade-to-NET10` and record initial commit.
3. Apply atomic changes to project TFMs and package versions.
4. Restore and build solution — collect compilation errors and fix them in the same branch as part of the atomic upgrade.
5. Run tests and fix failures.
6. Open PR and request review.


---

Plan draft complete.
