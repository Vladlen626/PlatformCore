# PlatformCore 2.0 - Usage Guide (Historical)

Status: historical reference.  
Use `README.md`, `Documentation~/SETUP.md`, and `Documentation~/ARCHITECTURE.md` as the current source of truth.

## What this document records

- Transition from imported baseline to reusable package structure.
- Foundation-first approach before gameplay-specific additions.
- Narrow network foundation policy (FishNet as extension, not full framework).

## Stable takeaways still valid

- Keep bootstrap flow centered on `BaseBootstrap` + `BaseGameRoot`.
- Register services in `ServiceLocator`; register controllers in `LifecycleService`.
- Use `Composite` only when a feature has a real orchestration/lifetime boundary.
- Keep platform and game layers separated.

## What changed since this snapshot

- Some gameplay camera references from early drafts were removed.
- Menu paths are now under `Len/...`.
- Resource path ownership is split into platform constants and project-generated constants.
