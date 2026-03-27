# PlatformCore 2.0 — Implementation Plan

Версия документа: 3.0  
Статус: рабочая спецификация после фактического импорта foundation baseline  
Назначение: использовать как основной план работ по доведению текущего репозитория `PlatformCore` до управляемого состояния PlatformCore 2.0

---

## 1. Текущий контекст

PlatformCore 2.0 больше не находится в сценарии «создать репозиторий и начать перенос с нуля».

На текущий момент в репозитории `PlatformCore` уже есть первый большой импорт legacy-базы из D6-Express. Это не пустой каркас, а рабочий baseline, перенесённый почти как есть.

Значит текущая задача меняется.

Теперь нужно не проектировать foundation заново, а:

1. зафиксировать, что уже реально находится в репозитории;
2. определить, какие части являются platform-level базой, а какие являются legacy-хвостами;
3. стабилизировать уже импортированный baseline;
4. постепенно разрезать его на целевые модули 2.0;
5. точечно переписывать и исправлять уже перенесённый код, а не заменять его новой абстрактной архитектурой.

Главный принцип этого документа:
`PlatformCore` уже существует как imported baseline, поэтому план реализации теперь строится вокруг **audit → stabilization → modularization → selective rewrite**.

---

## 2. Цель PlatformCore 2.0 на текущем этапе

PlatformCore 2.0 — это слой над Unity для быстрой сборки новых игр из повторно используемых систем.

Но текущая практическая цель этапа формулируется так:

**не сделать новый foundation с нуля, а довести уже импортированную базу до состояния чистой, устойчивой и модульной платформы.**

### Цели текущей фазы

- сохранить уже перенесённую рабочую основу;
- убрать project-specific и demo-specific хвосты;
- исправить дефекты imported baseline;
- разделить монолитную assembly на целевые модули;
- довести foundation до состояния, удобного для дальнейших переносов;
- только после этого переносить и нормализовать UI, settings, scene management и другие подсистемы.

### Не-цели текущей фазы

- не переписывать всё ради «красивой новой архитектуры»;
- не начинать FishNet раньше стабилизации локальной платформы;
- не тащить в foundation gameplay-specific код;
- не оставлять imported code в виде бесконтрольного legacy-монолита;
- не смешивать перенос нового слоя и глубокую переработку всей старой базы в одном шаге.

---

## 3. Что уже есть в репозитории

Ниже зафиксирован baseline, который уже присутствует в `PlatformCore` и должен учитываться как отправная точка плана.

### 3.1 Уже импортированные foundation-слои

В репозитории уже присутствуют:

- `ApplicationLifetimeService`;
- `BaseBootstrap`;
- `BaseGameRoot`;
- `PersistentSceneContext`;
- `LifecycleService` и lifecycle interfaces;
- `BaseContextController` и базовая UI-related инфраструктура;
- `ConfigLoader` и config foundation;
- `ConfigService`;
- `ObjectFactory`, `ResourceService`, `ResourcePaths`;
- `AudioBaseService` и `IAudioService`;
- `PlayerCameraService`, `ICameraService`, `ICameraShakeService`, `CinemachineCameraRegister`;
- editor-инструменты для text/color styles;
- trigger foundation.

### 3.2 Что это означает для плана

Это уже не Milestone вида «собрать skeleton и начать foundation import».

Это уже стадия:

- baseline imported;
- baseline частично компилируется как legacy extract;
- baseline содержит технический долг переноса;
- baseline нужно вычистить и разложить по модулям.

Именно поэтому все дальнейшие документы и задачи должны исходить не из greenfield-сценария, а из текущего состояния репозитория.

---

## 4. Зафиксированные проблемы текущего baseline

Эти пункты считаются не пожеланиями, а обязательной частью следующего этапа.

### 4.1 Монолитный runtime asmdef

Сейчас foundation собран через общий `PlatformCore.asmdef`, в который уже зашиты прямые ссылки на внешние зависимости вроде UniTask, TextMeshPro, FMOD, Cinemachine, Splines, DOTween Modules и Newtonsoft.

Это означает:

- Core не изолирован;
- Infrastructure не изолирован;
- UI, Audio, Camera и Editor пока не отделены по зависимостям;
- любая одна runtime-сборка тянет за собой лишние пакеты.

### 4.2 В imported foundation уже есть смешение platform-level и project-level логики

В текущем baseline уже попали части, которые требуют ревизии:

- `ResourcePaths` может нести project-specific структуру путей;
- camera states уже содержат игровые режимы;
- аудио завязано на конкретный FMOD-style runtime;
- часть asset-контента импортирована вместе с кодом foundation.

