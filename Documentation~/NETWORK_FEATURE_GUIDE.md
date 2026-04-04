# Network Feature Guide

This guide keeps PlatformCore networking small and reusable.

## Foundation Boundary

PlatformCore network layer includes:
- contracts in `Runtime/Services/Network/*`
- FishNet bridge/runtime glue in `Runtime/Infrastructure/Network/FishNet/*`
- registration entry `ServiceLocatorFishNetExtensions.RegisterFishNetFoundation(...)`

It tracks network session state and runtime callbacks. It does not own game-specific gameplay frameworks.

## Rules

- Keep FishNet-specific integration inside PlatformCore FishNet infrastructure layer.
- Keep gameplay orchestration in consumer project code.
- Do not move player spawn/game rules/inventory/interaction systems into PlatformCore.
- Do not force `Composite` on every network feature.
- Enforce owner-only input and owner-only camera on player-controlled entities.

## Recommended Extension Pattern

1. Add contract only when reusable across projects.
2. Implement integration adapter/service in FishNet infrastructure layer.
3. Register through existing bootstrap/composition flow.
4. Keep project-specific flow in consumer controllers.

## Anti-Patterns

- Turning PlatformCore into a universal multiplayer gameplay framework.
- Mixing transport/session concerns with unrelated UI/audio/gameplay logic.
- Solving project-specific rules inside PlatformCore network foundation.
