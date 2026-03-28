# PlatformCore 2.0 — Module Split Spec

Версия документа: 1.0  
Статус: рабочая спецификация на разрезание текущего legacy-монолита по модулям 2.0  
Назначение: использовать как guide для asmdef split и наведения порядка в зависимостях

---

## 1. Цель

Сейчас runtime-код находится в одном `PlatformCore.asmdef`.

Это допустимо как промежуточное состояние после legacy import, но дальше это мешает:

- изоляции Core;
- валидации зависимостей;
- чистому foundation API;
- дальнейшему переносу settings/scene/UI;
- локальному переписыванию отдельных подсистем.

Цель этого документа:

**разрезать текущий baseline на целевые модули 2.0 без big-bang rewrite.**

---

## 2. Главные правила split

### 2.1 Split не равен rewrite

Разделение сборок не должно сопровождаться одновременной полной переработкой логики.

Сначала:

- физически отделяем слои;
- выравниваем ссылки;
- убираем лишние зависимости.

Потом:

- переписываем проблемные реализации локально.

### 2.2 Core должен быть максимально лёгким

В `PlatformCore.Core` не должны жить прямые зависимости на:

- FMOD;
- Cinemachine;
- TMPro;
- DOTween;
- Splines;
- editor API;
- sample assets.

### 2.3 Infrastructure зависит на Core, но не наоборот

`Infrastructure` может зависеть от `Core`.  
`Core` не должен зависеть от `Infrastructure`.

### 2.4 UI / Audio / Camera / Settings / SceneManagement должны быть отдельными слоями

Они не должны оставаться «папками внутри одного runtime asmdef».

### 2.5 Editor и Samples всегда отдельно

- `PlatformCore.Editor` не должен смешиваться с runtime;
- `PlatformCore.Samples` не должен загрязнять foundation.

### 2.6 Localization вне scope PlatformCore

- Localization intentionally stays outside PlatformCore foundation scope.
- Platform runtime модули не должны требовать `ILocalizationService`/localization registration для базовой работы.
- Global in-app notifications остаются platform-level слоем и работают на raw message data.

---

## 3. Целевые модули

## 3.1 PlatformCore.Core

### Ответственность

Минимальные runtime contracts и composition base.

### Кандидаты на перенос

- `IBaseController`
- lifecycle-related interfaces
- `Composite`
- `Installer`
- composition utility types
- возможно минимальные service contracts без внешних vendor-dependencies

### Что не должно лежать здесь

- `LifecycleService`
- `BaseBootstrap`
- `BaseGameRoot`
- `ObjectFactory`
- UI runtime
- FMOD
- Cinemachine
- TMPro

---

## 3.2 PlatformCore.Infrastructure

### Ответственность

Application/bootstrap/runtime orchestration.

### Кандидаты на перенос

- `ApplicationLifetimeService`
- `BaseBootstrap`
- `BaseGameRoot`
- `PersistentSceneContext`
- `LifecycleService`
- возможно service registration / locator infrastructure

### Зависимости

- зависит от `PlatformCore.Core`

---

## 3.3 PlatformCore.UI

### Ответственность

Platform-level UI foundation.

### Кандидаты на перенос

- `BaseContextController<T>`
- `IUIService`
- `UIBaseElement`
- UI service base
- layers / cursor / helpers
- позже global notifications

### Особое правило

Runtime UI и editor style tooling должны быть разведены.

---

## 3.4 PlatformCore.Audio

### Ответственность

Platform audio API и его реализация.

### Кандидаты на перенос

- `IAudioService`
- `AudioBaseService`
- audio settings bridge later

### Особое правило

Если FMOD остаётся текущей реализацией, это должно быть явно видно на уровне module ownership и зависимостей.

---

## 3.5 PlatformCore.Settings

### Ответственность

Settings model, persistence, notifications, appliers.

### Кандидаты

- пока модуль создаётся позже, после split foundation

---

## 3.6 PlatformCore.SceneManagement

### Ответственность

Scene loading, loading flow, persistent/gameplay scene orchestration.

### Кандидаты

- `PersistentSceneContext`
- scene loader/services later

### Примечание

На transitional step `PersistentSceneContext` может временно оставаться в Infrastructure, пока не оформлен полноценный scene module.

---

## 3.7 PlatformCore.Camera

### Ответственность

Reusable camera foundation.

### Кандидаты на перенос

- `ICameraService`
- `ICameraShakeService`
- `CameraService` / `PlayerCameraService`
- `CinemachineCameraRegister`

### Особое правило

Игровые camera states не должны оставаться foundation contract.

---

## 3.8 PlatformCore.Editor

### Ответственность

Editor tooling only.

### Кандидаты на перенос

- `PlatformCore.Editor.asmdef`
- style editors
- drawers
- UI editor helpers

---

## 3.9 PlatformCore.Samples

### Ответственность

Sample/demo assets и демонстрационные runtime parts.

### Кандидаты на перенос

- placeholder materials
- texture packs
- demo resources
- проектные примеры, если они останутся

---

## 4. Желаемая карта зависимостей

```text
PlatformCore.Core
  ↑
PlatformCore.Infrastructure
  ↑
PlatformCore.Settings
PlatformCore.SceneManagement
PlatformCore.UI
PlatformCore.Audio
PlatformCore.Camera

PlatformCore.Gameplay
  depends on Core / Infrastructure / UI / Audio / Settings / SceneManagement / Camera

PlatformCore.Network.FishNet
  depends on Gameplay and selected foundation modules

PlatformCore.Editor
  depends on runtime modules as needed

PlatformCore.Samples
  depends on runtime modules, never наоборот
```

### Правило

Основание графа — `Core`, а не UI/Audio/Camera.

---

## 5. Практический порядок split

### Шаг 1. Выделить Core

Сначала вынести минимальные контракты и базовые interfaces.

### Шаг 2. Выделить Infrastructure

Перенести bootstrap и lifecycle foundation.

### Шаг 3. Оставить Editor отдельным

Проверить, что editor tooling не сидит в runtime tree без необходимости.

### Шаг 4. Подготовить UI / Audio / Camera boundaries

Пока можно не делать идеальный cleanup реализации, но физически отделить ownership слоёв.

### Шаг 5. Отдельно оформить Settings и SceneManagement

Не раньше, чем Foundation split уже устойчив.

---

## 6. Что нельзя делать во время split

Нельзя:

- одновременно делать полный rewrite lifecycle;
- одновременно придумывать новый UI framework;
- тащить gameplay systems;
- оставлять game-specific enums и resource paths как foundation contracts;
- превращать asmdef split в многонедельный rename-driven refactor.

---

## 7. Acceptance Criteria

Module split считается успешным, если:

- `Core` больше не тянет vendor/runtime-specific пакеты;
- `Infrastructure` отделён от UI/audio/camera;
- `Editor` физически отделён от runtime;
- `Samples` не живут в foundation runtime tree как обязательная часть платформы;
- следующий PR можно делать уже на уровне отдельного модуля, а не на уровне всего монолита.
