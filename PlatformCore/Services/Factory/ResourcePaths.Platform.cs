namespace PlatformCore.Services.Factory
{
	public static partial class ResourcePaths
	{
		/// <summary>
		/// Reusable paths owned by PlatformCore runtime/editor foundation.
		/// These entries must remain game-agnostic.
		/// </summary>
		public static class Platform
		{
			public static class Root
			{
				public const string DOTweenSettings = "DOTweenSettings";
			}

			public static class UI
			{
				public const string ColorStyleLibrary = "UI/ColorStyleLibrary";
				public const string TextStyleLibrary = "UI/TextStyleLibrary";
				public const string UICursorView = "UI/UICursorView";
			}
		}
	}
}
