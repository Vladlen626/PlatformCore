using PlatformCore.Gameplay.Settings;
using PlatformCore.Infrastructure;
using PlatformCore.Infrastructure.Composition;

namespace PlatformCore.Gameplay.PauseMenu
{
	public sealed class PauseMenuComposite : Composite
	{
		private readonly SettingsComposite _settingsComposite;

		public PauseMenuFlowController FlowController { get; private set; }

		public PauseMenuComposite(LifecycleService lifecycle, SettingsComposite settingsComposite) : base(lifecycle)
		{
			_settingsComposite = settingsComposite;
		}

		protected override CompositeInstaller CreateInstaller()
		{
			return new PauseMenuCompositeInstaller(this);
		}

		internal SettingsComposite SettingsComposite => _settingsComposite;

		internal void SetFlowController(PauseMenuFlowController controller)
		{
			FlowController = controller;
		}
	}
}
