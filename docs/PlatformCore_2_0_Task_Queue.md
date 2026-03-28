# PlatformCore 2.0 — Task Queue

Версия документа: 1.1
Статус: очередь после foundation sanity-pass (с закрытыми baseline-этапами)
Назначение: использовать как короткий operational backlog для ближайших PR

---

## Общий принцип

Сейчас `PlatformCore` уже содержит imported baseline.

Поэтому ближайшие задачи должны:

- стабилизировать уже имеющийся foundation;
- уменьшать legacy-долг;
- улучшать границы модулей;
- не расширять платформу раньше времени.

---

## Закрытые пачки foundation (исторически выполнены)

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

> Важно: localization остаётся вне PlatformCore, notifications остаются platform-level, а minimal async awaiter foundation уже присутствует в текущем baseline.

---

## Следующая активная пачка (decision point)

### Пачка A. Reusable Gameplay Layer (кандидат №1)

### Цель

Начать следующий крупный этап после почти закрытого foundation.

### Что сделать

- выбрать 1–2 первых reusable gameplay-composites с понятным platform value;
- опираться на уже нормализованный foundation без возврата к platform rewrite;
- зафиксировать границы gameplay vs foundation на уровне installers/composition root.

### Что не трогать

- network/FishNet;
- новый framework;
- большой rename-driven cleanup foundation;
- возврат localization в PlatformCore.

### Acceptance criteria

- foundation не получает новых subsystem changes;
- gameplay layer стартует как отдельный, читаемый трек.

---

## Альтернативная маленькая пачка перед gameplay (опционально)

### Пачка B. Composition sanity cleanup (только если реально нужен)

### Цель

Сделать короткий polish `Composite / Installer / composition root`, если найдены остаточные хвосты после foundation cleanup.

### Что сделать

- убрать только очевидные stale comments/notes/ownership хвосты;
- синхронизировать composition registration flow без расширения архитектуры;
- оставить поведение совместимым с текущим foundation.

### Что не трогать

- gameplay features;
- новый composition framework;
- network/FishNet;
- новый subsystem или большой refactor.

### Acceptance criteria

- cleanup маленький и локальный;
- улучшена читаемость/предсказуемость composition entry points;
- следующий шаг к reusable gameplay остаётся прямым.

---

## Пока не брать

Не брать в ближайшие пачки:

- analytics;
- FishNet;
- gameplay composites;
- gameplay UI;
- vendor-specific extensions beyond what already imported.

---

## Network track — первый минимальный стартовый шаг

### Пачка C. FishNet foundation / extension bootstrap (минимальный)

### Цель

Открыть network track самым узким platform-level шагом без ввода gameplay networking framework.

### Что сделано в минимальном шаге

- добавлен узкий `INetworkSessionService` + `INetworkSessionBridge` контракт для состояния сессии;
- добавлен `FishNetSessionBridge` как минимальный bridge-слой;
- добавлен `ServiceLocatorFishNetExtensions.RegisterFishNetFoundation(...)` для стандартной registration/wiring интеграции с текущим foundation и lifecycle.

### Что принципиально не входит в scope

- reusable networked character controller;
- player spawning framework;
- game-specific multiplayer logic;
- большой wrapper поверх FishNet API.
