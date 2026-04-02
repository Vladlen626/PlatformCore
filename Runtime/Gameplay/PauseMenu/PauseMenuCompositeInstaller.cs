using PlatformCore.Infrastructure.Composition;

namespace PlatformCore.Gameplay.PauseMenu
{
	public sealed class PauseMenuCompositeInstaller : CompositeInstaller<PauseMenuComposite>
	{
		public PauseMenuCompositeInstaller(PauseMenuComposite composite) : base(composite)
		{
		}

		protected override void Install(PauseMenuComposite composite, CompositeBuilder builder)
		{
			builder.AddOwnedDisposable(composite.SettingsComposite);
			builder.AddController(composite.SettingsComposite);

			var flowController = new PauseMenuFlowController(composite.SettingsComposite);
			composite.SetFlowController(flowController);
			builder.AddController(flowController);
		}
	}
}
