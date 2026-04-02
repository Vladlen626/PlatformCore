# PlatformCore 2.0 вЂ” Repo Baseline Audit

Р’РµСЂСЃРёСЏ РґРѕРєСѓРјРµРЅС‚Р°: 1.2
РЎС‚Р°С‚СѓСЃ: Р°СѓРґРёС‚ СЃРёРЅС…СЂРѕРЅРёР·РёСЂРѕРІР°РЅ СЃ С„РёРЅР°Р»СЊРЅС‹Рј docs pass (foundation Р·Р°РєСЂС‹С‚, gameplay/network narrow tracks СЃС‚Р°СЂС‚РѕРІР°РЅС‹)
РќР°Р·РЅР°С‡РµРЅРёРµ: РёСЃРїРѕР»СЊР·РѕРІР°С‚СЊ РєР°Рє РєР°СЂС‚Сѓ С‚РµРєСѓС‰РµРіРѕ baseline РїРµСЂРµРґ stabilization, modular split Рё selective rewrite

---

## 1. Р—Р°С‡РµРј РЅСѓР¶РµРЅ СЌС‚РѕС‚ РґРѕРєСѓРјРµРЅС‚

Р РµРїРѕР·РёС‚РѕСЂРёР№ `PlatformCore` СѓР¶Рµ РЅРµ РЅР°С…РѕРґРёС‚СЃСЏ РІ СЃРѕСЃС‚РѕСЏРЅРёРё РїСѓСЃС‚РѕРіРѕ РєР°СЂРєР°СЃР°.

Р’ РЅРµРіРѕ СѓР¶Рµ РёРјРїРѕСЂС‚РёСЂРѕРІР°РЅ РєСЂСѓРїРЅС‹Р№ baseline РёР· D6-Express. РџРѕСЌС‚РѕРјСѓ РґР°Р»СЊС€Рµ РЅРµР»СЊР·СЏ РїР»Р°РЅРёСЂРѕРІР°С‚СЊ СЂР°Р±РѕС‚Сѓ РєР°Рє Р±СѓРґС‚Рѕ foundation РµС‰С‘ С‚РѕР»СЊРєРѕ РїСЂРµРґСЃС‚РѕРёС‚ РїРµСЂРµРЅРµСЃС‚Рё.

Р­С‚РѕС‚ РґРѕРєСѓРјРµРЅС‚ С„РёРєСЃРёСЂСѓРµС‚:

- С‡С‚Рѕ СѓР¶Рµ СЂРµР°Р»СЊРЅРѕ РЅР°С…РѕРґРёС‚СЃСЏ РІ СЂРµРїРѕР·РёС‚РѕСЂРёРё;
- РєР°РєРёРµ С‡Р°СЃС‚Рё РІС‹РіР»СЏРґСЏС‚ РєР°Рє reusable platform foundation;
- РєР°РєРёРµ С‡Р°СЃС‚Рё РЅРµСЃСѓС‚ legacy/project-specific С…РІРѕСЃС‚С‹;
- С‡С‚Рѕ РЅСѓР¶РЅРѕ РѕСЃС‚Р°РІРёС‚СЊ;
- С‡С‚Рѕ РЅСѓР¶РЅРѕ Р°РґР°РїС‚РёСЂРѕРІР°С‚СЊ;
- С‡С‚Рѕ РЅСѓР¶РЅРѕ РїРµСЂРµРїРёСЃР°С‚СЊ;
- С‡С‚Рѕ РЅСѓР¶РЅРѕ РІС‹РЅРµСЃС‚Рё РёР· foundation СЃР»РѕС‘РІ.

---

## 2. РљСЂР°С‚РєРёР№ РІС‹РІРѕРґ РїРѕ СЃРѕСЃС‚РѕСЏРЅРёСЋ СЂРµРїРѕР·РёС‚РѕСЂРёСЏ

### 2.1 РђРєС‚СѓР°Р»СЊРЅР°СЏ С„РёРєСЃР°С†РёСЏ РЅР° С‚РµРєСѓС‰РёР№ РјРѕРјРµРЅС‚

Р”РѕРїРѕР»РЅРёС‚РµР»СЊРЅРѕ РїРѕРґС‚РІРµСЂР¶РґРµРЅРѕ РїРѕ РєРѕРґСѓ:

