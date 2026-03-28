using PlatformCore.Services.Factory;

public sealed class GlobalNotificationServiceOptions
{
	public string ToastItemResourcePath { get; set; } = ResourcePaths.Platform.UI.UINotificationView;
	public string PositiveNotificationSound { get; set; }
	public string NegativeNotificationSound { get; set; }
}
