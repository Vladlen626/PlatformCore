# PlatformCore 2.0 — Batch 4: Resource / Factory cleanup

## Scope

Controlled cleanup of imported Resource/Factory layer without runtime architecture rewrite.

## ResourcePaths ownership split

`ResourcePaths` was split into explicit ownership groups:

- `ResourcePaths.Platform.*` — platform-level reusable entries.
- `ResourcePaths.Sample.*` — sample/game-specific legacy payload imported from D6.
- `ResourcePaths` legacy alias groups remain for compatibility and are marked as obsolete.

This keeps generator-driven sample map in a dedicated generated file while removing the old flat "game map" appearance from the main entrypoint.

## Service responsibility clarification

- `IResourceService` / `ResourceService` are now explicitly in `PlatformCore.Services.Factory` namespace to align with factory-layer ownership.
- `ObjectFactory` and `ResourceService` now document platform scope in code comments.
- `ResourceService.LoadAsync` logging/flow was cleaned up to stop reporting successful load when asset is missing.

## Runtime usage adjustments

Runtime calls that depend on game/sample content now reference `ResourcePaths.Sample.*` explicitly:

- camera prefab loading;
- localization sample json loading;
- notification prefab loading.

This makes sample dependencies visible and non-foundational by default.