- foundation-phase РїРѕ СЃРѕРєСЂР°С‰С‘РЅРЅРѕРјСѓ РїР»Р°РЅСѓ РїСЂР°РєС‚РёС‡РµСЃРєРё Р·Р°РІРµСЂС€С‘РЅ;
- reusable gameplay layer СЂРµР°Р»СЊРЅРѕ РЅР°С‡Р°С‚ (`SettingsComposite`, `PauseMenuComposite`, camera gameplay layer first/third person);
- FishNet foundation/session runtime layer СѓР¶Рµ РµСЃС‚СЊ Рё РїРѕРґРєР»СЋС‡Р°РµС‚СЃСЏ С‡РµСЂРµР· service locator extensions;
- localization РЅРµ РІС…РѕРґРёС‚ РІ PlatformCore scope;
- global notifications РІС…РѕРґСЏС‚ РІ PlatformCore scope;
- reusable character controller, `PlayerComposite`, `ShopComposite`, `LevelComposite` РЅРµ РІС…РѕРґСЏС‚ РІ РѕР±СЏР·Р°С‚РµР»СЊРЅС‹Р№ scope С‚РµРєСѓС‰РµРіРѕ baseline.


РўРµРєСѓС‰РµРµ СЃРѕСЃС‚РѕСЏРЅРёРµ `PlatformCore` РјРѕР¶РЅРѕ РѕРїРёСЃР°С‚СЊ С‚Р°Рє:

- foundation baseline СѓР¶Рµ РёРјРїРѕСЂС‚РёСЂРѕРІР°РЅ;
- РёРјРїРѕСЂС‚ СЃРґРµР»Р°РЅ Р±Р»РёР·РєРѕ Рє legacy-РѕСЂРёРіРёРЅР°Р»Сѓ;
- СЌС‚Рѕ Р»СѓС‡С€Рµ, С‡РµРј greenfield rewrite, РЅРѕ Р±Р°Р·Р° РїРѕРєР° СЃС‹СЂР°СЏ;
- Core / Infrastructure / Editor split СѓР¶Рµ РѕС„РѕСЂРјР»РµРЅ;
- lifecycle, UI, audio, camera Рё resource/factory foundation РїСЂРѕС€Р»Рё РЅРѕСЂРјР°Р»РёР·Р°С†РёСЋ;
- settings Рё scene management foundation СѓР¶Рµ РґРѕР±Р°РІР»РµРЅС‹;
- localization РїРѕРґС‚РІРµСЂР¶РґС‘РЅРЅРѕ РѕСЃС‚Р°С‘С‚СЃСЏ Р·Р° РіСЂР°РЅРёС†Р°РјРё PlatformCore;
- global notifications Рё minimal async awaiter СѓР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚ РєР°Рє platform-level foundation.

Р“Р»Р°РІРЅС‹Р№ practical РІС‹РІРѕРґ:

**СЃРµР№С‡Р°СЃ РїСЂР°РІРёР»СЊРЅР°СЏ СЂР°Р±РѕС‚Р° вЂ” РЅРµ РїРµСЂРµРЅРѕСЃРёС‚СЊ РґР°Р»СЊС€Рµ РІСЃС‘ РїРѕРґСЂСЏРґ, Р° СЃС‚Р°Р±РёР»РёР·РёСЂРѕРІР°С‚СЊ Рё СЂР°Р·СЂРµР·Р°С‚СЊ СѓР¶Рµ СЃСѓС‰РµСЃС‚РІСѓСЋС‰РёР№ baseline.**

---

## 3. Р§С‚Рѕ СѓР¶Рµ РµСЃС‚СЊ РІ СЂРµРїРѕР·РёС‚РѕСЂРёРё

### 3.1 Foundation / bootstrap

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `ApplicationLifetimeService`
- `BaseBootstrap`
- `BaseGameRoot`
- `PersistentSceneContext`

### 3.2 Lifecycle

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `IBaseController`
- lifecycle interfaces
- `LifecycleService`

### 3.3 UI-related base

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `BaseContextController<T>`
- UI style editor tooling
- UI-related resource paths

