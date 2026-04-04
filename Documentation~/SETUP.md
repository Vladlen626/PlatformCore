# Setup

## 1. Add Package

In consumer project `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.len.platformcore": "file:../PlatformCore"
  }
}
```

## 2. Refresh Unity Packages

Reopen Unity or trigger package refresh.

## 3. Run Installer

Open `Len/Installer` and run:
- `Install Required Dependencies`
- `Install Optional Dependencies` (only if you need FishNet)
- `Install / Update Platform UI Foundation`
- `Validate Setup`

## 4. Dependency Matrix

Required:
- Cinemachine
- Input System
- Newtonsoft Json
- TextMesh Pro
- UniTask
- PrimeTween

Optional:
- FishNet

Manual external:
- FMOD (optional; used only when `FMOD_PRESENT` is defined and FMOD package is installed)
