# PlatformCore 2.0 вЂ” Module Split Spec

Р’РµСЂСЃРёСЏ РґРѕРєСѓРјРµРЅС‚Р°: 1.2
РЎС‚Р°С‚СѓСЃ: СЃРїРµС†РёС„РёРєР°С†РёСЏ + sync СЃ С„Р°РєС‚РёС‡РµСЃРєРёРј СЃРѕСЃС‚РѕСЏРЅРёРµРј split (Core / Infrastructure / Editor СЃРґРµР»Р°РЅС‹, РѕСЃС‚Р°Р»СЊРЅС‹Рµ СЃР»РѕРё РїРѕРєР° Р»РѕРіРёС‡РµСЃРєРёРµ)
РќР°Р·РЅР°С‡РµРЅРёРµ: РёСЃРїРѕР»СЊР·РѕРІР°С‚СЊ РєР°Рє guide РґР»СЏ asmdef split Рё РЅР°РІРµРґРµРЅРёСЏ РїРѕСЂСЏРґРєР° РІ Р·Р°РІРёСЃРёРјРѕСЃС‚СЏС…

---

## 1. Р¦РµР»СЊ Рё С‚РµРєСѓС‰РёР№ СЃС‚Р°С‚СѓСЃ

РЎРµР№С‡Р°СЃ runtime-РєРѕРґ РЅР°С…РѕРґРёС‚СЃСЏ РІ РѕРґРЅРѕРј `PlatformCore.asmdef`.

Р­С‚Рѕ РґРѕРїСѓСЃС‚РёРјРѕ РєР°Рє РїСЂРѕРјРµР¶СѓС‚РѕС‡РЅРѕРµ СЃРѕСЃС‚РѕСЏРЅРёРµ РїРѕСЃР»Рµ legacy import, РЅРѕ РґР°Р»СЊС€Рµ СЌС‚Рѕ РјРµС€Р°РµС‚:

- РёР·РѕР»СЏС†РёРё Core;
- РІР°Р»РёРґР°С†РёРё Р·Р°РІРёСЃРёРјРѕСЃС‚РµР№;
- С‡РёСЃС‚РѕРјСѓ foundation API;
- РґР°Р»СЊРЅРµР№С€РµРјСѓ РїРµСЂРµРЅРѕСЃСѓ settings/scene/UI;
- Р»РѕРєР°Р»СЊРЅРѕРјСѓ РїРµСЂРµРїРёСЃС‹РІР°РЅРёСЋ РѕС‚РґРµР»СЊРЅС‹С… РїРѕРґСЃРёСЃС‚РµРј.

Р¦РµР»СЊ СЌС‚РѕРіРѕ РґРѕРєСѓРјРµРЅС‚Р°:

**СЂР°Р·СЂРµР·Р°С‚СЊ С‚РµРєСѓС‰РёР№ baseline РЅР° С†РµР»РµРІС‹Рµ РјРѕРґСѓР»Рё 2.0 Р±РµР· big-bang rewrite.**

РўРµРєСѓС‰РёР№ СЃС‚Р°С‚СѓСЃ: РїРµСЂРІРёС‡РЅС‹Р№ split (`Core / Infrastructure / Editor`) РІС‹РїРѕР»РЅРµРЅ. РћС‚РґРµР»СЊРЅС‹Рµ asmdef РґР»СЏ `UI/Audio/Settings/SceneManagement/Camera` РїРѕРєР° РЅРµ РІС‹РґРµР»РµРЅС‹ С„РёР·РёС‡РµСЃРєРё Рё РѕСЃС‚Р°СЋС‚СЃСЏ Р»РѕРіРёС‡РµСЃРєРёРјРё СЃР»РѕСЏРјРё РІРЅСѓС‚СЂРё С‚РµРєСѓС‰РµРіРѕ runtime baseline. Р”РѕРєСѓРјРµРЅС‚ РёСЃРїРѕР»СЊР·СѓРµС‚СЃСЏ РєР°Рє reference РґР»СЏ boundary-polish Р±РµР· redesign.

