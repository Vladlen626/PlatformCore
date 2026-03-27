namespace PlatformCore.Services.Settings
{
	public interface ISettingsPersistence
	{
		bool TryLoad(out PlatformSettingsData data);
		void Save(PlatformSettingsData data);
	}
}
