# Agent Instructions

## Read First (Source Of Truth)
1. `README.md`
2. `Documentation~/SETUP.md`
3. `Documentation~/ARCHITECTURE.md`
4. `Runtime/Infrastructure/BaseBootstrap.cs`
5. `Runtime/Infrastructure/BaseGameRoot.cs`
6. `Runtime/Infrastructure/Lifecycle/LifecycleService.cs`
7. `Runtime/Infrastructure/Composition/*`

Use code as the final source of truth when docs and code diverge.

## Scope Hygiene
- Read only files needed for the task.
- Prefer text/code files (`*.cs`, `*.asmdef`, `*.json`, `*.md`, `*.yml`).
- Do not scan `Library/`, `Temp/`, `Obj/`, `Logs/`, `Builds/`, `.git/`, `.vs/`, `.idea/`, `UserSettings/`.

## PlatformCore Architecture Rules
- PlatformCore is a reusable foundation layer, not a full game framework.
- Keep FishNet as a narrow foundation extension layer.
- Keep platform vs game boundaries strict: game-specific logic belongs in consumer project.
- Do not invent a parallel bootstrap or lifecycle flow.

## LifecycleService vs Composite
- `LifecycleService` registers controllers only.
- Services are global reusable systems registered in `ServiceLocator` and initialized via `ISyncInitializable` / `IAsyncInitializable`.
- `Composite` is optional.
- Register directly in `LifecycleService` for a small local feature when no extra orchestration boundary exists.
- Use `Composite` only if there is clear value:
  - one lifetime boundary for multiple runtime parts;
  - owned sub-controllers/disposables;
  - reusable grouped orchestration.
- Do not add `Composite` mechanically for “architecture compliance”.

## Dependency Rules
- Prefer constructor injection and explicit dependency passing.
- Avoid controller-to-controller hard dependencies unless orchestration is tightly local.
- Do not add new static `Locator.Resolve(...)` calls when dependency can be passed from composition root.

## Resources, UI, Input
- Use `IResourceService`/`IObjectFactory` and `ResourcePaths.Platform.*` for platform-owned resources.
- Project-owned resources must stay in project-level generated paths (`ProjectResourcePaths`), not in PlatformCore constants.
- UI flow uses `UIBaseElement` + `BaseContextController<T>` + `IUIService`.
- UI references must be explicit `[SerializeField]` links; avoid runtime hierarchy auto-resolution.
- Keep input API minimal and reusable; avoid project-specific actions in platform input contracts.

## Implementation Defaults
- Minimal viable changes first (KISS, YAGNI).
- Fix root causes, not masking workarounds.
- Maintain activate/deactivate symmetry.
- Use Unity-style null checks (`if (!obj)`), and always braces.