---

## 2. Р“Р»Р°РІРЅС‹Рµ РїСЂР°РІРёР»Р° split

### 2.1 Split РЅРµ СЂР°РІРµРЅ rewrite

Р Р°Р·РґРµР»РµРЅРёРµ СЃР±РѕСЂРѕРє РЅРµ РґРѕР»Р¶РЅРѕ СЃРѕРїСЂРѕРІРѕР¶РґР°С‚СЊСЃСЏ РѕРґРЅРѕРІСЂРµРјРµРЅРЅРѕР№ РїРѕР»РЅРѕР№ РїРµСЂРµСЂР°Р±РѕС‚РєРѕР№ Р»РѕРіРёРєРё.

РЎРЅР°С‡Р°Р»Р°:

- С„РёР·РёС‡РµСЃРєРё РѕС‚РґРµР»СЏРµРј СЃР»РѕРё;
- РІС‹СЂР°РІРЅРёРІР°РµРј СЃСЃС‹Р»РєРё;
- СѓР±РёСЂР°РµРј Р»РёС€РЅРёРµ Р·Р°РІРёСЃРёРјРѕСЃС‚Рё.

РџРѕС‚РѕРј:

- РїРµСЂРµРїРёСЃС‹РІР°РµРј РїСЂРѕР±Р»РµРјРЅС‹Рµ СЂРµР°Р»РёР·Р°С†РёРё Р»РѕРєР°Р»СЊРЅРѕ.

### 2.2 Core РґРѕР»Р¶РµРЅ Р±С‹С‚СЊ РјР°РєСЃРёРјР°Р»СЊРЅРѕ Р»С‘РіРєРёРј

Р’ `PlatformCore.Core` РЅРµ РґРѕР»Р¶РЅС‹ Р¶РёС‚СЊ РїСЂСЏРјС‹Рµ Р·Р°РІРёСЃРёРјРѕСЃС‚Рё РЅР°:

- FMOD;
- Cinemachine;
- TMPro;
- PrimeTween;
- Splines;
- editor API;
- sample assets.

### 2.3 Infrastructure Р·Р°РІРёСЃРёС‚ РЅР° Core, РЅРѕ РЅРµ РЅР°РѕР±РѕСЂРѕС‚

`Infrastructure` РјРѕР¶РµС‚ Р·Р°РІРёСЃРµС‚СЊ РѕС‚ `Core`.
`Core` РЅРµ РґРѕР»Р¶РµРЅ Р·Р°РІРёСЃРµС‚СЊ РѕС‚ `Infrastructure`.

### 2.4 UI / Audio / Camera / Settings / SceneManagement РґРѕР»Р¶РЅС‹ Р±С‹С‚СЊ РѕС‚РґРµР»СЊРЅС‹РјРё СЃР»РѕСЏРјРё

РћРЅРё РЅРµ РґРѕР»Р¶РЅС‹ РѕСЃС‚Р°РІР°С‚СЊСЃСЏ В«РїР°РїРєР°РјРё РІРЅСѓС‚СЂРё РѕРґРЅРѕРіРѕ runtime asmdefВ».

### 2.5 Editor Рё Samples РІСЃРµРіРґР° РѕС‚РґРµР»СЊРЅРѕ

- `PlatformCore.Editor` РЅРµ РґРѕР»Р¶РµРЅ СЃРјРµС€РёРІР°С‚СЊСЃСЏ СЃ runtime;
- `PlatformCore.Samples` РЅРµ РґРѕР»Р¶РµРЅ Р·Р°РіСЂСЏР·РЅСЏС‚СЊ foundation.

### 2.6 Localization РІРЅРµ scope PlatformCore

