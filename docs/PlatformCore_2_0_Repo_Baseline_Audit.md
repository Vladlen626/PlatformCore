# PlatformCore 2.0 — Repo Baseline Audit

Версия документа: 1.0  
Статус: аудит текущего состояния репозитория `PlatformCore` после первого большого legacy import  
Назначение: использовать как карту текущего baseline перед stabilization, modular split и selective rewrite

---

## 1. Зачем нужен этот документ

Репозиторий `PlatformCore` уже не находится в состоянии пустого каркаса.

В него уже импортирован крупный baseline из D6-Express. Поэтому дальше нельзя планировать работу как будто foundation ещё только предстоит перенести.

Этот документ фиксирует:

- что уже реально находится в репозитории;
- какие части выглядят как reusable platform foundation;
- какие части несут legacy/project-specific хвосты;
- что нужно оставить;
- что нужно адаптировать;
- что нужно переписать;
- что нужно вынести из foundation слоёв.

---

## 2. Краткий вывод по состоянию репозитория

Текущее состояние `PlatformCore` можно описать так:

- foundation baseline уже импортирован;
- импорт сделан близко к legacy-оригиналу;
- это лучше, чем greenfield rewrite, но база пока сырая;
- runtime собран как монолит;
- editor tooling и sample/demo payload ещё не отделены;
- часть imported code уже platform-level;
- часть imported code явно project-specific и требует выноса или разделения.

Главный practical вывод:

**сейчас правильная работа — не переносить дальше всё подряд, а стабилизировать и разрезать уже существующий baseline.**

---

## 3. Что уже есть в репозитории

### 3.1 Foundation / bootstrap

Уже присутствуют:

- `ApplicationLifetimeService`
- `BaseBootstrap`
- `BaseGameRoot`
- `PersistentSceneContext`

### 3.2 Lifecycle

Уже присутствуют:

- `IBaseController`
- lifecycle interfaces
- `LifecycleService`

### 3.3 UI-related base

Уже присутствуют:

- `BaseContextController<T>`
- UI style editor tooling
- UI-related resource paths

### 3.4 Factory / resources / config

Уже присутствуют:

- `ConfigLoader`
- `ConfigService`
- `BaseConfig`
- `ObjectFactory`
- `ResourceService`
- `ResourcePaths`

### 3.5 Audio

Уже присутствуют:

- `IAudioService`
- `AudioBaseService`

### 3.6 Camera

Уже присутствуют:

- `ICameraService`
- `ICameraShakeService`
- `PlayerCameraService` / `CameraService`
- `CinemachineCameraRegister`

### 3.7 Editor

Уже присутствуют:

- `PlatformCore.Editor.asmdef`
- editors для text/color styles
- editor helper для UI background sizing

### 3.8 Misc foundation

Уже присутствуют:

- trigger foundation
- placeholder materials / textures
- generated resource path file

### 3.9 Notifications

Уже присутствуют:

- `IGlobalNotificationService`;
- `GlobalNotificationService`;
- banner/toast UI integration.

Примечание: localization subsystem не считается частью целевого PlatformCore и не входит в platform foundation scope.

---

## 4. Общая оценка слоёв

### 4.1 Что выглядит как хороший кандидат на сохранение

Это части, которые уже выглядят как platform foundation и должны быть сохранены как baseline с точечными исправлениями:

- `ApplicationLifetimeService`
- `BaseBootstrap`
- `BaseGameRoot`
- `PersistentSceneContext`
- `LifecycleService`
- `IBaseController` и lifecycle contracts
- `BaseContextController<T>` как legacy UI pattern
- `ObjectFactory` / `ResourceService` как candidates на foundation API
- `AudioBaseService` как legacy baseline for normalization
- `CameraService` / `PlayerCameraService` как legacy baseline for normalization
- `PlatformCore.Editor` как отдельный editor-layer

### 4.2 Что точно требует адаптации

Это части, которые не нужно выкидывать, но нельзя оставлять в текущем виде:

- `LifecycleService`
- `BaseContextController<T>`
- `ObjectFactory`
- `ResourceService`
- `AudioBaseService`
- `CameraService` / `PlayerCameraService`
- `ConfigLoader` / `ConfigService`

### 4.3 Что выглядит как project-specific хвост и требует выноса

Это части, которые не должны оставаться внутри platform foundation как есть:

- project-specific entries в `ResourcePaths`
- game-specific camera states
- sample/demo assets внутри foundation дерева
- UI prefab paths конкретной игры
- analytics resource entries
- игровые item/shop/player resource paths

---

## 5. Ключевые проблемы baseline

## 5.1 Монолитный asmdef

Сейчас runtime собран через общий `PlatformCore.asmdef`.

Он уже тянет внешние зависимости вроде:

- UniTask
- TMPro
- FMOD
- Cinemachine
- Splines
- DOTween Modules
- Newtonsoft.Json

Это значит:

- Core ещё не изолирован;
- Infrastructure ещё не изолирован;
- UI/Audio/Camera не выделены в самостоятельные модули;
- foundation не может считаться чистым по зависимостям.

### Статус
**rewrite/adapt обязательно**

---

## 5.2 LifecycleService содержит архитектурный долг

`LifecycleService` уже можно использовать как baseline, но текущая реализация не должна считаться финальной.

Главные проблемы:

