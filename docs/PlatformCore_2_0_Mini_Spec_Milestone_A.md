# PlatformCore 2.0 — Mini Spec for Milestone A

Версия документа: 3.1
Статус: milestone A зафиксирован как закрытый, документ оставлен как reference
Назначение: использовать как ТЗ на ближайший реальный milestone в текущем репозитории `PlatformCore`

---

## 1. Цель Milestone A (выполнено)

Milestone A теперь означает не foundation import с нуля.

Foundation baseline уже импортирован в репозиторий `PlatformCore`.

Поэтому цель Milestone A:

- провести аудит уже импортированной базы;
- исправить критические дефекты foundation;
- очистить базу от лишних legacy/sample/game-specific хвостов;
- подготовить foundation к модульному разделению;
- не допустить преждевременного переписывания всей платформы с нуля.

Результат достигнут: **стабильный imported baseline** сформирован, модульное разделение и ключевые foundation-нормализации выполнены.

---

## 2. Что входило в Milestone A (выполнено)

### 2.1 Repo baseline audit

Нужно:

- зафиксировать, какие слои уже присутствуют в репозитории;
- отметить, что является reusable platform code;
- отметить, что является project-specific хвостом;
- отметить, что нужно оставить, адаптировать, переписать, вынести или удалить.

Минимальный результат:

- markdown-файл `docs/PlatformCore_2_0_Repo_Baseline_Audit.md`.

### 2.2 Lifecycle stabilization

Нужно исправить текущий lifecycle baseline.

Минимально обязательно:

- duplicate registration guard;
- идемпотентный unregister;
- корректная регистрация контроллера, который реализует несколько update-интерфейсов;
- унификация `IActivatable` / `IDeactivatable` semantics;
- безопасный `Dispose`.

### 2.3 Foundation cleanup

Нужно:

- отделить runtime foundation от editor-only кода;
- определить, какие assets относятся к samples/demo;
- не держать лишний asset payload внутри foundation слоя;
- пересмотреть project-specific пути, enum и legacy-хардкоды.

### 2.4 Assembly split preparation

Нужно подготовить разрезание текущего монолита.

Минимально:

- выделить целевые границы `Core`, `Infrastructure`, `Editor`;
- зафиксировать, какие внешние зависимости лишние для core;
- подготовить asmdef split plan.

---

## 3. Что не входит в Milestone A

Не делать в этой пачке:

- gameplay systems;
- gameplay composites;
- gameplay UI;
- shop / level / mission systems;
- FishNet и любую сетевую логику;
- полноценный settings module;
- полноценный scene management module;
- глобальный rewrite audio/camera/UI;
- большой rename/refactor всего imported foundation;
- localization subsystem как часть PlatformCore foundation (выведен за границы платформы).

Допустимы только локальные исправления, которые реально нужны для стабилизации already imported baseline.

---

## 4. Главные правила реализации

### 4.1 Baseline-first

Каждая задача должна исходить из текущего состояния репозитория, а не из гипотетической чистой архитектуры.

### 4.2 Fix before expand

Сначала исправляем foundation, который уже есть.
Потом переносим новые слои.

### 4.3 Не смешивать стабилизацию и большой rewrite

Если задача про lifecycle stabilization, в ней не должно быть параллельного redesign UI/audio/camera.

### 4.4 Не тащить gameplay в foundation cleanup

Milestone A посвящён platform-level стабилизации, а не построению игровых фич.

---

## 5. Приоритеты Milestone A

Приоритет 1:

- `LifecycleService` stabilization.

Приоритет 2:

- baseline audit current repo state.

Приоритет 3:

- asmdef split plan для `Core / Infrastructure / Editor`.

Приоритет 4:

- cleanup sample/assets/editor payload.

---

## 6. Acceptance Criteria (статус)

Milestone A считается завершённым, если:

- текущее состояние репозитория зафиксировано в отдельном audit-документе;
- foundation больше не содержит критических lifecycle-дефектов;
- есть понятный и проверенный план asmdef split;
- sample/demo/editor payload отделён концептуально и частично физически от foundation;
- следующие этапы больше не опираются на устаревший greenfield-сценарий.

Текущий статус: **критерии выполнены**.

---

## 7. Что делать после закрытия Milestone A

После закрытия Milestone A текущий decision point:

1. перейти к `Reusable Gameplay Layer`;
2. или выполнить один маленький `Composite / Installer / composition sanity cleanup`, если по репозиторию ещё видны локальные хвосты.

---

## 8. Краткое резюме

Milestone A в истории проекта означает:

- не «создать foundation»;
- а «взять уже импортированный foundation baseline и привести его в устойчивое состояние».

Дополнительные фиксации после закрытия:

- localization остаётся вне PlatformCore;
- global notifications остаются platform-level;
- minimal async awaiter foundation уже присутствует в PlatformCore.
