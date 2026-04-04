# Editor Tools (com.len.platformcore)

This package includes reusable editor-only generators and navigation helpers.

## Scenes

### Refresh Scene Menu
- **Menu:** `Tools/Len/PlatformCore/Scenes/Refresh Menu`
- **Script:** `Editor/Tools/Scenes/ScenesMenuGenerator.cs`
- Reads enabled scenes from **Build Settings** and regenerates `Editor/Tools/Scenes/GeneratedScenesMenu.cs`.
- Generated entries appear under `Tools/Len/PlatformCore/Scenes/<SceneName>` and open the scene with save confirmation.

### Use Persistent On Play
- **Menu:** `Len/Scenes/Use Persistent`
- **Script:** `Editor/Tools/Scenes/PersistentPlayToggle.cs`
- Adds a checkable toggle that forces Play Mode to start from the enabled Build Settings scene named `Persistent`.

## Resources

### Generate ResourcePaths
- **Menu:** `Tools/Len/PlatformCore/Resources/Generate ResourcePaths`
- **Script:** `Editor/Tools/Resources/ResourcePathsGenerator.cs`
- Scans `Assets/Resources` and generates `Assets/PlatformCore.Generated/ResourcePaths.Generated.cs`.
- Generated constants are written to `Project.Infrastructure.ProjectResourcePaths`.

### Install Platform UI Foundation
- **Menu:** `Len/Installer`
- **Script:** `Editor/Installer/PlatformCoreInstallerWindow.cs`
- Installs/updates PlatformCore-owned UI foundation assets into project `Assets`:
  - `Resources/UI/ColorStyleLibrary.asset`
  - `Resources/UI/TextStyleLibrary.asset`
  - notification prefabs (`UIGlobalNotificationView`, `UINotificationsView`, `UINotificationView`)
- Installer keeps a version marker at `Assets/PlatformCore.Generated/InstallState/platform_ui_foundation.version.txt`.

## Audio

### Generate SoundNames (FMOD optional)
- **Menu:** `Tools/Len/PlatformCore/Audio/Generate SoundNames`
- **Script:** `Editor/Tools/Audio/FMODSoundNamesGenerator.cs`
- Generates `Assets/_Project/Code/Audio/SoundNames.Generated.cs` from FMOD event list.
- Tool is optional: if FMOD editor assembly is unavailable or event cache is not ready, the command logs a warning and no file is generated.

## Notes
- All tools are editor-only and do not add runtime dependencies.
- Generated files are designed to be project-level outputs (`Assets/PlatformCore.Generated/...`) and are safe to customize/relocate if your pipeline requires it.
