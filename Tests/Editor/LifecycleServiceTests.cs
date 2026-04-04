using System;
using NUnit.Framework;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;
using PlatformCore.Infrastructure;

namespace PlatformCore.Tests.Editor
{
	public sealed class LifecycleServiceTests
	{
		[Test]
		public void RegisteringSameControllerTwice_ActivatesOnlyOnce()
		{
			var lifecycle = new LifecycleService();
			var controller = new TestController();

			lifecycle.RegisterAsync(controller).GetAwaiter().GetResult();
			lifecycle.RegisterAsync(controller).GetAwaiter().GetResult();
			lifecycle.Update(0.016f);

			Assert.AreEqual(1, controller.ActivateCalls);
			Assert.AreEqual(1, controller.UpdateCalls);
		}

		[Test]
		public void Unregister_IsIdempotent()
		{
			var lifecycle = new LifecycleService();
			var controller = new TestController();

			lifecycle.RegisterAsync(controller).GetAwaiter().GetResult();
			lifecycle.Unregister(controller);
			lifecycle.Unregister(controller);

			Assert.AreEqual(1, controller.DeactivateCalls);
		}

		[Test]
		public void Dispose_DeactivatesAndDisposesRegisteredControllers()
		{
			var lifecycle = new LifecycleService();
			var controller = new TestController();

			lifecycle.RegisterAsync(controller).GetAwaiter().GetResult();
			lifecycle.Dispose();

			Assert.AreEqual(1, controller.DeactivateCalls);
			Assert.AreEqual(1, controller.DisposeCalls);
		}

		private sealed class TestController : IBaseController, IActivatable, IDeactivatable, IUpdatable, IDisposable
		{
			public int ActivateCalls { get; private set; }
			public int DeactivateCalls { get; private set; }
			public int UpdateCalls { get; private set; }
			public int DisposeCalls { get; private set; }

			public void Activate()
			{
				ActivateCalls++;
			}

			public void Deactivate()
			{
				DeactivateCalls++;
			}

			public void OnUpdate(float deltaTime)
			{
				UpdateCalls++;
			}

			public void Dispose()
			{
				DisposeCalls++;
			}
		}
	}
}
