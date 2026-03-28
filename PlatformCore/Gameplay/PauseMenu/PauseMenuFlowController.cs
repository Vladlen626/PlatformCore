using System;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;
using PlatformCore.Gameplay.Settings;

namespace PlatformCore.Gameplay.PauseMenu
{
	public sealed class PauseMenuFlowController : IBaseController, IActivatable, IDeactivatable
	{
		private readonly SettingsComposite _settingsComposite;
		private bool _isOpen;

		public PauseMenuFlowController(SettingsComposite settingsComposite)
		{
			_settingsComposite = settingsComposite;
		}

		public event Action Opened;
		public event Action Closed;
		public event Action<bool> StateChanged;

		public bool IsOpen => _isOpen;
		public bool IsSettingsOpen => _settingsComposite.FlowController != null && _settingsComposite.FlowController.IsOpen;

		public void Activate()
		{
		}

		public void Deactivate()
		{
			Close();
		}

		public void Open()
		{
			if (_isOpen)
			{
				return;
			}

			_isOpen = true;
			Opened?.Invoke();
			StateChanged?.Invoke(_isOpen);
		}

		public void Close()
		{
			if (_isOpen == false)
			{
				return;
			}

			CloseSettings();
			_isOpen = false;
			Closed?.Invoke();
			StateChanged?.Invoke(_isOpen);
		}

		public void Toggle()
		{
			if (_isOpen)
			{
				Close();
				return;
			}

			Open();
		}

		public void OpenSettings()
		{
			if (_isOpen == false)
			{
				Open();
			}

			if (_settingsComposite.FlowController != null)
			{
				_settingsComposite.FlowController.Open();
			}
		}

		public void CloseSettings()
		{
			if (_settingsComposite.FlowController != null)
			{
				_settingsComposite.FlowController.Close();
			}
		}
	}
}
