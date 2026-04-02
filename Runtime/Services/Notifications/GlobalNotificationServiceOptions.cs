using PlatformCore.Services.Factory;

namespace PlatformCore.Services.Notifications
{
	public sealed class GlobalNotificationServiceOptions
	{
		public string ToastItemResourcePath { get; set; } = ResourcePaths.Platform.UI.UINotificationView;
		public string PositiveNotificationSound { get; set; }
		public string NegativeNotificationSound { get; set; }
	}
}
