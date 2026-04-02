# PlatformCore 2.0 — Batch 10: Global Notifications foundation (Localization removed)

## Scope

Minimal platform-level foundation cleanup focused on global in-app notifications after localization was explicitly removed from PlatformCore scope.

## Localization status

- Localization foundation removed from PlatformCore runtime/infrastructure scope.
- `ILocalizationService`, `LocalizationServiceBase`, `LocalizationServiceOptions`, and localization registration extension are no longer part of platform foundation.
- No replacement localization subsystem is introduced in PlatformCore.

## Global in-app notifications foundation

- `IGlobalNotificationService` retained as platform contract for banner/toast usage.
- `GlobalNotificationService` keeps `GlobalNotificationServiceOptions`:
  - toast item prefab path;
  - optional positive/negative sound events.
- Removed direct dependency on `ResourcePaths.Sample.*` from notification runtime service.
- Toast enqueue path is now serialized through a small queue chain (`toastQueueTail`) to provide deterministic display policy for transient messages.

## UI foundation integration

- Notifications still integrate through `IUIService` and `UIBaseElement` views.
- Added explicit service registration extensions:
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
