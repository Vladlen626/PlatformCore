# Setup

## 1. Add package as local dependency

In consumer project `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.len.platformcore": "file:../PlatformCore"
  }
}
```

## 2. Refresh packages

Unity Package Manager will install declared dependencies automatically from `com.len.platformcore/package.json`.

## 3. Run installer window (validation + helper install actions)

Open `Len/Installer`:
- Install Required Dependencies (helper action for required UPM deps)
- Install Optional Dependencies (FishNet)
- Install / Update Platform UI Foundation (styles + notifications assets in `Assets/Resources/UI`)
- Validate Setup

## 4. Required dependencies (declared)
- Cinemachine
- TextMesh Pro
- Input System
- Newtonsoft Json
- UniTask

## 5. Optional dependencies
- FishNet (not mandatory; required only for networking sample/integration)

## 6. Manual/external integrations (optional)
- FMOD (audio service falls back to no-op when FMOD is not installed)
- PrimeTween (not required by base runtime)
