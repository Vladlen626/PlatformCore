# Tests

Editor smoke tests for `com.len.platformcore` foundation.

Current coverage:
- `LifecycleServiceTests` (duplicate register/idempotent unregister/dispose semantics)
- `ServiceLocatorInitializationTests` (sync + async service initialization path)

Run in Unity Test Runner (`EditMode`).

For consumer projects, add `com.len.platformcore` to `manifest.json -> testables` to enable package tests in Test Runner.
