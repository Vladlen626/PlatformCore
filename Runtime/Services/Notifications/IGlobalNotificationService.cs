using Cysharp.Threading.Tasks;

namespace PlatformCore.Services.Notifications
{
	public interface IGlobalNotificationService
	{
		void ShowBanner(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true);
		UniTask ShowBannerAsync(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true);

		void EnqueueToast(string message, bool isNegative = false);
		UniTask EnqueueToastAsync(string message, bool isNegative = false);
	}
}
