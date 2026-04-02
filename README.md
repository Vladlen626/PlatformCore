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
6. Click **Validate Setup** (optional: run dependency install actions if your project blocks transitive install).

## Dependencies

### Required (declared in `package.json`)
- Cinemachine (`com.unity.cinemachine`)
- TextMesh Pro (`com.unity.textmeshpro`)
- Input System (`com.unity.inputsystem`)
- Newtonsoft Json (`com.unity.nuget.newtonsoft-json`)
- UniTask (`com.cysharp.unitask`)

### Optional
- FishNet (`com.firstgeargames.fishnet`) — only for FishNet integration/sample.

### Manual/External integrations (not required for base package compile)
- FMOD (audio integration is no-op when FMOD is absent).
- DOTween (legacy tween integration removed from base runtime path).

## Samples status

Current samples are lightweight wiring examples, **not full production demos**:
- `BasicBootstrap` — bootstrap/config placeholder content.
- `SettingsPause` — settings/pause flow outline.
- `CameraSample` — camera wiring outline.
- `NetworkSample` — optional FishNet-oriented demo scaffold.

## Package layout

- `Runtime/`
- `Editor/`
- `Documentation~/`
- `Samples~/`
- `Tests/`
