using PlatformCore.Services;
using PlatformCore.Services.Audio;
using PlatformCore.Services.Factory;
using PlatformCore.Services.UI;

namespace PlatformCore.Infrastructure.Notifications
{
	public static class ServiceLocatorNotificationExtensions
	{
		public static IGlobalNotificationService RegisterGlobalNotificationsFoundation(this ServiceLocator serviceLocator,
			IUIService uiService,
			IObjectFactory objectFactory,
			ILocalizationService localizationService,
			IAudioService audioService,
			GlobalNotificationServiceOptions options = null)
		{
			var notificationService = new GlobalNotificationService(
				uiService,
				objectFactory,
				localizationService,
				audioService,
				options);
			serviceLocator.Register<IGlobalNotificationService, GlobalNotificationService>(notificationService);
			return notificationService;
		}
	}
}
