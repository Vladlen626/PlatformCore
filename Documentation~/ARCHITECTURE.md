# Architecture

PlatformCore keeps existing boundaries and responsibilities:

- `Runtime/Core` — lifecycle contracts and core abstractions.
- `Runtime/Infrastructure` — orchestration, composition helpers, integration glue.
- `Runtime/Services` — reusable gameplay/platform services.
- `Runtime/Gameplay` — sample gameplay-level controllers and views.

Editor-only tooling is isolated in `Editor/`.

## Dependency model

- Required package dependencies are installable from the installer window.
- FishNet integration is optional and isolated.
- FMOD and PrimeTween stay manual/external integrations.

## API stability

This migration focuses on packaging/layout changes with minimal behavior refactoring to preserve existing public API where practical.
