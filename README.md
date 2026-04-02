# Len PlatformCore (`com.len.platformcore`)

Reusable platform foundation for Unity projects.

## Installation (Local Package)

1. Open your Unity project.
2. Open `Packages/manifest.json`.
3. Add a local dependency:

```json
{
  "dependencies": {
    "com.len.platformcore": "file:../PlatformCore"
  }
}
```

4. Reopen Unity (or trigger package refresh).
5. Open installer: `Tools/Len/PlatformCore/Installer`.
6. Click **Install Required Dependencies**.
7. Run **Validate Setup** and finish manual steps if needed.

## Dependencies

### Required
- Cinemachine (`com.unity.cinemachine`)
- TextMesh Pro (`com.unity.textmeshpro`)
- Input System (`com.unity.inputsystem`)

### Optional
- FishNet (`com.firstgeargames.fishnet`)

### Manual/External
- FMOD
- PrimeTween

> FishNet is optional and is only required for `NetworkSample` and FishNet integration code paths.

## Package layout

- `Runtime/`
- `Editor/`
- `Documentation~/`
- `Samples~/`
- `Tests/`