### 3.4 Factory / resources / config

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `ConfigService`
- `BaseConfig`
- `ObjectFactory`
- `ResourceService`
- `ResourcePaths`

### 3.5 Audio

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `IAudioService`
- `AudioBaseService`

### 3.6 Camera

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `ICameraService`
- `ICameraShakeService`
- `PlayerCameraService` / `CameraService`
- `CinemachineCameraRegister`

### 3.7 Editor

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `PlatformCore.Editor.asmdef`
- editors РґР»СЏ text/color styles
- editor helper РґР»СЏ UI background sizing

### 3.8 Misc foundation

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- trigger foundation
- placeholder materials / textures
- generated resource path file

### 3.9 Notifications

РЈР¶Рµ РїСЂРёСЃСѓС‚СЃС‚РІСѓСЋС‚:

- `IGlobalNotificationService`;
- `GlobalNotificationService`;
- banner/toast UI integration.

РџСЂРёРјРµС‡Р°РЅРёРµ: localization subsystem РЅРµ СЃС‡РёС‚Р°РµС‚СЃСЏ С‡Р°СЃС‚СЊСЋ С†РµР»РµРІРѕРіРѕ PlatformCore Рё РЅРµ РІС…РѕРґРёС‚ РІ platform foundation scope.

---

## 4. РћР±С‰Р°СЏ РѕС†РµРЅРєР° СЃР»РѕС‘РІ

### 4.1 Р§С‚Рѕ РІС‹РіР»СЏРґРёС‚ РєР°Рє С…РѕСЂРѕС€РёР№ РєР°РЅРґРёРґР°С‚ РЅР° СЃРѕС…СЂР°РЅРµРЅРёРµ

Р­С‚Рѕ С‡Р°СЃС‚Рё, РєРѕС‚РѕСЂС‹Рµ СѓР¶Рµ РІС‹РіР»СЏРґСЏС‚ РєР°Рє platform foundation Рё РґРѕР»Р¶РЅС‹ Р±С‹С‚СЊ СЃРѕС…СЂР°РЅРµРЅС‹ РєР°Рє baseline СЃ С‚РѕС‡РµС‡РЅС‹РјРё РёСЃРїСЂР°РІР»РµРЅРёСЏРјРё:

- `ApplicationLifetimeService`
- `BaseBootstrap`
- `BaseGameRoot`
- `PersistentSceneContext`
- `LifecycleService`
- `IBaseController` Рё lifecycle contracts
- `BaseContextController<T>` РєР°Рє legacy UI pattern
- `ObjectFactory` / `ResourceService` РєР°Рє candidates РЅР° foundation API
- `AudioBaseService` РєР°Рє legacy baseline for normalization
- `CameraService` / `PlayerCameraService` РєР°Рє legacy baseline for normalization
- `PlatformCore.Editor` РєР°Рє РѕС‚РґРµР»СЊРЅС‹Р№ editor-layer

### 4.2 Р§С‚Рѕ С‚РѕС‡РЅРѕ С‚СЂРµР±СѓРµС‚ Р°РґР°РїС‚Р°С†РёРё

Р­С‚Рѕ С‡Р°СЃС‚Рё, РєРѕС‚РѕСЂС‹Рµ РЅРµ РЅСѓР¶РЅРѕ РІС‹РєРёРґС‹РІР°С‚СЊ, РЅРѕ РЅРµР»СЊР·СЏ РѕСЃС‚Р°РІР»СЏС‚СЊ РІ С‚РµРєСѓС‰РµРј РІРёРґРµ:

- `LifecycleService`
- `BaseContextController<T>`
- `ObjectFactory`
- `ResourceService`
- `AudioBaseService`
- `CameraService` / `PlayerCameraService`
- `ConfigLoader` / `ConfigService`

### 4.3 Р§С‚Рѕ РІС‹РіР»СЏРґРёС‚ РєР°Рє project-specific С…РІРѕСЃС‚ Рё С‚СЂРµР±СѓРµС‚ РІС‹РЅРѕСЃР°

Р­С‚Рѕ С‡Р°СЃС‚Рё, РєРѕС‚РѕСЂС‹Рµ РЅРµ РґРѕР»Р¶РЅС‹ РѕСЃС‚Р°РІР°С‚СЊСЃСЏ РІРЅСѓС‚СЂРё platform foundation РєР°Рє РµСЃС‚СЊ:

