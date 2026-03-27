# PlatformCore 2.0 — Task Queue

Версия документа: 1.0  
Статус: рабочая очередь задач после baseline import  
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

## Пачка 1. LifecycleService stabilization

### Цель

Сделать lifecycle foundation безопасным и предсказуемым.

### Что сделать

- добавить защиту от duplicate registration;
- сделать `Unregister` идемпотентным;
- исправить регистрацию controller, который реализует несколько update-интерфейсов;
- зафиксировать семантику `Activate` / `Deactivate` / `Dispose`;
- перепроверить group registration APIs;
- добавить минимальные runtime guards и комментарии по expected behavior.

### Что не трогать

- asmdef split;
- UI rewrite;
- audio/camera;
- settings;
- scene management.

### Acceptance criteria

- один controller не регистрируется дважды;
- multi-interface controller обновляется во всех нужных фазах;
- повторный unregister безопасен;
- dispose не ломает внутренние списки lifecycle.

---

## Пачка 2. Core / Infrastructure / Editor split

### Цель

Разрезать foundation на первые реальные модули.

### Что сделать

- выделить `PlatformCore.Core`;
- выделить `PlatformCore.Infrastructure`;
- оставить `PlatformCore.Editor` отдельной assembly;
- убрать тяжёлые vendor/runtime references из Core;
- привести namespace ownership к новой модульной карте.

### Что не трогать

- settings foundation;
- scene management foundation;
- полный UI/audio/camera cleanup.

### Acceptance criteria

- `Core` собирается без FMOD/Cinemachine/TMP dependencies;
- `Infrastructure` зависит от `Core`, но не наоборот;
- `Editor` не смешан с runtime.

---

## Пачка 3. Baseline payload cleanup

### Цель

Убрать лишний sample/demo/project-specific payload из foundation дерева.

### Что сделать

- пересмотреть placeholder materials и textures;
- определить, что остаётся sample content, а что удаляется;
- проверить generated assets и generated source files;
- убрать случайный project payload из foundation слоёв.

### Что не трогать

- lifecycle logic;
- settings implementation;
- network;
- gameplay systems.

### Acceptance criteria

- foundation tree не содержит случайный demo payload как обязательную часть runtime;
- sample assets вынесены или явно помечены как sample/demo.

---

## Пачка 4. Resource / Factory audit and cleanup

### Цель

Отделить reusable resource/factory layer от project-specific структуры D6.

### Что сделать

- перепроверить `ObjectFactory`;
- перепроверить `ResourceService`;
- перепроверить `ResourcePaths`;
- отделить platform-level entries от project/game-specific entries;
- определить, нужен ли split на runtime foundation paths и sample/game paths.

### Что не трогать

- полный scene management;
- gameplay resource graphs;
- analytics.

### Acceptance criteria

- `ResourcePaths` больше не выглядит как карта ресурсов конкретной игры;
- `ObjectFactory` и `ResourceService` имеют понятную platform responsibility.

---

## Пачка 5. Settings foundation

### Цель

Добавить нормальный platform-level слой настроек.

### Что сделать

- `SettingsService`;
- data model;
- persistence;
- notifications;
- appliers.

### Что не трогать

- settings menu;
- gameplay options;
- full audio rewrite;
- full camera rewrite.

### Acceptance criteria

- настройки сохраняются и читаются через единый platform service;
- audio/camera могут интегрироваться через settings layer.

---

## Пачка 6. SceneManagement foundation

### Цель

Оформить scene loading и transitions как отдельный platform module.

### Что сделать

- scene loader;
- loading flow;
- persistent/gameplay scene split;
- orchestration point для переходов.

### Что не трогать

- game-specific scene names;
- quest/mission flows;
- network scene logic.

### Acceptance criteria

- есть единый scene management слой;
- scene orchestration не размазана по bootstrap и runtime services.

---

## Пачка 7. UI normalization

### Цель

Нормализовать уже импортированный UI baseline.

### Что сделать

- перепроверить `BaseContextController<T>`;
- оформить runtime UI base;
- ввести минимальные layers/cursor/helpers;
- отделить runtime UI от editor style tooling.

### Что не трогать

- gameplay UI;
- pause menu;
- HUD;
- inventory;
- settings menu.

### Acceptance criteria

- есть platform-level UI foundation;
- нет смешения с gameplay UI.

---

## Пачка 8. Audio normalization

### Цель

Довести imported audio baseline до platform-level качества.

### Что сделать

- определить границу between platform API and FMOD implementation;
- связать с settings;
- убрать лишнюю project-specific политику из сервиса.

---

## Пачка 9. Camera normalization

### Цель

Довести imported camera baseline до reusable platform module.

### Что сделать

- убрать игровые camera states из foundation contract;
- выровнять API;
- связать с settings;
- сохранить shake/basic camera functionality.

---

## Пачка 10. Localization + Global Notifications

### Цель

После stabilization foundation перенести следующие reusable UI-related слои.

### Что сделать

- localization foundation;
- global in-app notifications;
- без переноса D6 prefab pack как обязательной части foundation.

---

## Пока не брать

Не брать в ближайшие пачки:

- analytics;
- FishNet;
- gameplay composites;
- gameplay UI;
- vendor-specific extensions beyond what already imported.
