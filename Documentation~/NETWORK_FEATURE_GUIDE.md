# Network Feature Guide

This guide defines how to extend networking without turning PlatformCore into a full multiplayer framework.

## Current Foundation Boundary

PlatformCore network layer is intentionally narrow:
- contracts: `Runtime/Services/Network/*`
- FishNet bridge/runtime glue: `Runtime/Infrastructure/Network/FishNet/*`
- registration entry: `ServiceLocatorFishNetExtensions.RegisterFishNetFoundation(...)`

It tracks session state and runtime callbacks. It does not own full gameplay spawning/rules/progression.

## Rules

- Keep FishNet-specific code inside `Runtime/Infrastructure/Network/FishNet`.
- Keep generic contracts in `Runtime/Services/Network`.
- Do not move game-specific systems into PlatformCore.
- Do not introduce mandatory `Composite` for every network feature.
- Owner-only input and owner-only camera are mandatory for player-controlled entities.

## Recommended Extension Pattern

1. Add a minimal feature contract in `Runtime/Services/Network` only if reusable.
2. Implement FishNet adapter/service in `Runtime/Infrastructure/Network/FishNet`.
3. Register via service locator extension in bootstrap/composition root.
4. Keep gameplay orchestration in consumer project controllers.

## Owner Rules For Player Entities

- Input: read only on owner/local player.
- Camera: attach/enable only on owner/local player.
- Movement authority: choose one model and keep it explicit (server-authoritative or owner-authoritative).
- Synchronization: use FishNet network transform/sync components, not ad-hoc custom polling.

## Anti-Patterns

- Building a universal “multiplayer gameplay framework” inside PlatformCore.
- Mixing network transport code with unrelated UI/audio/gameplay features.
- Global static network lookups in feature code when dependencies can be injected.