- project-specific entries РІ `ResourcePaths`
- game-specific camera states
- sample/demo assets РІРЅСѓС‚СЂРё foundation РґРµСЂРµРІР°
- UI prefab paths РєРѕРЅРєСЂРµС‚РЅРѕР№ РёРіСЂС‹
- analytics resource entries
- РёРіСЂРѕРІС‹Рµ item/shop/player resource paths

---

## 5. РљР»СЋС‡РµРІС‹Рµ РїСЂРѕР±Р»РµРјС‹ baseline

## 5.1 РњРѕРЅРѕР»РёС‚РЅС‹Р№ asmdef

РЎРµР№С‡Р°СЃ runtime СЃРѕР±СЂР°РЅ С‡РµСЂРµР· РѕР±С‰РёР№ `PlatformCore.asmdef`.

РћРЅ СѓР¶Рµ С‚СЏРЅРµС‚ РІРЅРµС€РЅРёРµ Р·Р°РІРёСЃРёРјРѕСЃС‚Рё РІСЂРѕРґРµ:

- UniTask
- TMPro
- FMOD
- Cinemachine
- Splines
- PrimeTween Modules
- Newtonsoft.Json

Р­С‚Рѕ Р·РЅР°С‡РёС‚:

- Core РµС‰С‘ РЅРµ РёР·РѕР»РёСЂРѕРІР°РЅ;
- Infrastructure РµС‰С‘ РЅРµ РёР·РѕР»РёСЂРѕРІР°РЅ;
- UI/Audio/Camera РЅРµ РІС‹РґРµР»РµРЅС‹ РІ СЃР°РјРѕСЃС‚РѕСЏС‚РµР»СЊРЅС‹Рµ РјРѕРґСѓР»Рё;
- foundation РЅРµ РјРѕР¶РµС‚ СЃС‡РёС‚Р°С‚СЊСЃСЏ С‡РёСЃС‚С‹Рј РїРѕ Р·Р°РІРёСЃРёРјРѕСЃС‚СЏРј.

### РЎС‚Р°С‚СѓСЃ
**rewrite/adapt РѕР±СЏР·Р°С‚РµР»СЊРЅРѕ**

---

## 5.2 LifecycleService: РєСЂРёС‚РёС‡РµСЃРєРёР№ РґРѕР»Рі Р·Р°РєСЂС‹С‚

`LifecycleService` Р±РѕР»СЊС€Рµ РЅРµ РЅР°С…РѕРґРёС‚СЃСЏ РІ РєСЂРёС‚РёС‡РµСЃРєРѕР№ Р·РѕРЅРµ:

- duplicate registration guard СЂРµР°Р»РёР·РѕРІР°РЅ;
- `Unregister` СЂР°Р±РѕС‚Р°РµС‚ РёРґРµРјРїРѕС‚РµРЅС‚РЅРѕ;
- multi-interface update registration РїРѕРґРґРµСЂР¶РёРІР°РµС‚СЃСЏ;
- dispose/unregister semantics СЃС‚Р°Р±РёР»РёР·РёСЂРѕРІР°РЅС‹.

### РЎС‚Р°С‚СѓСЃ
**keep as stabilized baseline**

---

## 5.3 BaseContextController<T> СѓР¶Рµ РµСЃС‚СЊ, РЅРѕ С‚СЂРµР±СѓРµС‚ РЅРѕСЂРјР°Р»РёР·Р°С†РёРё

Р­С‚Рѕ С…РѕСЂРѕС€РёР№ Р·РЅР°Рє: legacy UI pattern СѓР¶Рµ РїРµСЂРµРЅРµСЃС‘РЅ, Рё РЅРµ РЅСѓР¶РЅРѕ РІС‹РґСѓРјС‹РІР°С‚СЊ РµРіРѕ СЃ РЅСѓР»СЏ.

РќРѕ С‚РµРєСѓС‰РёР№ baseline РЅР°РґРѕ РїРµСЂРµРїСЂРѕРІРµСЂРёС‚СЊ РїРѕ РЅРµСЃРєРѕР»СЊРєРёРј РІРѕРїСЂРѕСЃР°Рј:

