# Upgrade Checklist

- [ ] Commit current project state.
- [ ] Update `com.len.platformcore` reference (path/version/revision).
- [ ] Reopen Unity and wait for reimport.
- [ ] Open `Len/Installer` and run `Validate Setup`.
- [ ] Ensure required dependencies are installed (Cinemachine, Input System, Newtonsoft Json, TMP, UniTask, PrimeTween).
- [ ] Re-check FMOD integration if your project uses FMOD.
- [ ] Re-run `Len/Resources/Generate ResourcePaths` in consumer project.
- [ ] Re-run `Len/Audio/Generate SoundNames` if FMOD events changed.
- [ ] Build solution and run smoke scene startup.
