using PlatformCore.Services;
using PlatformCore.Services.UI;
using PlatformCore.Services.UI.SplashScreen;

namespace PlatformCore.Infrastructure.UI
{
	public static class ServiceLocatorSplashScreenExtensions
	{
		public static ISplashScreenService RegisterSplashScreenFoundation(
			this ServiceLocator serviceLocator,
			ILoggerService loggerService = null)
		{
			var uiService = serviceLocator.Get<IUIService>();
			var splashScreenService = new SplashScreenService(uiService, loggerService);
			serviceLocator.Register<ISplashScreenService, SplashScreenService>(splashScreenService);
			return splashScreenService;
		}
	}
}
