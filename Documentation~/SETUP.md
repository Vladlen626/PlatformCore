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

## 2. Run installer window

Open `Tools/Len/PlatformCore/Installer`:
- Install Required Dependencies
- (Optional) Install Optional Dependencies
- Validate Setup

## 3. Required dependencies
- Cinemachine
- TextMesh Pro
- Input System

## 4. Optional dependencies
- FishNet (not mandatory; required only for networking sample/integration)

## 5. Manual/external setup
- FMOD
- PrimeTween

These are not auto-installed by this package and should be integrated manually when your project needs them.