### 4.3 В foundation смешаны runtime code, editor code и sample/demo assets

В репозитории уже есть editor-код, placeholder materials и texture-наборы.

Это значит, что следующий этап должен явно разделить:

- runtime foundation;
- editor tooling;
- sample/demo content.

### 4.4 Lifecycle foundation требует не только переноса, но и исправления

Текущий `LifecycleService` уже является рабочей основой, но baseline содержит конкретные архитектурные проблемы:

- нет ясной защиты от повторной регистрации;
- поведение unregister/dispose нужно сделать идемпотентным;
- одновременно используются `IActivatable` и `IDeactivatable`, что создаёт рассинхрон семантики;
- регистрация update-типов через `switch` ломает сценарий, когда один контроллер реализует несколько lifecycle-интерфейсов одновременно;
- group registration нужно зафиксировать как предсказуемую часть API, а не как побочный helper.

### 4.5 Не все целевые foundation-модули реально оформлены как модули

Хотя код уже импортирован, целевая структура 2.0 пока не достигнута:

- `Settings` как цельный слой отсутствует;
- `SceneManagement` как цельный слой отсутствует;
- `Localization` начат, но не доведён;
- `UI` находится скорее в состоянии частичного legacy import, чем законченного platform module;
- `Audio` и `Camera` пока скорее imported services внутри legacy-сборки, чем завершённые независимые модули.

---

## 5. Новая стратегия реализации

### 5.1 Главный принцип

Следующие шаги по PlatformCore 2.0 выполняются не как перенос с нуля, а как **controlled rewrite поверх imported baseline**.

Порядок принятия решений должен быть таким:

1. сначала понять, что уже импортировано;
2. затем отделить reusable foundation от project-specific хвостов;
3. затем исправить явные дефекты;
4. затем разрезать зависимости и выровнять модульную структуру;
5. только после этого переносить следующие подсистемы или делать локальный rewrite.

### 5.2 Что считается допустимым rewrite

Допустимый rewrite на текущем этапе — это только такой rewrite, который:

- исправляет дефект imported foundation;
- убирает project-specific связность;
- переводит слой в целевой модуль 2.0;
- не ломает ментальную модель без сильной причины.

### 5.3 Что считается ошибкой

Ошибкой считается ситуация, когда вместо controlled rewrite:

- переписывается весь foundation из вкусовых соображений;
- меняются названия и семантика без необходимости;
- не фиксируется, что было унаследовано из D6, а что переписано заново;
- platform-level cleanup смешивается с переносом новой большой подсистемы.

---

## 6. Зафиксированные архитектурные решения

### 6.1 Базовые сущности

Остаются зафиксированными:

- составной runtime-блок: `Composite`;
- декларация состава: `Installer`;
- UI-специализированный контроллер: `BaseContextController<T>`;
- lifecycle строится вокруг `LifecycleService`;
- сеть позже живёт отдельным узким extension-модулем под FishNet.

### 6.2 Общий стиль архитектуры

PlatformCore 2.0 по-прежнему делится на:

1. **Foundation / Infrastructure**
   - core contracts;
   - lifecycle;
   - bootstrap;
   - config/resource/factory base;
   - UI foundation;
   - audio foundation;
   - settings foundation;
   - scene management foundation;
   - camera foundation.

2. **Reusable Gameplay**
   - только после стабилизации foundation.

3. **Network Extension**
   - только после стабилизации local platform layer.

### 6.3 Новый приоритет

Если есть выбор между:

- «быстро перенести ещё один кусок поверх сырой базы»;
- «сначала стабилизировать уже импортированную основу»;

приоритет у второго варианта.

---

## 7. Целевая структура репозитория

Текущий baseline должен быть постепенно приведён к такой структуре:

```text
PlatformCore/
  Assets/
    PlatformCore/
      Core/
      Infrastructure/
      UI/
      Audio/
      Settings/
      SceneManagement/
      Camera/
      Gameplay/
      Network/
        FishNet/
      Editor/
      Samples/
      Docs/
```

Или, если репозиторий пока не переводится в `Assets/PlatformCore`, допускается transitional layout, но с теми же границами модулей.

### Целевые asmdef

```text
PlatformCore.Core
PlatformCore.Infrastructure
PlatformCore.UI
PlatformCore.Audio
PlatformCore.Settings
PlatformCore.SceneManagement
PlatformCore.Camera
PlatformCore.Gameplay
PlatformCore.Network.FishNet
PlatformCore.Editor
PlatformCore.Samples
```

