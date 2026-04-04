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
			public static class UI
			{
				public const string ColorStyleLibrary = "UI/ColorStyleLibrary";
				public const string TextStyleLibrary = "UI/TextStyleLibrary";
				public const string UIGlobalNotificationView = "UI/UIGlobalNotificationView";
				public const string UINotificationsView = "UI/UINotificationsView";
				public const string UINotificationView = "UI/UINotificationView";
			}
		}
	}
}
