using PlatformCore.Services;
using PlatformCore.Services.Audio;
using PlatformCore.Services.Settings;
using PlatformCore.Services.Settings.Appliers;

namespace PlatformCore.Infrastructure.Settings
{
	public static class ServiceLocatorSettingsExtensions
	{
		public static SettingsService RegisterSettingsFoundation(this ServiceLocator serviceLocator)
		{
			var settingsService = new SettingsService(new PlayerPrefsSettingsPersistence());
			serviceLocator.Register<ISettingsService, SettingsService>(settingsService);
			return settingsService;
		}

		public static void RegisterAudioSettingsApplier(this ServiceLocator serviceLocator, IAudioService audioService)
		{
			var settingsService = serviceLocator.Get<ISettingsService>();
			serviceLocator.Register<AudioSettingsApplier, AudioSettingsApplier>(
				new AudioSettingsApplier(settingsService, audioService));
		}

		public static void RegisterCameraSettingsApplier(this ServiceLocator serviceLocator, ICameraService cameraService)
		{
			var settingsService = serviceLocator.Get<ISettingsService>();
			serviceLocator.Register<CameraSettingsApplier, CameraSettingsApplier>(
				new CameraSettingsApplier(settingsService, cameraService));
		}
	}
}