- Localization intentionally stays outside PlatformCore foundation scope.
- Platform runtime РјРѕРґСѓР»Рё РЅРµ РґРѕР»Р¶РЅС‹ С‚СЂРµР±РѕРІР°С‚СЊ `ILocalizationService`/localization registration РґР»СЏ Р±Р°Р·РѕРІРѕР№ СЂР°Р±РѕС‚С‹.
- Global in-app notifications РѕСЃС‚Р°СЋС‚СЃСЏ platform-level СЃР»РѕРµРј Рё СЂР°Р±РѕС‚Р°СЋС‚ РЅР° raw message data.

---

## 3. Р¦РµР»РµРІС‹Рµ РјРѕРґСѓР»Рё

## 3.1 PlatformCore.Core

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

РњРёРЅРёРјР°Р»СЊРЅС‹Рµ runtime contracts Рё composition base.

### РљР°РЅРґРёРґР°С‚С‹ РЅР° РїРµСЂРµРЅРѕСЃ

- `IBaseController`
- lifecycle-related interfaces
- `Composite`
- `Installer`
- composition utility types
- РІРѕР·РјРѕР¶РЅРѕ РјРёРЅРёРјР°Р»СЊРЅС‹Рµ service contracts Р±РµР· РІРЅРµС€РЅРёС… vendor-dependencies

### Р§С‚Рѕ РЅРµ РґРѕР»Р¶РЅРѕ Р»РµР¶Р°С‚СЊ Р·РґРµСЃСЊ

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

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Application/bootstrap/runtime orchestration.

### РљР°РЅРґРёРґР°С‚С‹ РЅР° РїРµСЂРµРЅРѕСЃ

- `ApplicationLifetimeService`
- `BaseBootstrap`
- `BaseGameRoot`
- `PersistentSceneContext`
- `LifecycleService`
- РІРѕР·РјРѕР¶РЅРѕ service registration / locator infrastructure

### Р—Р°РІРёСЃРёРјРѕСЃС‚Рё

- Р·Р°РІРёСЃРёС‚ РѕС‚ `PlatformCore.Core`

---

## 3.3 PlatformCore.UI

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Platform-level UI foundation.

### РљР°РЅРґРёРґР°С‚С‹ РЅР° РїРµСЂРµРЅРѕСЃ

- `BaseContextController<T>`
- `IUIService`
- `UIBaseElement`
- UI service base
- layers / cursor / helpers
- РїРѕР·Р¶Рµ global notifications

### РћСЃРѕР±РѕРµ РїСЂР°РІРёР»Рѕ

Runtime UI Рё editor style tooling РґРѕР»Р¶РЅС‹ Р±С‹С‚СЊ СЂР°Р·РІРµРґРµРЅС‹.

---

## 3.4 PlatformCore.Audio

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Platform audio API Рё РµРіРѕ СЂРµР°Р»РёР·Р°С†РёСЏ.

### РљР°РЅРґРёРґР°С‚С‹ РЅР° РїРµСЂРµРЅРѕСЃ

- `IAudioService`
- `AudioBaseService`
- audio settings bridge later

### РћСЃРѕР±РѕРµ РїСЂР°РІРёР»Рѕ

Р•СЃР»Рё FMOD РѕСЃС‚Р°С‘С‚СЃСЏ С‚РµРєСѓС‰РµР№ СЂРµР°Р»РёР·Р°С†РёРµР№, СЌС‚Рѕ РґРѕР»Р¶РЅРѕ Р±С‹С‚СЊ СЏРІРЅРѕ РІРёРґРЅРѕ РЅР° СѓСЂРѕРІРЅРµ module ownership Рё Р·Р°РІРёСЃРёРјРѕСЃС‚РµР№.

---

## 3.5 PlatformCore.Settings

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Settings model, persistence, notifications, appliers.

### РљР°РЅРґРёРґР°С‚С‹

- РїРѕРєР° РјРѕРґСѓР»СЊ СЃРѕР·РґР°С‘С‚СЃСЏ РїРѕР·Р¶Рµ, РїРѕСЃР»Рµ split foundation

---

## 3.6 PlatformCore.SceneManagement

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Scene loading, loading flow, persistent/gameplay scene orchestration.

### РљР°РЅРґРёРґР°С‚С‹

