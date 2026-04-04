# PlatformCore 2.0 - Module Split Spec (Historical)

Status: historical split reference.

## Historical target

Separate monolithic runtime into clear layers:
- `PlatformCore.Core`
- `PlatformCore.Infrastructure`
- `PlatformCore.Editor`

## Stable rules retained

- `Core` should stay lightweight and free from vendor-heavy dependencies.
- `Infrastructure` depends on `Core`, not vice versa.
- Editor tooling must stay in editor assemblies.
- Runtime foundation should not depend on samples.

## Current practical state

- Primary split (`Core` / `Infrastructure` / `Editor`) is done.
- Additional logical boundaries are handled by folder/module ownership and targeted refactors.
