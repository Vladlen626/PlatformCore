using PlatformCore.Infrastructure.Composition;

namespace PlatformCore.Gameplay.Settings
{
	public sealed class SettingsCompositeInstaller : CompositeInstaller<SettingsComposite>
	{
		public SettingsCompositeInstaller(SettingsComposite composite) : base(composite)
		{
		}

		protected override void Install(SettingsComposite composite, CompositeBuilder builder)
		{
			var flowController = new SettingsFlowController(composite.SettingsService);
			composite.SetFlowController(flowController);
			builder.AddController(flowController);
		}
	}
}