### Правила перехода к этой структуре

- не делать полный big-bang rewrite;
- разрезать сборку слоями;
- сначала стабилизировать код, потом переносить его между модулями;
- sample/demo content не должен жить в том же слое, что и core runtime foundation.

---

## 8. Модульная карта и статус по текущему baseline

### 8.1 PlatformCore.Core

**Цель модуля**

Минимальные контракты, composition foundation и runtime utility types.

**Что уже частично есть**

- `IBaseController`;
- lifecycle interfaces;
- часть foundation utility types;
- composition-related база частично распределена по legacy-структуре.

**Что нужно сделать**

- вынести минимум зависимостей;
- убрать внешние пакеты, не относящиеся к core;
- финализировать contracts для composition и lifecycle.

### 8.2 PlatformCore.Infrastructure

**Цель модуля**

Bootstrap, application lifetime и controller lifecycle.

**Что уже есть**

- `BaseBootstrap`;
- `BaseGameRoot`;
- `ApplicationLifetimeService`;
- `LifecycleService`.

**Что нужно сделать**

- исправить семантику lifecycle;
- зафиксировать idempotent-safe API;
- выровнять service registration policy;
- сократить лишнюю legacy-сцепку.

### 8.3 PlatformCore.UI

**Цель модуля**

Платформенный UI foundation.

**Что уже есть**

- `BaseContextController`;
- часть editor tooling для text/color styles;
- отдельные UI helpers.

**Что нужно сделать**

- отделить runtime UI foundation от editor tooling;
- довести UI до целостного platform-level модуля;
- не смешивать foundation с gameplay UI.

### 8.4 PlatformCore.Audio

**Цель модуля**

Платформенный аудио-слой.

**Что уже есть**

- `AudioBaseService`;
- `IAudioService`.

**Что нужно сделать**

- определить, что остаётся core API, а что является vendor-specific реализацией;
- отделить platform API от FMOD-specific привязки;
- подготовить слой к интеграции с settings.

### 8.5 PlatformCore.Settings

**Цель модуля**

Единая система настроек и их применения.

**Что уже есть**

- целостного settings module ещё нет.

**Что нужно сделать**

- создать settings foundation уже поверх стабилизированного baseline;
- не лепить settings напрямую в audio/camera/ui.

### 8.6 PlatformCore.SceneManagement

**Цель модуля**

Scene loading, persistent scene flow и transition orchestration.

**Что уже есть**

- `PersistentSceneContext`;
- foundation-намёк в bootstrap layer.

**Что нужно сделать**

- оформить единый scene management layer;
- убрать размазанную загрузочную логику из других мест.

### 8.7 PlatformCore.Camera

**Цель модуля**

Reusable camera foundation.

**Что уже есть**

- `PlayerCameraService`;
- `ICameraService`;
- `ICameraShakeService`;
- `CinemachineCameraRegister`.

**Что нужно сделать**

- убрать project-specific camera states;
- определить reusable camera API;
- подготовить слой к settings integration;
- отделить foundation от конкретных игровых режимов.

### 8.8 PlatformCore.Gameplay

**Цель модуля**

Reusable gameplay blocks только после стабилизации foundation.

**Текущий статус**

- в baseline не должен тащиться до завершения foundation cleanup.

### 8.9 PlatformCore.Network.FishNet

**Цель модуля**

Узкая FishNet-интеграция поверх уже устойчивой платформы.

**Текущий статус**

- отложено до конца локальной стабилизации.

---

## 9. Обязательная программа исправления imported baseline

Этот раздел считается главным practical backlog ближайшего этапа.

### 9.1 LifecycleService stabilization

Обязательно сделать:

- защиту от повторной регистрации одного и того же controller instance;
- идемпотентный `Unregister`;
- предсказуемый `Dispose`;
- поддержку контроллера, который реализует несколько update-интерфейсов;
- финальное решение по `IActivatable` / `IDeactivatable`;
- зафиксированный API для групповой регистрации.

### 9.2 Asmdef split

Обязательно сделать:

- выделить `PlatformCore.Core`;
- выделить `PlatformCore.Infrastructure`;
- оставить `PlatformCore.Editor` отдельным assembly;
- убрать тяжёлые package references из core assembly;
- сделать зависимости однонаправленными.

### 9.3 Asset and sample cleanup

Обязательно сделать:

- вынести placeholder materials и texture-наборы в `Samples` или отдельный demo-layer;
- убрать случайный asset payload из foundation-слоёв;
- оставить в runtime только действительно необходимые assets.

### 9.4 Resource/Factory cleanup

