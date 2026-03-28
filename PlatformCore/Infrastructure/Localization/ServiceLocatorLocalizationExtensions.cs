using PlatformCore.Services;

namespace PlatformCore.Infrastructure.Localization
{
	public static class ServiceLocatorLocalizationExtensions
	{
		public static ILocalizationService RegisterLocalizationFoundation(this ServiceLocator serviceLocator,
			ConfigService configService,
			LocalizationServiceOptions options = null)
		{
			var localizationService = new LocalizationServiceBase(configService, options);
			serviceLocator.Register<ILocalizationService, LocalizationServiceBase>(localizationService);
			return localizationService;
		}
	}
}
