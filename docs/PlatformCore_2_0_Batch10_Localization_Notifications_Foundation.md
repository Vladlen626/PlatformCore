# PlatformCore 2.0 — Batch 10: Localization + Global Notifications foundation

## Scope

Minimal platform-level foundation cleanup for localization and global in-app notifications, reusing imported baseline runtime services.

## Localization foundation

- `ILocalizationService` expanded from legacy single-method contract into platform API:
  - active locale (`CurrentLocale`);
  - locale switch (`SetLocaleAsync`);
  - key resolving (`TryGet` / `Get`);
  - formatted resolving (`Get(key, args)`);
  - locale change event (`LocaleChanged`).
- `LocalizationServiceBase` now accepts `LocalizationServiceOptions` and no longer hardcodes sample `texts_eng` resource.
- locale resources are resolved through explicit `LocaleResourcePaths` mapping, so sample/demo text packs stay optional integration payload.

## Global in-app notifications foundation

- Added platform contract `IGlobalNotificationService` for banner/toast usage.
- `GlobalNotificationService` now uses `GlobalNotificationServiceOptions`:
  - toast item prefab path;
  - optional positive/negative sound events.
- Removed direct dependency on `ResourcePaths.Sample.*` from notification runtime service.
- Toast enqueue path is now serialized through a small queue chain (`toastQueueTail`) to provide deterministic display policy for transient messages.

## UI foundation integration

- Notifications still integrate through `IUIService` and `UIBaseElement` views.
- Added explicit service registration extensions:
  - `ServiceLocatorLocalizationExtensions.RegisterLocalizationFoundation`;
  - `ServiceLocatorNotificationExtensions.RegisterGlobalNotificationsFoundation`.

## Platform vs sample boundaries

- Platform-level runtime now depends on contracts/options, not on fixed sample resource ids.
- `ResourcePaths.Platform.UI` now contains notification view paths for foundation usage.
- Existing sample/demo assets can still be used by passing their paths through options, but are not mandatory runtime payload.

## Explicit non-goals

This batch intentionally does **not** include:

- gameplay-specific notifications/HUD/inventory/quest UI;
- mobile push/local OS notifications;
- localization editor tooling;
- prefab-pack migration as mandatory platform runtime;
- wide UI/audio/camera/scene/settings rewrites.
