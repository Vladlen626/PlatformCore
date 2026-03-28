using PlatformCore.Infrastructure;
using PlatformCore.Infrastructure.Composition;
using PlatformCore.Services.Settings;

namespace PlatformCore.Gameplay.Settings
{
	public sealed class SettingsComposite : Composite
	{
		private readonly ISettingsService _settingsService;
		public SettingsFlowController FlowController { get; private set; }

		public SettingsComposite(LifecycleService lifecycle, ISettingsService settingsService) : base(lifecycle)
		{
			_settingsService = settingsService;
		}

		protected override CompositeInstaller CreateInstaller()
		{
			return new SettingsCompositeInstaller(this);
		}

		internal ISettingsService SettingsService => _settingsService;
		internal void SetFlowController(SettingsFlowController controller)
		{
			FlowController = controller;
		}
	}
}
