# Architecture

## Source Of Truth Order

1. Runtime code in `Runtime/Infrastructure`, `Runtime/Services`, `Runtime/Gameplay`.
2. `README.md` and this file.
3. Historical `PlatformCore_2_0_*` docs.

If docs and code diverge, use code.

## Runtime Boundaries

- `Runtime/Core`: low-level contracts and lifecycle interfaces.
- `Runtime/Infrastructure`: bootstrap, service locator, lifecycle orchestration, composition helpers.
- `Runtime/Services`: reusable platform services (UI, scene, settings, audio, camera, input, notifications).
- `Runtime/Gameplay`: reusable gameplay-level blocks only when still generic.

## Composition Rules

- `LifecycleService` manages controllers.
- Services are global reusable systems and are registered in `ServiceLocator`.
- `Composite` is optional:
  - use direct `LifecycleService` registration for small local features;
  - use `Composite` only for explicit orchestration/lifetime boundary between multiple runtime parts.
- Do not add `Composite` mechanically.

## Platform vs Game

- PlatformCore contains reusable foundation only.
- Consumer project contains game-specific models/controllers/content paths/feature logic.
- FishNet in PlatformCore is a narrow session/foundation layer, not a full multiplayer framework.

## Resource Ownership

- Platform-owned resource ids live in `ResourcePaths.Platform.*`.
- Project-owned resources live in project generated constants (for example `ProjectResourcePaths`).
- Do not move project-specific paths into PlatformCore constants.
