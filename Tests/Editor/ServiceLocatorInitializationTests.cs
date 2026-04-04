using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using PlatformCore.Core;
using PlatformCore.Infrastructure;

namespace PlatformCore.Tests.Editor
{
	public sealed class ServiceLocatorInitializationTests
	{
		[Test]
		public void InitializeAllAsync_InitializesAsyncAndSyncServices()
		{
			var locator = new ServiceLocator();
			var asyncService = new TestAsyncService();
			var syncService = new TestSyncService();

			locator.Register<TestAsyncService, TestAsyncService>(asyncService);
			locator.Register<TestSyncService, TestSyncService>(syncService);

			locator.InitializeAllAsync(CancellationToken.None).GetAwaiter().GetResult();

			Assert.IsTrue(asyncService.PreInitialized);
			Assert.IsTrue(asyncService.PostInitialized);
			Assert.IsTrue(syncService.Initialized);
		}

		private sealed class TestAsyncService : IAsyncInitializable
		{
			public bool PreInitialized { get; private set; }
			public bool PostInitialized { get; private set; }

			public UniTask PreInitializeAsync(CancellationToken ct)
			{
				PreInitialized = true;
				return UniTask.CompletedTask;
			}

			public UniTask PostInitializeAsync(CancellationToken ct)
			{
				PostInitialized = true;
				return UniTask.CompletedTask;
			}

			public void Dispose()
			{
			}
		}

		private sealed class TestSyncService : ISyncInitializable
		{
			public bool Initialized { get; private set; }

			public void Initialize()
			{
				Initialized = true;
			}

			public void Dispose()
			{
			}
		}
	}
}
