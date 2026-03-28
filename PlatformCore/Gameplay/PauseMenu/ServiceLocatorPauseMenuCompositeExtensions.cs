using PlatformCore.Gameplay.Settings;
using PlatformCore.Infrastructure;

namespace PlatformCore.Gameplay.PauseMenu
{
	public static class ServiceLocatorPauseMenuCompositeExtensions
	{
		public static PauseMenuComposite CreatePauseMenuComposite(this ServiceLocator serviceLocator)
		{
			var lifecycle = serviceLocator.Get<LifecycleService>();
			var settingsComposite = serviceLocator.CreateSettingsComposite();
			return new PauseMenuComposite(lifecycle, settingsComposite);
		}
	}
}
