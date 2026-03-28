# PlatformCore 2.0 — Task Queue

Версия документа: 1.2
Статус: финальный docs sync после закрытия foundation-phase и запуска gameplay/network foundation tracks
Назначение: короткий operational backlog без расширения scope платформы

---

## Общий принцип

Сейчас `PlatformCore` находится в состоянии **platform baseline ready**:

- foundation-этап по сокращённому плану практически завершён;
- reusable gameplay layer уже начат;
- FishNet foundation/session runtime entry layer уже есть;
- следующий шаг — не новый subsystem, а поддержание чистых границ platform vs game.

---

## Закрытые и зафиксированные foundation-пачки

- Пачка 1. `LifecycleService stabilization` — выполнено.
- Пачка 2. `Core / Infrastructure / Editor split` — выполнено.
- Пачка 3. `Baseline payload cleanup` — выполнено.
- Пачка 4. `Resource / Factory audit and cleanup` — выполнено.
- Пачка 5. `Settings foundation` — выполнено.
- Пачка 6. `SceneManagement foundation` — выполнено.
- Пачка 7. `UI normalization` — выполнено.
- Пачка 8. `Audio normalization` — выполнено.
- Пачка 9. `Camera normalization` — выполнено.
- Пачка 10. `Global Notifications (без Localization)` — выполнено.
- Пачка 11. `Minimal AsyncAwaiter foundation` — выполнено.
- Пачка 12. `Composition foundation` — выполнено.

> Важно: localization остаётся вне PlatformCore; notifications остаются platform-level; platform не включает готовый reusable character controller.

---

## Reusable gameplay layer — текущий статус

Трек уже начат в runtime:

- `SettingsComposite` — есть;
- `PauseMenuComposite` — есть;
- reusable camera gameplay layer для first-person / third-person — есть.

### Границы трека

- это **не** переход к "полноценному game framework";
- `PlayerComposite`, `ShopComposite`, `LevelComposite` не являются обязательной частью текущего PlatformCore scope;
- `Composite` не является default-решением для любой фичи.

---

## Network track — текущий статус

FishNet foundation track уже доведён до узкого runtime-ready состояния:

- session foundation (`INetworkSessionService`, `INetworkSessionBridge`, `FishNetSessionBridge`) — есть;
- registration entry points (`ServiceLocatorFishNetExtensions.RegisterFishNetFoundation(...)`) — есть;
- runtime callback adapter (`FishNetRuntimeSessionAdapter`) — есть;
- role cleanup в network layer (service/controller split) — есть.

### Что принципиально не входит в scope

- reusable networked character controller;
- player spawning framework;
- full multiplayer gameplay framework;
- game-specific multiplayer orchestration.

---

## Ближайший backlog (без нового subsystem)

1. Финальный docs sync по фактическому repo state.
2. Практическая usage-документация для запуска новой игры на текущем baseline.
3. Небольшой boundary-polish только при явной необходимости (без redesign).

---

## Пока не брать

- localization subsystem внутрь PlatformCore;
- analytics subsystem;
- shop/level/dialogue frameworks;
- gameplay-specific character controller в platform scope;
- новый общий framework поверх текущего baseline.