- РїСЂР°РІРёР»СЊРЅР° Р»Рё СЃРµРјР°РЅС‚РёРєР° `PreloadAsync -> Activate -> GetWindow -> Unload`;
- РЅРµ СЏРІР»СЏРµС‚СЃСЏ Р»Рё `Deactivate()` СЃР»РёС€РєРѕРј Р¶С‘СЃС‚РєРёРј С‡РµСЂРµР· РѕР±СЏР·Р°С‚РµР»СЊРЅС‹Р№ `Unload<T>()`;
- РґРѕР»Р¶РµРЅ Р»Рё foundation-level controller РІСЃРµРіРґР° РѕР±РЅСѓР»СЏС‚СЊ РєРѕРЅС‚РµРєСЃС‚ С‚Р°РєРёРј РѕР±СЂР°Р·РѕРј;
- С‡С‚Рѕ СЏРІР»СЏРµС‚СЃСЏ platform-level РѕР±СЏР·Р°РЅРЅРѕСЃС‚СЊСЋ `IUIService`, Р° С‡С‚Рѕ РѕС‚РЅРѕСЃРёС‚СЃСЏ Рє РєРѕРЅРєСЂРµС‚РЅРѕР№ РёРіСЂРµ.

### РЎС‚Р°С‚СѓСЃ
**keep + adapt during UI normalization**

---

## 5.4 ResourcePaths вЂ” РіР»Р°РІРЅС‹Р№ РёСЃС‚РѕС‡РЅРёРє project-specific payload

РўРµРєСѓС‰РёР№ `ResourcePaths` РІС‹РіР»СЏРґРёС‚ РєР°Рє РїСЂСЏРјРѕР№ РёРјРїРѕСЂС‚ РёР· РїСЂРѕРµРєС‚Р°, Р° РЅРµ РєР°Рє platform-level СЂРµСЃСѓСЂСЃРЅР°СЏ РєР°СЂС‚Р°.

РўР°Рј СѓР¶Рµ РЅР°С…РѕРґСЏС‚СЃСЏ:

- shop paths;
- item paths;
- player/npc paths;
- dice-related paths;
- РєРѕРЅРєСЂРµС‚РЅС‹Рµ UI prefabs РёРіСЂС‹;
- analytics settings path;
- РёРіСЂРѕРІС‹Рµ json-РєРѕРЅС„РёРіРё;
- С‚РµРєСЃС‚С‹ РєРѕРЅРєСЂРµС‚РЅРѕРіРѕ РїСЂРѕРµРєС‚Р°.

Р­С‚Рѕ Р·РЅР°С‡РёС‚:

- СЃР°Рј РјРµС…Р°РЅРёР·Рј generated resource paths РјРѕР¶РЅРѕ СЃРѕС…СЂР°РЅРёС‚СЊ;
- СЃРѕРґРµСЂР¶РёРјРѕРµ С‚РµРєСѓС‰РµРіРѕ С„Р°Р№Р»Р° РЅРµР»СЊР·СЏ СЃС‡РёС‚Р°С‚СЊ foundation;
- С„Р°Р№Р» РЅСѓР¶РЅРѕ РёР»Рё СЂР°Р·РґРµР»РёС‚СЊ, РёР»Рё РІС‹РЅРµСЃС‚Рё project-specific entries РІ sample/demo/game layer.

### РЎС‚Р°С‚СѓСЃ
**split / move / partially rewrite**

---

## 5.5 Audio baseline usable, but vendor-coupled

`AudioBaseService` вЂ” СЂР°Р±РѕС‡РёР№ РёРјРїРѕСЂС‚РёСЂРѕРІР°РЅРЅС‹Р№ baseline, РЅРѕ СЃРµР№С‡Р°СЃ РѕРЅ СЃРёР»СЊРЅРѕ РїСЂРёРІСЏР·Р°РЅ Рє FMOD runtime.

РџСЂРѕР±Р»РµРјС‹:

- platform API Рё FMOD-specific implementation РЅРµ СЂР°Р·РІРµРґРµРЅС‹;
- volume policy Р¶РёРІС‘С‚ РїСЂСЏРјРѕ РІРЅСѓС‚СЂРё service implementation;
- settings integration РµС‰С‘ РЅРµ РѕС„РѕСЂРјР»РµРЅР°;
- СЃРµСЂРІРёСЃ РїРѕРєР° С‚СЂСѓРґРЅРѕ СЃС‡РёС‚Р°С‚СЊ neutral foundation API.

