# PlatformCore 2.0 — Usage Guide

Версия документа: 1.0  
Статус: practical guide для текущего runtime-ready baseline

---

## 1) Что такое текущий PlatformCore

Текущий `PlatformCore` — это **чистый platform baseline** для Unity-проектов:

- bootstrap + game root + service registration + lifecycle orchestration;
- reusable foundation-слои (UI, settings, scene, audio, camera, notifications, async awaiter);
- узкий network entry layer под FishNet;
- первые reusable gameplay-блоки поверх foundation.

Это **не** "полный универсальный game framework".

---

## 2) Что уже входит в платформу

### Foundation/runtime

- `BaseBootstrap`, `BaseGameRoot`, `PersistentSceneContext`;
- `ServiceLocator` + lifecycle orchestration через `LifecycleService`;
- composition foundation (`Composite`, `CompositeInstaller`, `CompositeBuilder`);
- foundation registration extensions:
  - `RegisterUIFoundation(...)`;
  - `RegisterSettingsFoundation(...)`;
  - `RegisterSceneManagementFoundation(...)`;
  - `RegisterGlobalNotificationsFoundation(...)`;
  - `RegisterAsyncAwaiterFoundation(...)`;
  - `RegisterFishNetFoundation(...)`.

### Foundation-слои

- UI;
- Settings;
- SceneManagement;
- Audio;
- Camera;
- Notifications;
- AsyncAwaiter;
- FishNet foundation/session layer.

### Gameplay-level reusable blocks (уже есть)

- `SettingsComposite`;
- `PauseMenuComposite`;
- reusable first-person / third-person camera gameplay layer.

---

## 3) Что сознательно не входит в платформу

- localization subsystem;
- reusable character controller;
- полный multiplayer gameplay framework;
- shop/level/dialogue готовые игровые подсистемы;
- обязательные `PlayerComposite`, `ShopComposite`, `LevelComposite`.

Также: `Composite` — это не default-решение для любой новой фичи.

---

## 4) Базовый runtime stack в новой игре

## 4.1 Bootstrap

Сцена содержит компонент-наследник `BaseBootstrap`, который создаёт ваш `GameRoot`.

## 4.2 Game root

Ваш наследник `BaseGameRoot`:

1. в `RegisterServices(...)` регистрирует platform services + game services;
2. в `LaunchGameAsync(...)` создаёт и активирует gameplay runtime parts;
3. runtime updates идут через `LifecycleService` (`Update/FixedUpdate/LateUpdate`).

## 4.3 Service registration

Минимальный практический порядок:

1. register logger/resource/factory/audio/camera, если используются в проекте;
2. register foundation extensions (`UI`, `Settings`, `SceneManagement`, `Notifications`, `AsyncAwaiter`);
3. опционально register `FishNet foundation`, если игра network-enabled;
4. затем создавать gameplay-level composites/controllers.

---

## 5) Практическая схема подключения в новой игре

## 5.1 Extension points, которые уже есть

Используйте существующие точки входа в `ServiceLocator`:

- foundation registration methods из `PlatformCore/Infrastructure/*/ServiceLocator*Extensions.cs`;
- gameplay constructors из `PlatformCore/Gameplay/*/ServiceLocator*Extensions.cs`.

## 5.2 Куда добавлять свою gameplay-логику

Рекомендуемый минимальный путь:

- оставлять platform foundation в `PlatformCore/*` как базу;
- игровые сценарии добавлять отдельными game-level controllers/composites в вашем проектном слое;
- подписки/оркестрацию держать в контроллерах (activate/deactivate symmetry);
- gameplay state хранить в моделях/сервисах игры, а не в platform foundation.

## 5.3 Где заканчивается платформа и начинается игра

**Платформа** заканчивается там, где кончаются reusable foundation contracts и общий runtime orchestration.  
**Игра** начинается там, где появляется game-specific контент/правила/баланс/прогресс/мета.

Практический тест границы:

- если компонент нужен большинству игр как инфраструктура — это кандидат в PlatformCore;
- если логика зависит от конкретного жанра/контента/баланса — это game layer.

---

## 6) Короткий checklist запуска новой игры

1. Создать свой `BaseBootstrap` + `BaseGameRoot` наследник.
2. В `RegisterServices(...)` зарегистрировать foundation-слои, которые реально нужны игре.
3. Подключить settings appliers для audio/camera при использовании соответствующих сервисов.
4. Создать и активировать нужные gameplay-blocks (`PauseMenuComposite`, `SettingsComposite`, свои блоки).
5. (Опционально) подключить FishNet foundation/session registration.
6. Проверить, что game-specific системы не добавляются в platform namespace/слой.

---

## 7) Что не надо тащить в PlatformCore

Не переносите в `PlatformCore`:

- game-specific player controller;
- конкретные gameplay feature frameworks (shop/quests/dialogue/level flow);
- специфичные для проекта content paths и балансные правила;
- workaround-код, который маскирует проблемы конкретной игры.

Правило: если это можно переиспользовать как platform baseline без знания вашей конкретной игры — только тогда это кандидат в PlatformCore.
