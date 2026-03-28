using PlatformCore.Services.AsyncAwaiter;

namespace PlatformCore.Infrastructure.AsyncAwaiter
{
	public static class ServiceLocatorAsyncAwaiterExtensions
	{
		public static IAsyncAwaiterService RegisterAsyncAwaiterFoundation(this ServiceLocator serviceLocator)
		{
			var asyncAwaiterService = new AsyncAwaiterService();
			serviceLocator.Register<IAsyncAwaiterService, AsyncAwaiterService>(asyncAwaiterService);
			return asyncAwaiterService;
		}
	}
}
