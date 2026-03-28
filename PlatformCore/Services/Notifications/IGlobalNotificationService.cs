using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IGlobalNotificationService
{
	void ShowBanner(string key, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true);
	UniTask ShowBannerAsync(string key, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true);
	UniTask ShowBannerAsync(string key, IReadOnlyList<string> args, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true);
	void ShowBannerRaw(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true);
	UniTask ShowBannerRawAsync(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true);

	void EnqueueToast(string key, bool isNegative = false);
	UniTask EnqueueToastAsync(string key, bool isNegative = false);
	UniTask EnqueueToastAsync(string key, IReadOnlyList<string> args, bool isNegative = false);
	void EnqueueToastRaw(string message, bool isNegative = false);
	UniTask EnqueueToastRawAsync(string message, bool isNegative = false);
}
