using System;

namespace PlatformCore.Services.Settings
{
	[Serializable]
	public sealed class PlatformSettingsData
	{
		public AudioSettingsData Audio = new();
		public CameraSettingsData Camera = new();
		public UISettingsData UI = new();

		public void EnsureDefaults()
		{
			Audio ??= new AudioSettingsData();
			Camera ??= new CameraSettingsData();
			UI ??= new UISettingsData();
		}
	}

	[Serializable]
	public sealed class AudioSettingsData
	{
		public float MasterVolume = 0.8f;
		public float MusicVolume = 0.5f;
		public float SfxVolume = 0.5f;
		public bool IsMuted;
	}

	[Serializable]
	public sealed class CameraSettingsData
	{
		public float FieldOfView = 60f;
		public float Dutch = 0f;
	}

	[Serializable]
	public sealed class UISettingsData
	{
		public float Scale = 1f;
	}
}