- `PersistentSceneContext`
- scene loader/services later

### РџСЂРёРјРµС‡Р°РЅРёРµ

РќР° transitional step `PersistentSceneContext` РјРѕР¶РµС‚ РІСЂРµРјРµРЅРЅРѕ РѕСЃС‚Р°РІР°С‚СЊСЃСЏ РІ Infrastructure, РїРѕРєР° РЅРµ РѕС„РѕСЂРјР»РµРЅ РїРѕР»РЅРѕС†РµРЅРЅС‹Р№ scene module.

---

## 3.7 PlatformCore.Camera

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Reusable camera foundation.

### РљР°РЅРґРёРґР°С‚С‹ РЅР° РїРµСЂРµРЅРѕСЃ

- `ICameraService`
- `ICameraShakeService`
- `CameraService` / `PlayerCameraService`
- `CinemachineCameraRegister`

### РћСЃРѕР±РѕРµ РїСЂР°РІРёР»Рѕ

РРіСЂРѕРІС‹Рµ camera states РЅРµ РґРѕР»Р¶РЅС‹ РѕСЃС‚Р°РІР°С‚СЊСЃСЏ foundation contract.

---

## 3.8 PlatformCore.Editor

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Editor tooling only.

### РљР°РЅРґРёРґР°С‚С‹ РЅР° РїРµСЂРµРЅРѕСЃ

- `PlatformCore.Editor.asmdef`
- style editors
- drawers
- UI editor helpers

---

## 3.9 PlatformCore.Samples

### РћС‚РІРµС‚СЃС‚РІРµРЅРЅРѕСЃС‚СЊ

Sample/demo assets Рё РґРµРјРѕРЅСЃС‚СЂР°С†РёРѕРЅРЅС‹Рµ runtime parts.

### РљР°РЅРґРёРґР°С‚С‹ РЅР° РїРµСЂРµРЅРѕСЃ

- placeholder materials
- texture packs
- demo resources
- РїСЂРѕРµРєС‚РЅС‹Рµ РїСЂРёРјРµСЂС‹, РµСЃР»Рё РѕРЅРё РѕСЃС‚Р°РЅСѓС‚СЃСЏ

---

## 4. Р–РµР»Р°РµРјР°СЏ РєР°СЂС‚Р° Р·Р°РІРёСЃРёРјРѕСЃС‚РµР№

```text
PlatformCore.Core
  в†‘
PlatformCore.Infrastructure
  в†‘
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
  depends on runtime modules, never РЅР°РѕР±РѕСЂРѕС‚
```

### РџСЂР°РІРёР»Рѕ

РћСЃРЅРѕРІР°РЅРёРµ РіСЂР°С„Р° вЂ” `Core`, Р° РЅРµ UI/Audio/Camera.

---

## 5. РџСЂР°РєС‚РёС‡РµСЃРєРёР№ РїРѕСЂСЏРґРѕРє split

### РЁР°Рі 1. Р’С‹РґРµР»РёС‚СЊ Core вЂ” РІС‹РїРѕР»РЅРµРЅ

РЎРЅР°С‡Р°Р»Р° РІС‹РЅРµСЃС‚Рё РјРёРЅРёРјР°Р»СЊРЅС‹Рµ РєРѕРЅС‚СЂР°РєС‚С‹ Рё Р±Р°Р·РѕРІС‹Рµ interfaces.

### РЁР°Рі 2. Р’С‹РґРµР»РёС‚СЊ Infrastructure вЂ” РІС‹РїРѕР»РЅРµРЅ

РџРµСЂРµРЅРµСЃС‚Рё bootstrap Рё lifecycle foundation.

### РЁР°Рі 3. РћСЃС‚Р°РІРёС‚СЊ Editor РѕС‚РґРµР»СЊРЅС‹Рј вЂ” РІС‹РїРѕР»РЅРµРЅ

