# PlatformCore 2.0 - Implementation Plan (Historical)

Status: historical plan snapshot.  
This file was reduced to avoid stale/duplicated guidance and encoding noise.

## Historical objective

Move from imported legacy baseline to maintainable package foundation using:
- audit;
- stabilization;
- modularization;
- selective rewrite.

## High-level decisions preserved

- PlatformCore is a reusable foundation, not a full game framework.
- Keep game-specific logic in consumer projects.
- Keep FishNet integration narrow and optional.
- Prefer controlled incremental changes over big-bang rewrites.
- Use `Composite` only when it creates clear orchestration value.

## Current source of truth

- `README.md`
- `Documentation~/SETUP.md`
- `Documentation~/ARCHITECTURE.md`
- runtime code in `Runtime/*`
