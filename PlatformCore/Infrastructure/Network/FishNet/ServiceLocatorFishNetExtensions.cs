using Cysharp.Threading.Tasks;
using PlatformCore.Services;
using PlatformCore.Services.Network;

namespace PlatformCore.Infrastructure.Network.FishNet
{
	public static class ServiceLocatorFishNetExtensions
	{
		public static INetworkSessionService RegisterFishNetFoundation(this ServiceLocator serviceLocator, ILoggerService loggerService = null)
		{
			var lifecycleService = serviceLocator.Get<LifecycleService>();
			var networkSessionBridge = new FishNetSessionBridge();
			serviceLocator.Register<INetworkSessionBridge, FishNetSessionBridge>(networkSessionBridge);
			return serviceLocator.RegisterFishNetFoundation(lifecycleService, networkSessionBridge, loggerService);
		}

		public static INetworkSessionService RegisterFishNetFoundation(this ServiceLocator serviceLocator,
			LifecycleService lifecycleService,
			INetworkSessionBridge networkSessionBridge,
			ILoggerService loggerService = null)
		{
			var networkSessionService = new NetworkSessionService(networkSessionBridge, loggerService);
			serviceLocator.Register<INetworkSessionService, NetworkSessionService>(networkSessionService);
			lifecycleService.RegisterAsync(networkSessionService).Forget();

			var runtimeSessionAdapter = new FishNetRuntimeSessionAdapter(networkSessionBridge, loggerService);
			lifecycleService.RegisterAsync(runtimeSessionAdapter).Forget();

			return networkSessionService;
		}
	}
}
