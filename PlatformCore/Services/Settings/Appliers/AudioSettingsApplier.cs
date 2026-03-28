using PlatformCore.Core;
using PlatformCore.Services.Audio;

namespace PlatformCore.Services.Settings.Appliers
{
	public sealed class AudioSettingsApplier : ISyncInitializable
	{
		private readonly ISettingsService _settingsService;
		private readonly IAudioService _audioService;

		public AudioSettingsApplier(ISettingsService settingsService, IAudioService audioService)
		{
			_settingsService = settingsService;
			_audioService = audioService;
		}

		public void Initialize()
		{
			_settingsService.AudioChanged += OnAudioChanged;
			Apply(_settingsService.Current.Audio);
		}

		public void Dispose()
		{
			_settingsService.AudioChanged -= OnAudioChanged;
		}

		private void OnAudioChanged(AudioSettingsData data)
		{
			Apply(data);
		}

		private void Apply(AudioSettingsData data)
		{
			_audioService.ApplyVolumeSettings(data.MasterVolume, data.MusicVolume, data.SfxVolume, data.IsMuted);
		}
	}
}
