# Solution .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of an atomic, all-at-once upgrade of the solution to `net10.0`, followed by automated test validation. The upgrade will update TargetFrameworks and package versions across all projects in a single coordinated change, then run and repair tests.

**Progress**: 1/3 tasks complete (33%) ![0%](https://progress-bar.xyz/33)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-03-26 10:20)*
**References**: Plan §Migration Strategy, Plan §Next Steps, Plan §Outstanding Questions & Unknowns

- [✓] (1) Verify required .NET 10 SDK is installed on the build agent per Plan §Migration Strategy
- [✓] (2) Installed SDK version meets the minimum requirement for `net10.0` (**Verify**)
- [✓] (3) If a `global.json` file is present, verify its `sdk.version` aligns with the required .NET 10 SDK per Plan §Next Steps
- [✓] (4) `global.json` (if present) is compatible with the required SDK (**Verify**)

### [▶] TASK-002: Atomic framework and package upgrade with compilation fixes
**References**: Plan §Migration Strategy, Plan §Project-by-Project Plans, Plan §Package Update Reference, Plan §Breaking Changes Catalog, Plan §Source Control Strategy

- [✓] (1) Update `TargetFramework` to `net10.0` in all project files listed in Plan §Project-by-Project Plans (including `API`, `BLL`, `DAL`, `Exceptions`, `ClientApp`, `Tests`) and adjust multi-targeting if required
- [✓] (2) Update shared MSBuild imports or central package/version files (e.g., `Directory.Build.props`, `Directory.Packages.props`) per Plan §Migration Strategy and Plan §Package Update Reference
- [✓] (3) Update all NuGet package references to the target versions from Plan §Package Update Reference (focus: `AutoMapper`, `MimeKit`, EF Core and ASP.NET Core packages)
- [✓] (4) Restore dependencies (`dotnet restore`) for the solution
- [▶] (5) Build the solution to identify compilation issues (`dotnet build`) and fix all compilation errors caused by framework and package upgrades using guidance in Plan §Breaking Changes Catalog
- [ ] (6) Rebuild the solution to verify fixes
- [ ] (7) Solution builds with 0 errors (**Verify**)
- [ ] (8) Commit atomic upgrade with message: "TASK-002: Atomic upgrade to net10.0 — update TFMs and package versions"

### [ ] TASK-003: Run test suite and validate upgrade
**References**: Plan §Testing & Validation Strategy, Plan §Project-by-Project Plans, Plan §Breaking Changes Catalog, Plan §Source Control Strategy

- [ ] (1) Run the `Tests` project test suite per Plan §Testing & Validation Strategy (e.g., `dotnet test Tests/Tests.csproj`)
- [ ] (2) Fix any test failures referencing Plan §Breaking Changes Catalog and per-project notes in Plan §Project-by-Project Plans
- [ ] (3) Re-run the `Tests` project after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)
- [ ] (5) Commit test fixes with message: "TASK-003: Complete testing and validation"




