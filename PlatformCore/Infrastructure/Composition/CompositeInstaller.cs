namespace PlatformCore.Infrastructure.Composition
{
	public abstract class CompositeInstaller
	{
		public abstract void Install(CompositeBuilder builder);
	}

	public abstract class CompositeInstaller<TComposite> : CompositeInstaller
		where TComposite : Composite
	{
		private readonly TComposite _composite;

		protected CompositeInstaller(TComposite composite)
		{
			_composite = composite;
		}

		public sealed override void Install(CompositeBuilder builder)
		{
			Install(_composite, builder);
		}

		protected abstract void Install(TComposite composite, CompositeBuilder builder);
	}
}
