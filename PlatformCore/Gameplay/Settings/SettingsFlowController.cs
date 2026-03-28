using System;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;
using PlatformCore.Services.Settings;

namespace PlatformCore.Gameplay.Settings
{
	public sealed class SettingsFlowController : IBaseController, IActivatable, IDeactivatable
	{
		private readonly ISettingsService _settingsService;
		private bool _isOpen;

		public SettingsFlowController(ISettingsService settingsService)
		{
			_settingsService = settingsService;
		}

		public event Action<PlatformSettingsData> FlowOpened;
		public event Action<PlatformSettingsData> FlowStateChanged;
		public event Action FlowClosed;

		public bool IsOpen => _isOpen;
		public PlatformSettingsData Current => _settingsService.Current;

		public void Activate()
		{
			_settingsService.SettingsChanged += OnSettingsChanged;
		}

		public void Deactivate()
		{
			if (_isOpen)
			{
				Close();
			}

			_settingsService.SettingsChanged -= OnSettingsChanged;
		}

		public void Open()
		{
			if (_isOpen)
			{
				return;
			}

			_isOpen = true;
			FlowOpened?.Invoke(_settingsService.Current);
			FlowStateChanged?.Invoke(_settingsService.Current);
		}

		public void Close()
		{
			if (_isOpen == false)
			{
				return;
			}

			_isOpen = false;
			FlowClosed?.Invoke();
		}

		public void SetAudio(float masterVolume, float musicVolume, float sfxVolume, bool isMuted)
		{
			_settingsService.UpdateAudio(data =>
			{
				data.MasterVolume = masterVolume;
				data.MusicVolume = musicVolume;
				data.SfxVolume = sfxVolume;
				data.IsMuted = isMuted;
			});
		}

		public void SetCamera(float fieldOfView, float dutch)
		{
			_settingsService.UpdateCamera(data =>
			{
				data.FieldOfView = fieldOfView;
				data.Dutch = dutch;
			});
		}

		public void SetUI(float scale)
		{
			_settingsService.UpdateUI(data =>
			{
				data.Scale = scale;
			});
		}

		private void OnSettingsChanged(PlatformSettingsData settings)
		{
			if (_isOpen == false)
			{
				return;
			}

			FlowStateChanged?.Invoke(settings);
		}
	}
}
