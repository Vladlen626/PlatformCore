# Network Smoke Test Plan (2 Players)

Scope: minimal template validation for host/client flow on localhost.  
Goal: verify baseline network behavior without building a large framework.

## Preconditions

- FishNet package installed in consumer project.
- `PlatformCore.Infrastructure.FishNet` assembly is enabled by package version define.
- Build Settings contain `Persistent` and gameplay scene(s).
- Player prefab is network-ready (FishNet object + required sync component).

## Test Case A: Editor Host + Editor Client

1. Launch host.
2. Launch client and connect to localhost.
3. Verify two player instances exist.
4. Verify each peer controls only its own player (owner-only input).
5. Verify each peer has camera only on local owner (owner-only camera attach).
6. Move/jump on one peer and verify transform sync on the other.

Pass criteria:
- No duplicate owner control.
- No remote camera hijack.
- Position/rotation stay synchronized.

## Test Case B: Windows Build + Editor

1. Build Windows player.
2. Start host in build, connect client from editor (localhost).
3. Repeat checks from Test Case A.

## Test Case C: Windows Build + Windows Build

1. Run two local builds (host and client).
2. Connect on localhost.
3. Repeat checks from Test Case A.

## Failure Checklist

- Both peers control same player.
- Remote peer gets active gameplay camera.
- Second player not spawned.
- Movement visible only locally (sync missing).
- Connection/session state not reflected in runtime logs/services.

## Notes

- This is a manual smoke plan, not a full automated multiplayer test suite.
- Keep template validation narrow; gameplay-specific network tests stay in consumer project.