- нет явной защиты от duplicate registration;
- unregister/dispose семантика недостаточно зафиксирована;
- одновременно используются `IActivatable` и `IDeactivatable`;
- регистрация update-интерфейсов через `switch` не позволяет одному controller одновременно быть, например, и `IUpdatable`, и `ILateUpdatable`;
- group registration API есть, но не зафиксирован как контракт platform-level поведения.

### Статус
**keep as baseline + fix immediately**

---

## 5.3 BaseContextController<T> уже есть, но требует нормализации

Это хороший знак: legacy UI pattern уже перенесён, и не нужно выдумывать его с нуля.

Но текущий baseline надо перепроверить по нескольким вопросам:

- правильна ли семантика `PreloadAsync -> Activate -> GetWindow -> Unload`;
- не является ли `Deactivate()` слишком жёстким через обязательный `Unload<T>()`;
- должен ли foundation-level controller всегда обнулять контекст таким образом;
- что является platform-level обязанностью `IUIService`, а что относится к конкретной игре.

### Статус
**keep + adapt during UI normalization**

---

## 5.4 ResourcePaths — главный источник project-specific payload

Текущий `ResourcePaths` выглядит как прямой импорт из проекта, а не как platform-level ресурсная карта.

Там уже находятся:

- shop paths;
- item paths;
- player/npc paths;
- dice-related paths;
- конкретные UI prefabs игры;
- analytics settings path;
- игровые json-конфиги;
- тексты конкретного проекта.

Это значит:

- сам механизм generated resource paths можно сохранить;
- содержимое текущего файла нельзя считать foundation;
- файл нужно или разделить, или вынести project-specific entries в sample/demo/game layer.

### Статус
**split / move / partially rewrite**

---

## 5.5 Audio baseline usable, but vendor-coupled

`AudioBaseService` — рабочий импортированный baseline, но сейчас он сильно привязан к FMOD runtime.

Проблемы:

- platform API и FMOD-specific implementation не разведены;
- volume policy живёт прямо внутри service implementation;
- settings integration ещё не оформлена;
- сервис пока трудно считать neutral foundation API.

### Статус
**keep as baseline + normalize later**

---

## 5.6 Camera baseline imported, but currently game-specific

Camera слой уже импортирован, но сейчас там видны признаки project-specific design.

Главный маркер — `CameraStateEnum` уже содержит игровые состояния вроде:

- `MainMenu`
- `TrainWatch`
- `DiceGame`
- `DiceGameCombinations`
- `Inventory`

Это хороший legacy baseline, но не platform-level финальная форма.

### Что с этим делать

- сохранить service как baseline;
- вынести/убрать game-specific states;
- придумать reusable camera policy;
- оставить игровой набор режимов для sample/game layer, а не для foundation.

### Статус
**keep as baseline + rewrite specific parts**

---

## 5.7 Runtime, editor and sample payload пока смешаны

Сейчас в дереве репозитория рядом живут:

- runtime services;
- editor tools;
- placeholder materials;
- texture packs;
- generated content;
- UI resource references.

Такое состояние нормально как промежуточный legacy import, но не как зрелый PlatformCore.

### Статус
**must split**

---

## 6. Карта статусов по основным блокам

| Блок | Статус | Решение |
|---|---|---|
| ApplicationLifetimeService | usable baseline | keep |
| BaseBootstrap | usable baseline | keep |
| BaseGameRoot | usable baseline | keep + adapt |
| PersistentSceneContext | reusable baseline | keep |
| LifecycleService | reusable but flawed | fix immediately |
| IBaseController + lifecycle interfaces | foundation contracts | keep |
| BaseContextController<T> | reusable UI baseline | keep + adapt |
| ConfigLoader / ConfigService | partially reusable | audit + adapt |
| ObjectFactory | reusable candidate | keep + adapt |
| ResourceService | reusable candidate | keep + adapt |
| ResourcePaths | mixed project payload | split / move |
| AudioBaseService | vendor-coupled baseline | keep + normalize |
| CameraService / PlayerCameraService | game-specific baseline | keep + rewrite parts |
| PlatformCore.Editor | correct separate concern | keep |
| Placeholder materials / textures | sample/demo payload | move out of foundation |
| Game-specific resource entries | not foundation | remove from platform layer |

---

## 7. Что делать следующим

Порядок ближайших шагов должен быть таким:

1. `LifecycleService stabilization`
2. `asmdef split: Core / Infrastructure / Editor`
3. `ResourcePaths / Factory / ResourceService audit`
4. `sample/demo asset cleanup`
5. `Settings foundation`
6. `SceneManagement foundation`
7. `UI normalization`
8. `Audio normalization`
9. `Camera normalization`

---

## 8. Что пока не делать

Пока не делать:

- gameplay systems;
- gameplay UI;
- shop / inventory / HUD / pause menu;
- analytics;
- FishNet;
- большой rename-driven rewrite всего репозитория.

---

## 9. Главный вывод

`PlatformCore` уже содержит реальную foundation base.

Значит дальнейшая работа должна идти не по логике «что бы ещё перенести», а по логике:

- что из уже импортированного является основой платформы;
- что из уже импортированного ломает чистоту платформы;
- что нужно исправить сейчас, чтобы следующие переносы не ухудшали ситуацию.

Текущий baseline хороший как отправная точка, но его нужно:

- стабилизировать;
- почистить;
- разрезать по модулям;
- и только потом расширять дальше.
