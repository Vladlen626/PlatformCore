# Agent Instructions

PlatformCore is a reusable Unity foundation, not a game framework.

## Architecture
- Keep game-specific logic out of PlatformCore.
- Use existing startup flow: BaseBootstrap -> BaseGameRoot -> ServiceLocator -> LifecycleService.
- Services are global reusable systems registered in ServiceLocator.
- Controllers are runtime orchestration units registered in LifecycleService.
- Models hold state and domain data.
- Views are MonoBehaviour/UI presentation and input forwarding only.

## Simplicity
- Prefer KISS and YAGNI.
- Make the smallest local change that solves the problem.
- Do not add abstractions, managers, factories, events, composites, or interfaces unless there is an immediate use.
- Composite is optional. Use it only when it clearly owns multiple controllers/disposables.
- Prefer direct constructor dependencies from composition root.
- Avoid new static Locator calls.

## Errors
- Do not hide setup errors behind fallbacks.
- Do not create silent no-op fallback behavior.
- Missing required prefab, view, service, pool, or serialized reference must fail clearly.
- Use clear exceptions or error logs instead of guessing default behavior.

## Boundaries
- FishNet integration must stay narrow foundation glue.
- Do not add gameplay networking rules to PlatformCore.
- Platform resources use ResourcePaths.Platform.
- Project resources must stay in the consumer project.

## Code style
- Keep activate/deactivate and subscribe/unsubscribe symmetric.
- Keep code short and explicit.
- Always use braces after if.
