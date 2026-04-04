# Editor Tools (`com.len.platformcore`)

## Installer

- Menu: `Len/Installer`
- Script: `Editor/Installer/PlatformCoreInstallerWindow.cs`
- Actions:
  - install required package dependencies;
  - install optional FishNet dependency;
  - install/update platform UI foundation assets;
  - validate setup.

## Scenes

### Refresh Scene Menu
- Menu: `Len/Scenes/Refresh Menu`
- Script: `Editor/Tools/Scenes/ScenesMenuGenerator.cs`
- Output: `Assets/Editor/Tools/Scenes/PlatformCoreGeneratedScenesMenu.cs`

### Use Persistent On Play
- Menu: `Len/Scenes/Use Persistent`
- Script: `Editor/Tools/Scenes/PersistentPlayToggle.cs`
- Forces play mode start scene to enabled Build Settings scene named `Persistent`.

## Resources

### Generate ResourcePaths
- Menu: `Len/Resources/Generate ResourcePaths`
- Script: `Editor/Tools/Resources/ResourcePathsGenerator.cs`
- Output: `Assets/PlatformCore.Generated/ResourcePaths.Generated.cs`
- Namespace: `Project.Infrastructure.ProjectResourcePaths`
- Generator skips entries already declared in `ResourcePaths.Platform.*`.

## Audio

### Generate SoundNames (FMOD optional)
- Menu: `Len/Audio/Generate SoundNames`
- Script: `Editor/Tools/Audio/FMODSoundNamesGenerator.cs`
- Output: `Assets/_Project/Code/Audio/SoundNames.Generated.cs`
- Requires FMOD editor assembly + event cache. Otherwise logs warning and skips generation.
