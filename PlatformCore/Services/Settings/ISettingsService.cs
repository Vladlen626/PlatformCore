using System;

namespace PlatformCore.Services.Settings
{
	public interface ISettingsService
	{
		PlatformSettingsData Current { get; }
		event Action<PlatformSettingsData> SettingsChanged;
		event Action<AudioSettingsData> AudioChanged;
		event Action<CameraSettingsData> CameraChanged;
		event Action<UISettingsData> UIChanged;

		void Reload();
		void Save();
		void UpdateAudio(Action<AudioSettingsData> update);
		void UpdateCamera(Action<CameraSettingsData> update);
		void UpdateUI(Action<UISettingsData> update);
	}
}
