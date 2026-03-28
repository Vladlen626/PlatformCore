using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlatformCore.Services.Audio;
using PlatformCore.Services.Factory;
using PlatformCore.Services.UI;

public class GlobalNotificationService : BaseAsyncService, IGlobalNotificationService
{
	private readonly IUIService uiService;
	private readonly IObjectFactory objectFactory;
	private readonly ILocalizationService localizationService;
	private readonly IAudioService audioService;
	private readonly GlobalNotificationServiceOptions options;

	private UIGlobalNotificationView bannerView;
	private UINotificationsView notificationsView;
	private UniTask toastQueueTail;

	public GlobalNotificationService(
		IUIService uiService,
		IObjectFactory objectFactory,
		ILocalizationService localizationService,
		IAudioService audioService,
		GlobalNotificationServiceOptions options = null)
	{
		this.uiService = uiService;
		this.objectFactory = objectFactory;
		this.localizationService = localizationService;
		this.audioService = audioService;
		this.options = options ?? new GlobalNotificationServiceOptions();
		toastQueueTail = UniTask.CompletedTask;
	}

	public void ShowBanner(string key, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
	{
		ShowBannerAsync(key, holdSeconds, isNegative, playSound).Forget();
	}

	public UniTask ShowBannerAsync(string key, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return UniTask.CompletedTask;
		}

		var message = ResolveMessage(key);
		return ShowBannerRawAsync(message, holdSeconds, isNegative, playSound);
	}

	public UniTask ShowBannerAsync(string key, IReadOnlyList<string> args, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return UniTask.CompletedTask;
		}

		var message = ResolveMessage(key, args);
		return ShowBannerRawAsync(message, holdSeconds, isNegative, playSound);
	}

	public void ShowBannerRaw(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
	{
		ShowBannerRawAsync(message, holdSeconds, isNegative, playSound).Forget();
	}

	public async UniTask ShowBannerRawAsync(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
	{
		if (string.IsNullOrWhiteSpace(message))
		{
			return;
		}

		await EnsureBannerViewAsync();
		if (!bannerView)
		{
			return;
		}

		bannerView.Interrupt();
		if (playSound)
		{
			PlayNotificationSound(isNegative);
		}

		await bannerView.PlayAsync(message, holdSeconds, isNegative);
	}

	public void EnqueueToast(string key, bool isNegative = false)
	{
		EnqueueToastAsync(key, isNegative).Forget();
	}

	public UniTask EnqueueToastAsync(string key, bool isNegative = false)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return UniTask.CompletedTask;
		}

		var message = ResolveMessage(key);
		return EnqueueToastRawAsync(message, isNegative);
	}

	public UniTask EnqueueToastAsync(string key, IReadOnlyList<string> args, bool isNegative = false)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return UniTask.CompletedTask;
		}

		var message = ResolveMessage(key, args);
		return EnqueueToastRawAsync(message, isNegative);
	}

	public void EnqueueToastRaw(string message, bool isNegative = false)
	{
		EnqueueToastRawAsync(message, isNegative).Forget();
	}

	public UniTask EnqueueToastRawAsync(string message, bool isNegative = false)
	{
		if (string.IsNullOrWhiteSpace(message))
		{
			return UniTask.CompletedTask;
		}

		return QueueToastInternal(message, isNegative);
	}


	public void ShowToastImmediate(string key, bool isNegative = false)
	{
		ShowToastImmediateAsync(key, isNegative).Forget();
	}

	public UniTask ShowToastImmediateAsync(string key, bool isNegative = false)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return UniTask.CompletedTask;
		}

		var message = ResolveMessage(key);
		return ShowToastRawImmediateAsync(message, isNegative);
	}

	public UniTask ShowToastImmediateAsync(string key, IReadOnlyList<string> args, bool isNegative = false)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return UniTask.CompletedTask;
		}

		var message = ResolveMessage(key, args);
		return ShowToastRawImmediateAsync(message, isNegative);
	}

	public void ShowToastRawImmediate(string message, bool isNegative = false)
	{
		ShowToastRawImmediateAsync(message, isNegative).Forget();
	}

	public UniTask ShowToastRawImmediateAsync(string message, bool isNegative = false)
	{
		if (string.IsNullOrWhiteSpace(message))
		{
			return UniTask.CompletedTask;
		}

		return ShowToastInternal(message, isNegative);
	}

	private async UniTask EnsureBannerViewAsync()
	{
		if (bannerView)
		{
			return;
		}

		if (uiService == null)
		{
			return;
		}

		await uiService.PreloadAsync<UIGlobalNotificationView>();
		bannerView = uiService.GetWindow<UIGlobalNotificationView>();
		if (bannerView)
		{
			bannerView.Hide();
		}
	}

	private async UniTask EnsureNotificationsViewAsync()
	{
		if (notificationsView)
		{
			return;
		}

		if (uiService == null)
		{
			return;
		}

		await uiService.PreloadAsync<UINotificationsView>();
		notificationsView = uiService.GetWindow<UINotificationsView>();
		if (notificationsView)
		{
			notificationsView.Show();
		}
	}

	private async UniTask QueueToastInternal(string message, bool isNegative)
	{
		var previous = toastQueueTail;
		var tcs = new UniTaskCompletionSource();
		toastQueueTail = tcs.Task;

		await previous;
		await ShowToastInternal(message, isNegative);

		tcs.TrySetResult();
	}

	private async UniTask ShowToastInternal(string message, bool isNegative)
	{
		await EnsureNotificationsViewAsync();
		if (!notificationsView || objectFactory == null || string.IsNullOrWhiteSpace(options.ToastItemResourcePath))
		{
			return;
		}

		var parent = notificationsView.List ? notificationsView.List : notificationsView.transform;
		var view = await objectFactory.CreateAsync<UINotificationView>(
			options.ToastItemResourcePath,
			UnityEngine.Vector3.zero,
			UnityEngine.Quaternion.identity,
			parent);

		if (!view)
		{
			return;
		}

		if (parent && view.transform is UnityEngine.RectTransform rect)
		{
			rect.SetParent(parent, false);
		}

		var tcs = new UniTaskCompletionSource();
		void OnShowed(UINotificationView v)
		{
			view.Showed -= OnShowed;
			tcs.TrySetResult();
		}

		view.Showed += OnShowed;
		view.SetText(message, isNegative);
		PlayNotificationSound(isNegative);
		view.Show();

		await tcs.Task;

		UnityEngine.Object.Destroy(view.gameObject);
	}

	private string ResolveMessage(string key)
	{
		return localizationService?.Get(key) ?? key;
	}

	private string ResolveMessage(string key, IReadOnlyList<string> args)
	{
		if (localizationService == null)
		{
			if (args == null || args.Count == 0)
			{
				return key;
			}

			var messageArgs = ToObjectArray(args);
			return string.Format(key, messageArgs);
		}

		return localizationService.Get(key, args);
	}

	private static object[] ToObjectArray(IReadOnlyList<string> args)
	{
		var data = new object[args.Count];
		for (var i = 0; i < args.Count; i++)
		{
			data[i] = args[i];
		}

		return data;
	}

	private void PlayNotificationSound(bool isNegative)
	{
		if (audioService == null)
		{
			return;
		}

		var soundEvent = isNegative
			? options.NegativeNotificationSound
			: options.PositiveNotificationSound;
		if (string.IsNullOrWhiteSpace(soundEvent))
		{
			return;
		}

		audioService.PlaySound(soundEvent);
	}
}