### РЎС‚Р°С‚СѓСЃ
**keep as baseline + normalize later**

---

## 5.6 Camera baseline imported, but currently game-specific

Camera СЃР»РѕР№ СѓР¶Рµ РёРјРїРѕСЂС‚РёСЂРѕРІР°РЅ, РЅРѕ СЃРµР№С‡Р°СЃ С‚Р°Рј РІРёРґРЅС‹ РїСЂРёР·РЅР°РєРё project-specific design.

Р“Р»Р°РІРЅС‹Р№ РјР°СЂРєРµСЂ вЂ” `CameraStateEnum` СѓР¶Рµ СЃРѕРґРµСЂР¶РёС‚ РёРіСЂРѕРІС‹Рµ СЃРѕСЃС‚РѕСЏРЅРёСЏ РІСЂРѕРґРµ:

- `MainMenu`
- `TrainWatch`
- `DiceGame`
- `DiceGameCombinations`
- `Inventory`

Р­С‚Рѕ С…РѕСЂРѕС€РёР№ legacy baseline, РЅРѕ РЅРµ platform-level С„РёРЅР°Р»СЊРЅР°СЏ С„РѕСЂРјР°.

### Р§С‚Рѕ СЃ СЌС‚РёРј РґРµР»Р°С‚СЊ

- СЃРѕС…СЂР°РЅРёС‚СЊ service РєР°Рє baseline;
- РІС‹РЅРµСЃС‚Рё/СѓР±СЂР°С‚СЊ game-specific states;
- РїСЂРёРґСѓРјР°С‚СЊ reusable camera policy;
- РѕСЃС‚Р°РІРёС‚СЊ РёРіСЂРѕРІРѕР№ РЅР°Р±РѕСЂ СЂРµР¶РёРјРѕРІ РґР»СЏ sample/game layer, Р° РЅРµ РґР»СЏ foundation.

### РЎС‚Р°С‚СѓСЃ
**keep as baseline + rewrite specific parts**

---

## 5.7 Runtime, editor and sample payload: РіСЂР°РЅРёС†С‹ СѓР»СѓС‡С€РµРЅС‹

РЎРµР№С‡Р°СЃ РІ РґРµСЂРµРІРµ СЂРµРїРѕР·РёС‚РѕСЂРёСЏ СЂСЏРґРѕРј Р¶РёРІСѓС‚:

- runtime services;
- editor tools;
- placeholder materials;
- texture packs;
- generated content;
- UI resource references.

Р­С‚Рѕ РІСЃС‘ РµС‰С‘ Р·РѕРЅР° РґР»СЏ С‚РѕС‡РµС‡РЅРѕРіРѕ polish, РЅРѕ РєСЂРёС‚РёС‡РЅС‹Р№ baseline cleanup СѓР¶Рµ РІС‹РїРѕР»РЅРµРЅ.

### РЎС‚Р°С‚СѓСЃ
**partially resolved, polish only**

---

## 5.8 Async Awaiter foundation СѓР¶Рµ РґРѕР±Р°РІР»РµРЅ

Р’ foundation РїСЂРёСЃСѓС‚СЃС‚РІСѓРµС‚ РјРёРЅРёРјР°Р»СЊРЅС‹Р№ async awaiter СЃР»РѕР№ (D6-style РїРѕ РЅР°Р·РЅР°С‡РµРЅРёСЋ), Р·Р°СЂРµРіРёСЃС‚СЂРёСЂСѓРµРјС‹Р№ С‡РµСЂРµР· platform-level service extensions.

### РЎС‚Р°С‚СѓСЃ
**keep as platform-level foundation**

---

## 6. РљР°СЂС‚Р° СЃС‚Р°С‚СѓСЃРѕРІ РїРѕ РѕСЃРЅРѕРІРЅС‹Рј Р±Р»РѕРєР°Рј

| Р‘Р»РѕРє | РЎС‚Р°С‚СѓСЃ | Р РµС€РµРЅРёРµ |
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

