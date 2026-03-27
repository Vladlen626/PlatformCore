# PlatformCore 2.0 — Batch 3 Baseline Payload Cleanup

Дата: 2026-03-27  
Статус: выполнено (controlled cleanup, без runtime redesign)

## Scope

Проведена ревизия non-code payload, импортированного вместе с legacy baseline, с фокусом на placeholder/demo content в foundation дереве.

## Classification

| Payload | Решение | Примечание |
|---|---|---|
| `PlatformCore/Materials/PlaceholderMats/*` | перенесено в sample/demo layer | Набор placeholder материалов не нужен как обязательная часть foundation runtime. |
| `PlatformCore/Materials/Textures/kenney_prototypeTextures/*` | перенесено в sample/demo layer | Texture pack относится к demo/sample payload. |
| `PlatformCore/Materials/PlaceholderMat.mat`, `base_placeholder_mat.mat`, `white_placeholder_mat.mat` | перенесено в sample/demo layer | Foundation runtime не должен зависеть от этих материалов. |
| `orange_placeholder_mat 1.mat` (+ `.meta`) | удалено из репозитория | Дублирующий project-specific мусорный asset. |

## Resulting layout

- `PlatformCore/Materials` больше не используется как foundation папка.
- Sample/demo payload расположен в `PlatformCore/Samples/Materials`.

## Explicit non-goals (not touched)

- `ResourcePaths` redesign.
- `ObjectFactory` / `ResourceService` rewrite.
- Settings / SceneManagement foundation.
- UI / Audio / Camera rewrite.
- asmdef architecture beyond existing Core/Infrastructure/Editor split.
- gameplay systems and FishNet.
