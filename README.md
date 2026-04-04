# Len PlatformCore (`com.len.platformcore`)

Reusable platform foundation for Unity projects.

## Install (Local Package)

1. Add package in consumer project `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.len.platformcore": "file:../PlatformCore"
  }
}
```

2. Reopen Unity or refresh packages.
3. Open installer: `Len/Installer`.
4. Click `Validate Setup`.

## Dependency Status

Required (declared in `package.json`):
- `com.unity.cinemachine`
- `com.unity.inputsystem`
- `com.unity.nuget.newtonsoft-json`
- `com.unity.textmeshpro`
- `com.cysharp.unitask`
- `com.kyrylokuzyk.primetween`

Optional:
- `com.firstgeargames.fishnet` (only for FishNet foundation/sample).

Manual external integration:
- FMOD is optional. Audio service runs in no-op mode without `FMOD_PRESENT`.

## Architecture Scope

- PlatformCore provides bootstrap/lifecycle/services foundation.
- FishNet stays a narrow foundation extension layer.
- PlatformCore is not a full game framework.
- Game-specific gameplay systems belong in the consumer project.

## Docs Entry Points

- `Documentation~/SETUP.md`
- `Documentation~/ARCHITECTURE.md`
- `Documentation~/EDITOR_TOOLS.md`
- `Documentation~/DOCS_STATUS.md`

## Package Layout

- `Runtime/`
- `Editor/`
- `Documentation~/`
- `Samples~/`
- `Templates~/`
- `Tests/`