РџСЂРѕРІРµСЂРёС‚СЊ, С‡С‚Рѕ editor tooling РЅРµ СЃРёРґРёС‚ РІ runtime tree Р±РµР· РЅРµРѕР±С…РѕРґРёРјРѕСЃС‚Рё.

### РЁР°Рі 4. РџРѕРґРіРѕС‚РѕРІРёС‚СЊ UI / Audio / Camera boundaries вЂ” РІС‹РїРѕР»РЅРµРЅ РЅР° СѓСЂРѕРІРЅРµ foundation normalization

РџРѕРєР° РјРѕР¶РЅРѕ РЅРµ РґРµР»Р°С‚СЊ РёРґРµР°Р»СЊРЅС‹Р№ cleanup СЂРµР°Р»РёР·Р°С†РёРё, РЅРѕ С„РёР·РёС‡РµСЃРєРё РѕС‚РґРµР»РёС‚СЊ ownership СЃР»РѕС‘РІ.

### РЁР°Рі 5. РћС‚РґРµР»СЊРЅРѕ РѕС„РѕСЂРјРёС‚СЊ Settings Рё SceneManagement вЂ” РІС‹РїРѕР»РЅРµРЅ

РќРµ СЂР°РЅСЊС€Рµ, С‡РµРј Foundation split СѓР¶Рµ СѓСЃС‚РѕР№С‡РёРІ.

---

## 6. Р§С‚Рѕ РЅРµР»СЊР·СЏ РґРµР»Р°С‚СЊ РІРѕ РІСЂРµРјСЏ split

РќРµР»СЊР·СЏ:

- РѕРґРЅРѕРІСЂРµРјРµРЅРЅРѕ РґРµР»Р°С‚СЊ РїРѕР»РЅС‹Р№ rewrite lifecycle;
- РѕРґРЅРѕРІСЂРµРјРµРЅРЅРѕ РїСЂРёРґСѓРјС‹РІР°С‚СЊ РЅРѕРІС‹Р№ UI framework;
- С‚Р°С‰РёС‚СЊ gameplay systems;
- РѕСЃС‚Р°РІР»СЏС‚СЊ game-specific enums Рё resource paths РєР°Рє foundation contracts;
- РїСЂРµРІСЂР°С‰Р°С‚СЊ asmdef split РІ РјРЅРѕРіРѕРЅРµРґРµР»СЊРЅС‹Р№ rename-driven refactor.

---

## 7. Acceptance Criteria

Module split СЃС‡РёС‚Р°РµС‚СЃСЏ СѓСЃРїРµС€РЅС‹Рј, РµСЃР»Рё:

- `Core` Р±РѕР»СЊС€Рµ РЅРµ С‚СЏРЅРµС‚ vendor/runtime-specific РїР°РєРµС‚С‹;
- `Infrastructure` РѕС‚РґРµР»С‘РЅ РѕС‚ UI/audio/camera;
- `Editor` С„РёР·РёС‡РµСЃРєРё РѕС‚РґРµР»С‘РЅ РѕС‚ runtime;
- `Samples` РЅРµ Р¶РёРІСѓС‚ РІ foundation runtime tree РєР°Рє РѕР±СЏР·Р°С‚РµР»СЊРЅР°СЏ С‡Р°СЃС‚СЊ РїР»Р°С‚С„РѕСЂРјС‹;
- СЃР»РµРґСѓСЋС‰РёР№ PR РјРѕР¶РЅРѕ РґРµР»Р°С‚СЊ СѓР¶Рµ РЅР° СѓСЂРѕРІРЅРµ РѕС‚РґРµР»СЊРЅРѕРіРѕ РјРѕРґСѓР»СЏ, Р° РЅРµ РЅР° СѓСЂРѕРІРЅРµ РІСЃРµРіРѕ РјРѕРЅРѕР»РёС‚Р°.

РўРµРєСѓС‰РёР№ СЃС‚Р°С‚СѓСЃ: **РєСЂРёС‚РµСЂРёРё РІС‹РїРѕР»РЅРµРЅС‹ РґР»СЏ foundation-phase**.

