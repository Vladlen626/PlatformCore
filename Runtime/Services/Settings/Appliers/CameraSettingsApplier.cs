using PlatformCore.Core;

namespace PlatformCore.Services.Settings.Appliers
{
	public sealed class CameraSettingsApplier : ISyncInitializable
	{
		private readonly ISettingsService _settingsService;
		private readonly ICameraService _cameraService;

		public CameraSettingsApplier(ISettingsService settingsService, ICameraService cameraService)
		{
			_settingsService = settingsService;
			_cameraService = cameraService;
		}

		public void Initialize()
		{
			_settingsService.CameraChanged += OnCameraChanged;
			Apply(_settingsService.Current.Camera);
		}

		public void Dispose()
		{
			_settingsService.CameraChanged -= OnCameraChanged;
		}

		private void OnCameraChanged(CameraSettingsData data)
		{
			Apply(data);
		}

		private void Apply(CameraSettingsData data)
		{
			_cameraService.SetFOV(data.FieldOfView);
			_cameraService.SetDutch(data.Dutch);
		}
	}
}