## 7. Р§С‚Рѕ РґРµР»Р°С‚СЊ СЃР»РµРґСѓСЋС‰РёРј

РџРѕСЂСЏРґРѕРє Р±Р»РёР¶Р°Р№С€РёС… С€Р°РіРѕРІ РґРѕР»Р¶РµРЅ Р±С‹С‚СЊ С‚Р°РєРёРј:

1. `Reusable Gameplay Layer` (РѕСЃРЅРѕРІРЅРѕР№ СЃР»РµРґСѓСЋС‰РёР№ РєР°РЅРґРёРґР°С‚)
2. `Composition sanity cleanup` (РјР°Р»РµРЅСЊРєРёР№ РѕРїС†РёРѕРЅР°Р»СЊРЅС‹Р№ С€Р°Рі РїРµСЂРµРґ gameplay, РµСЃР»Рё СЂРµР°Р»СЊРЅРѕ РЅСѓР¶РµРЅ)

---

## 8. Р§С‚Рѕ РїРѕРєР° РЅРµ РґРµР»Р°С‚СЊ

РџРѕРєР° РЅРµ РґРµР»Р°С‚СЊ:

- gameplay systems;
- gameplay UI;
- shop / inventory / HUD / pause menu;
- analytics;
- FishNet;
- Р±РѕР»СЊС€РѕР№ rename-driven rewrite РІСЃРµРіРѕ СЂРµРїРѕР·РёС‚РѕСЂРёСЏ;
- РІРѕР·РІСЂР°С‚ localization РІ PlatformCore foundation.

---

## 9. Р“Р»Р°РІРЅС‹Р№ РІС‹РІРѕРґ

`PlatformCore` СѓР¶Рµ СЃРѕРґРµСЂР¶РёС‚ СЂРµР°Р»СЊРЅСѓСЋ foundation base.

Р—РЅР°С‡РёС‚ РґР°Р»СЊРЅРµР№С€Р°СЏ СЂР°Р±РѕС‚Р° РґРѕР»Р¶РЅР° РёРґС‚Рё РЅРµ РїРѕ Р»РѕРіРёРєРµ В«С‡С‚Рѕ Р±С‹ РµС‰С‘ РїРµСЂРµРЅРµСЃС‚РёВ», Р° РїРѕ Р»РѕРіРёРєРµ:

- С‡С‚Рѕ РёР· СѓР¶Рµ РёРјРїРѕСЂС‚РёСЂРѕРІР°РЅРЅРѕРіРѕ СЏРІР»СЏРµС‚СЃСЏ РѕСЃРЅРѕРІРѕР№ РїР»Р°С‚С„РѕСЂРјС‹;
- С‡С‚Рѕ РёР· СѓР¶Рµ РёРјРїРѕСЂС‚РёСЂРѕРІР°РЅРЅРѕРіРѕ Р»РѕРјР°РµС‚ С‡РёСЃС‚РѕС‚Сѓ РїР»Р°С‚С„РѕСЂРјС‹;
- С‡С‚Рѕ РЅСѓР¶РЅРѕ РёСЃРїСЂР°РІРёС‚СЊ СЃРµР№С‡Р°СЃ, С‡С‚РѕР±С‹ СЃР»РµРґСѓСЋС‰РёРµ РїРµСЂРµРЅРѕСЃС‹ РЅРµ СѓС…СѓРґС€Р°Р»Рё СЃРёС‚СѓР°С†РёСЋ.

РўРµРєСѓС‰РёР№ baseline С…РѕСЂРѕС€РёР№ РєР°Рє РѕС‚РїСЂР°РІРЅР°СЏ С‚РѕС‡РєР°, РЅРѕ РµРіРѕ РЅСѓР¶РЅРѕ:

- СЃС‚Р°Р±РёР»РёР·РёСЂРѕРІР°С‚СЊ;
- РїРѕС‡РёСЃС‚РёС‚СЊ;
- СЂР°Р·СЂРµР·Р°С‚СЊ РїРѕ РјРѕРґСѓР»СЏРј;
- Рё С‚РѕР»СЊРєРѕ РїРѕС‚РѕРј СЂР°СЃС€РёСЂСЏС‚СЊ РґР°Р»СЊС€Рµ.

