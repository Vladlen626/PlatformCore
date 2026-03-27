using Newtonsoft.Json;
using UnityEngine;

namespace PlatformCore.Services.Settings
{
	public sealed class PlayerPrefsSettingsPersistence : ISettingsPersistence
	{
		private const string SettingsKey = "platform.settings.v1";

		public bool TryLoad(out PlatformSettingsData data)
		{
			if (!PlayerPrefs.HasKey(SettingsKey))
			{
				data = null;
				return false;
			}

			var json = PlayerPrefs.GetString(SettingsKey);
			if (string.IsNullOrWhiteSpace(json))
			{
				data = null;
				return false;
			}

			data = JsonConvert.DeserializeObject<PlatformSettingsData>(json);
			if (data == null)
			{
				return false;
			}

			data.EnsureDefaults();
			return true;
		}

		public void Save(PlatformSettingsData data)
		{
			var json = JsonConvert.SerializeObject(data);
			PlayerPrefs.SetString(SettingsKey, json);
			PlayerPrefs.Save();
		}
	}
}
