using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlatformCore.Services.Audio;
using PlatformCore.Services.Factory;
using PlatformCore.Services.UI;

namespace PlatformCore.Services.Notifications
{
	public class GlobalNotificationService : BaseAsyncService, IGlobalNotificationService
	{
		private readonly IUIService uiService;
		private readonly IObjectFactory objectFactory;
		private readonly IAudioService audioService;
		private readonly GlobalNotificationServiceOptions options;

		private UIGlobalNotificationView bannerView;
		private UINotificationsView notificationsView;
		private UniTask toastQueueTail;

		public GlobalNotificationService(
			IUIService uiService,
			IObjectFactory objectFactory,
			IAudioService audioService,
			GlobalNotificationServiceOptions options = null)
		{
			this.uiService = uiService;
			this.objectFactory = objectFactory;
			this.audioService = audioService;
			this.options = options ?? new GlobalNotificationServiceOptions();
			toastQueueTail = UniTask.CompletedTask;
		}

		protected override async UniTask OnPostInitializeAsync(CancellationToken ct)
		{
			if (uiService == null)
			{
				return;
			}

			await uiService.PreloadAsync<UIGlobalNotificationView>();
			bannerView = uiService.GetWindow<UIGlobalNotificationView>();
			bannerView.gameObject.SetActive(true);
			if (bannerView)
			{
				bannerView.Hide();
			}

			await uiService.PreloadAsync<UINotificationsView>();
			notificationsView = uiService.GetWindow<UINotificationsView>();
			notificationsView.gameObject.SetActive(true);
			if (notificationsView)
			{
				notificationsView.Show();
			}
		}

		public void ShowBanner(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
		{
			ShowBannerAsync(message, holdSeconds, isNegative, playSound).Forget();
		}

		public async UniTask ShowBannerAsync(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
		{
			if (string.IsNullOrWhiteSpace(message) || !bannerView)
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

		public void EnqueueToast(string message, bool isNegative = false)
		{
			EnqueueToastAsync(message, isNegative).Forget();
		}

		public UniTask EnqueueToastAsync(string message, bool isNegative = false)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return UniTask.CompletedTask;
			}

			var previous = toastQueueTail;
			var queued = QueueToastInternal(previous, message, isNegative);
			toastQueueTail = queued;
			return queued;
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

		private async UniTask QueueToastInternal(UniTask previous, string message, bool isNegative)
		{
			try
			{
				await previous;
			}
			catch (Exception)
			{
			}

			await ShowToastInternal(message, isNegative);
		}

		private async UniTask ShowToastInternal(string message, bool isNegative)
		{
			if (!notificationsView || !notificationsView.List || objectFactory == null || string.IsNullOrWhiteSpace(options.ToastItemResourcePath))
			{
				return;
			}

			var view = await objectFactory.CreateAsync<UINotificationView>(
				options.ToastItemResourcePath,
				UnityEngine.Vector3.zero,
				UnityEngine.Quaternion.identity,
				notificationsView.List);

			if (!view)
			{
				return;
			}
			
			view.gameObject.SetActive(true);

			PlayNotificationSound(isNegative);
			await view.PlayAsync(message, isNegative);
			UnityEngine.Object.Destroy(view.gameObject);
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
}
