using System;
using PlatformCore.Core;
using UnityEngine;

namespace PlatformCore.Services.Settings
{
	public sealed class SettingsService : ISettingsService, ISyncInitializable
	{
		private readonly ISettingsPersistence _persistence;

		public PlatformSettingsData Current { get; private set; } = new();
		public event Action<PlatformSettingsData> SettingsChanged;
		public event Action<AudioSettingsData> AudioChanged;
		public event Action<CameraSettingsData> CameraChanged;
		public event Action<UISettingsData> UIChanged;

		public SettingsService(ISettingsPersistence persistence)
		{
			_persistence = persistence;
		}

		public void Initialize()
		{
			Reload();
		}

		public void Reload()
		{
			if (!_persistence.TryLoad(out var data))
			{
				data = new PlatformSettingsData();
			}

			data.EnsureDefaults();
			Current = data;
			NotifyChanged();
		}

		public void Save()
		{
			Current.EnsureDefaults();
			_persistence.Save(Current);
		}

		public void UpdateAudio(Action<AudioSettingsData> update)
		{
			if (update == null)
			{
				return;
			}

			update(Current.Audio);
			ClampAudio(Current.Audio);
			Save();
			NotifyChanged();
		}

		public void UpdateCamera(Action<CameraSettingsData> update)
		{
			if (update == null)
			{
				return;
			}

			update(Current.Camera);
			Save();
			NotifyChanged();
		}

		public void UpdateUI(Action<UISettingsData> update)
		{
			if (update == null)
			{
				return;
			}

			update(Current.UI);
			Save();
			NotifyChanged();
		}

		public void Dispose()
		{
			SettingsChanged = null;
			AudioChanged = null;
			CameraChanged = null;
			UIChanged = null;
		}

		private void NotifyChanged()
		{
			SettingsChanged?.Invoke(Current);
			AudioChanged?.Invoke(Current.Audio);
			CameraChanged?.Invoke(Current.Camera);
			UIChanged?.Invoke(Current.UI);
		}

		private static void ClampAudio(AudioSettingsData data)
		{
			data.MasterVolume = Mathf.Clamp01(data.MasterVolume);
			data.MusicVolume = Mathf.Clamp01(data.MusicVolume);
			data.SfxVolume = Mathf.Clamp01(data.SfxVolume);
		}
	}
}
