using Cysharp.Threading.Tasks;
using PlatformCore.Services;
using PlatformCore.Services.Network;

namespace PlatformCore.Infrastructure.Network.FishNet
{
	public static class ServiceLocatorFishNetExtensions
	{
		public static INetworkSessionService RegisterFishNetFoundation(this ServiceLocator serviceLocator,
			LifecycleService lifecycleService,
			INetworkSessionBridge networkSessionBridge,
			ILoggerService loggerService = null)
		{
			var networkSessionService = new NetworkSessionService(networkSessionBridge, loggerService);
			serviceLocator.Register<INetworkSessionService, NetworkSessionService>(networkSessionService);
			lifecycleService.RegisterAsync(networkSessionService).Forget();
			return networkSessionService;
		}
	}
}
