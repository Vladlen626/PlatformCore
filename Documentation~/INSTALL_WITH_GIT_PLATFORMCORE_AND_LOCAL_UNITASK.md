# Install `com.len.platformcore` from Git with local UniTask package

Use this guide when:
- `com.len.platformcore` is added from Git;
- `com.cysharp.unitask` is **not** resolved from UPM/OpenUPM in your project;
- you want to point Unity to a local UniTask package on disk.

## Expected folder layout

Example:

```text
C:/Users/<you>/projects/
  GameTemplate/
  UniTask/
```

Inside the UniTask repository, the package root must be the folder that contains `package.json`.

Example package root:

```text
C:/Users/<you>/projects/UniTask/src/UniTask/Assets/Plugins/UniTask
```

## Important note about relative paths

`Packages/manifest.json` lives inside your Unity project `Packages/` folder.

That means `file:` paths are resolved **relative to the `Packages/` folder**, not relative to the project root.

For a project located at:

```text
C:/Users/<you>/projects/GameTemplate
```

and a UniTask package located at:

```text
C:/Users/<you>/projects/UniTask/src/UniTask/Assets/Plugins/UniTask
```

the correct relative path from `GameTemplate/Packages` is:

```text
../../UniTask/src/UniTask/Assets/Plugins/UniTask
```

## `manifest.json` example

Add this to the consumer project `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.len.platformcore": "https://github.com/Vladlen626/PlatformCore.git#LeN",
    "com.cysharp.unitask": "file:../../UniTask/src/UniTask/Assets/Plugins/UniTask"
  }
}
```

## Installation steps

1. Ensure the UniTask folder you reference contains its own `package.json`.
2. Add `com.len.platformcore` from Git.
3. Add `com.cysharp.unitask` as a local `file:` package using the path to the UniTask package root.
4. Save `Packages/manifest.json`.
5. Reopen Unity or wait for Package Manager refresh.
6. Open `Len/Installer` and run `Validate Setup`.

## Troubleshooting

### Error: `Package [com.cysharp.unitask@...] cannot be found`

Unity did not resolve `UniTask` from registries. Add it explicitly as a local `file:` package.

### Error: `The file [...]/package.json cannot be found`

The `file:` path points to the wrong folder.

Check both:
- the target folder really contains `package.json`;
- the relative path is calculated from the Unity project's `Packages/` folder.

### UniTask copied into `Assets/`

This does **not** satisfy the package dependency declared by `com.len.platformcore`.

`UniTask` must be available as a package dependency, not only as source files under `Assets/`.
