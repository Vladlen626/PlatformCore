using PlatformCore.Infrastructure;
using PlatformCore.Services.Settings;

namespace PlatformCore.Gameplay.Settings
{
	public static class ServiceLocatorSettingsCompositeExtensions
	{
		public static SettingsComposite CreateSettingsComposite(this ServiceLocator serviceLocator)
		{
			var lifecycle = serviceLocator.Get<LifecycleService>();
			var settingsService = serviceLocator.Get<ISettingsService>();
			return new SettingsComposite(lifecycle, settingsService);
		}
	}
}