Обязательно сделать ревизию:

- `ObjectFactory`;
- `ResourceService`;
- `ResourcePaths`.

Нужно определить:

- что является reusable foundation API;
- что завязано на старую структуру конкретного проекта;
- что должно уйти в samples или game layer.

### 9.5 Audio and Camera normalization

Обязательно сделать:

- убрать project-specific состояния камер;
- отделить foundation API от конкретных игровых сценариев;
- зафиксировать vendor-specific зависимости как implementation detail там, где это возможно.

### 9.6 Repo documentation baseline

Обязательно сделать:

- положить актуальные docs в сам репозиторий;
- зафиксировать новую стратегию не как greenfield, а как imported-baseline cleanup;
- вести следующие задачи уже от этого документа.

---

## 10. Новая дорожная карта

### Этап 0. Repository Baseline Audit

Сначала нужно провести ревизию уже существующего `PlatformCore`.

Для каждого уже присутствующего блока зафиксировать:

- где он лежит сейчас;
- откуда он пришёл;
- platform-level он или нет;
- оставить как foundation / адаптировать / переписать / вынести / удалить.

**Результат этапа**

Отдельный markdown-документ:
`docs/PlatformCore_2_0_Repo_Baseline_Audit.md`

### Этап 1. Foundation Stabilization

Исправить критические дефекты уже импортированной базы.

Входит:

- lifecycle fixes;
- cleanup повторных регистраций и dispose-semantics;
- минимальная стабилизация bootstrap/runtime foundation;
- фиксация composition rules.

### Этап 2. Assembly and Dependency Split

Разрезать текущий монолитный runtime assembly.

Входит:

- `PlatformCore.Core`;
- `PlatformCore.Infrastructure`;
- `PlatformCore.Editor`;
- подготовительные заготовки для `UI`, `Audio`, `Settings`, `SceneManagement`, `Camera`.

### Этап 3. Sample and Legacy Payload Cleanup

Очистить foundation от лишнего контента.

Входит:

- перенос placeholder/demo assets из foundation;
- ревизия editor-only кода;
- ревизия sample-пакета.

### Этап 4. UI Foundation Normalization

Не greenfield UI, а нормализация уже импортированного UI baseline.

Входит:

- `BaseContextController`;
- runtime UI base;
- layers/cursor/helpers;
- отделение editor tooling от runtime UI.

### Этап 5. Settings Foundation

Создать целостный settings module поверх уже стабилизированных foundation services.

Входит:

- `SettingsService`;
- `SettingsData`;
- persistence;
- notifications;
- appliers.

### Этап 6. SceneManagement Foundation

Вынести и оформить scene flow как отдельный platform module.

Входит:

- scene loader;
- loading orchestration;
- persistent/gameplay scene split;
- единая точка переходов.

### Этап 7. Audio Foundation Rewrite/Normalization

Не переписать аудио с нуля, а довести импортированный `AudioBaseService` до platform-level состояния.

Входит:

- отделение API от vendor-specific runtime;
- settings integration;
- cleanup service contract.

### Этап 8. Camera Foundation Rewrite/Normalization

Не переписать камеру полностью, а довести импортированный camera baseline до reusable platform module.

Входит:

- cleanup camera states;
- settings integration;
- reusable camera mode policy;
- удаление project-specific режимов.

### Этап 9. Localization and Notifications

После стабилизации foundation можно переносить следующие reusable слои:

- localization foundation;
- global notifications foundation.

### Этап 10. Reusable Gameplay Layer

Только после стабилизации platform modules.

Примеры:

- `PlayerComposite`;
- `PauseMenuComposite`;
- `SettingsComposite`;
- `LevelComposite`;
- `ShopComposite`.

### Этап 11. FishNet Extension

Только после устойчивого local foundation.

---

## 11. Composite и Installer

### 11.1 Composite

`Composite` остаётся runtime-контейнером, который владеет:

- контроллерами;
- дочерними композитами;
- lifecycle-связью;
- собственным lifetime.

На текущем этапе важно не изобрести новый complicated framework, а довести `Composite` до понятной и устойчивой семантики:

- build один раз;
- activate/deactivate предсказуемы;
- dispose безопасен и повторно-устойчив;
- интеграция с `LifecycleService` очевидна по коду.

### 11.2 Installer

`Installer` остаётся декларацией состава.

`Installer`:

- собирает контроллеры;
- создает дочерние блоки;
- описывает состав runtime.

`Composite`:

- управляет жизнью runtime.

На текущем этапе задача не в redesign, а в том, чтобы сохранить эту модель и сделать её рабочей поверх уже импортированной базы.

---

## 12. Правила для Codex и следующих PR

Каждая следующая задача должна исходить из того, что база уже импортирована.

Codex не должен:

- делать новый foundation с нуля;
- размазывать большой rewrite по нескольким подсистемам сразу;
- тащить gameplay-specific код в platform modules;
- переименовывать знакомые сущности без причины;
- смешивать asmdef split, lifecycle rewrite и перенос новой подсистемы в одном PR.

Codex должен:

- сначала проверить текущий код репозитория;
- затем сравнить его с legacy source;
- явно отметить, что исправляется, что сохраняется, что выносится;
- делать маленькие PR с узкой целью;
- оставлять после каждого этапа более чистую, а не просто более новую архитектуру.

---

## 13. Формат задач

Правильный формат задач теперь такой:

- какой уже существующий слой берём;
- какие проблемы в нём исправляем;
- какие файлы считаются источником истины;
- что можно переписать;
- что нельзя менять в этой задаче;
- acceptance criteria.

### Пример правильной задачи

**Пачка: стабилизировать `LifecycleService` в текущем репозитории**

- использовать текущий `PlatformCore/Infrastructure/Lifecycle/LifecycleService.cs` как baseline;
- исправить duplicate registration;
- исправить multi-interface registration;
- унифицировать activate/deactivate semantics;
- не трогать UI, audio, camera, settings;
- не делать asmdef split в этом же PR.

---

## 14. Acceptance Criteria по ближайшим этапам

### 14.1 Baseline audit готов, если

- зафиксировано, что уже находится в репозитории;
- каждому слою назначен статус: keep / adapt / rewrite / move / remove;
- дальнейшие задачи опираются на текущий repo state, а не на устаревший greenfield-план.

### 14.2 Foundation stabilization готов, если

- lifecycle больше не ломается на повторной регистрации и unregister;
- dispose безопасен;
- imported foundation компилируется и предсказуемо работает;
- нет критических legacy-рассинхронов в базовом lifecycle.

### 14.3 Assembly split готов, если

- Core и Infrastructure изолированы;
- Editor не сидит в runtime монолите;
- внешние зависимости больше не тянутся в core без необходимости.

### 14.4 UI normalization готов, если

- есть platform-level UI foundation;
- editor tooling не смешан с runtime UI;
- gameplay UI ещё не залез в foundation.

### 14.5 Settings и SceneManagement готовы, если

- появился самостоятельный settings layer;
- появился самостоятельный scene management layer;
- audio/camera начинают зависеть от них осознанно, а не хаотично.

### 14.6 Audio/Camera normalization готова, если

- удалены project-specific режимы и хвосты;
- API стало reusable;
- зависимость от конкретных vendor/runtime-решений ограничена и понятна.

---

## 15. Какие документы нужны рядом

После обновления этого плана рядом должны появиться:

1. `PlatformCore_2_0_Repo_Baseline_Audit.md`
   - карта текущего состояния репозитория.

2. `PlatformCore_2_0_Module_Split_Spec.md`
   - точный план разрезания asmdef и зависимостей.

3. `PlatformCore_2_0_Task_Queue.md`
   - короткие пачки задач на стабилизацию и нормализацию.

4. `PlatformCore_2_0_Mini_Spec_Milestone_A.md`
   - обновлённый mini spec уже не про import from scratch, а про baseline audit + stabilization.

---

## 16. Следующий практический шаг

Следующий рабочий шаг после этого документа:

1. положить обновлённый plan в сам репозиторий;
2. сделать `Repo Baseline Audit` как отдельный markdown;
3. первой инженерной пачкой взять `LifecycleService stabilization`;
4. второй пачкой взять `asmdef split: Core / Infrastructure / Editor`;
5. третьей пачкой сделать cleanup sample/assets payload;
6. только потом переходить к settings и scene management.

---

## 17. Краткое резюме

PlatformCore 2.0 больше не находится в точке «надо создать foundation».

Foundation уже импортирован.

Теперь реальный план такой:

- признать текущий репозиторий baseline-ом;
- перепроверить и зафиксировать его текущее состояние;
- исправить дефекты imported foundation;
- вычистить project-specific и sample/demo хвосты;
- разрезать монолит по целевым модулям;
- затем довести UI / Settings / SceneManagement / Audio / Camera до platform-level качества;
- потом уже переходить к reusable gameplay и FishNet.

Это не отказ от PlatformCore 2.0.  
Это переход от идеи «создать» к задаче «довести уже созданный baseline до нормальной платформы».
