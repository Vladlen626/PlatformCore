using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlatformCore.Services.Audio;
using PlatformCore.Services.Factory;
using PlatformCore.Services.UI;
using UnityEngine;

namespace PlatformCore.Services.Notifications
{
	public class GlobalNotificationService : BaseAsyncService, IGlobalNotificationService, IDisposable
	{
		private readonly IUIService _uiService;
		private readonly IObjectFactory _objectFactory;
		private readonly IAudioService _audioService;
		private readonly GlobalNotificationServiceOptions _options;

		private UIGlobalNotificationView _bannerView;
		private UINotificationsView _notificationsView;
		private CancellationTokenSource _disposeCts;
		private UniTask _toastQueueTail;
		private bool _isInitialized;
		private bool _isDisposed;

		public GlobalNotificationService(
			IUIService uiService,
			IObjectFactory objectFactory,
			IAudioService audioService,
			GlobalNotificationServiceOptions options = null)
		{
			_uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
			_objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
			_audioService = audioService;
			_options = options ?? new GlobalNotificationServiceOptions();
			_disposeCts = new CancellationTokenSource();
			_toastQueueTail = UniTask.CompletedTask;
		}

		protected override async UniTask OnPostInitializeAsync(CancellationToken ct)
		{
			ThrowIfDisposed();
			await _uiService.PreloadAsync<UIGlobalNotificationView>();
				_bannerView = _uiService.GetWindow<UIGlobalNotificationView>();
				if (!_bannerView)
				{
					throw new MissingReferenceException("UIGlobalNotificationView is required but was not found after preload.");
				}

			await _uiService.PreloadAsync<UINotificationsView>();
				_notificationsView = _uiService.GetWindow<UINotificationsView>();
				if (!_notificationsView)
				{
					throw new MissingReferenceException("UINotificationsView is required but was not found after preload.");
				}

			_bannerView.gameObject.SetActive(true);
			_notificationsView.gameObject.SetActive(true);
			_bannerView.Hide();
			_notificationsView.Show();
			_isInitialized = true;
		}

		public void ShowBanner(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
		{
			ShowBannerAsync(message, holdSeconds, isNegative, playSound).Forget();
		}

		public async UniTask ShowBannerAsync(string message, float holdSeconds = 0.9f, bool isNegative = false, bool playSound = true)
		{
			ThrowIfDisposed();
			ThrowIfNotInitialized();
			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			_bannerView.Interrupt();
			if (playSound)
			{
				PlayNotificationSound(isNegative);
			}

			await _bannerView.PlayAsync(message, holdSeconds, isNegative);
		}

		public void EnqueueToast(string message, bool isNegative = false)
		{
			EnqueueToastAsync(message, isNegative).Forget();
		}

		public UniTask EnqueueToastAsync(string message, bool isNegative = false)
		{
			ThrowIfDisposed();
			ThrowIfNotInitialized();
			if (string.IsNullOrWhiteSpace(message))
			{
				return UniTask.CompletedTask;
			}

			var previous = _toastQueueTail;
			var queued = QueueToastInternal(previous, message, isNegative);
			_toastQueueTail = queued;
			return queued;
		}

		public void ShowToastRawImmediate(string message, bool isNegative = false)
		{
			ShowToastRawImmediateAsync(message, isNegative).Forget();
		}

		public UniTask ShowToastRawImmediateAsync(string message, bool isNegative = false)
		{
			ThrowIfDisposed();
			ThrowIfNotInitialized();
			if (string.IsNullOrWhiteSpace(message))
			{
				return UniTask.CompletedTask;
			}

			return ShowToastInternal(message, isNegative);
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			_isDisposed = true;
			_disposeCts.Cancel();
			_disposeCts.Dispose();
			_disposeCts = null;
			_bannerView = null;
			_notificationsView = null;
			_toastQueueTail = UniTask.CompletedTask;
			_isInitialized = false;
		}

		private async UniTask QueueToastInternal(UniTask previous, string message, bool isNegative)
		{
			var disposeToken = _disposeCts.Token;

			try
			{
				await previous;
			}
			catch (Exception exception)
			{
				Debug.LogError($"[GlobalNotificationService] Previous toast task failed: {exception}");
			}

			disposeToken.ThrowIfCancellationRequested();
			await ShowToastInternal(message, isNegative);
		}

		private async UniTask ShowToastInternal(string message, bool isNegative)
		{
			if (string.IsNullOrWhiteSpace(_options.ToastItemResourcePath))
			{
				throw new ArgumentException("ToastItemResourcePath is required for notification toasts.", nameof(_options.ToastItemResourcePath));
			}

			var view = await _objectFactory.CreateAsync<UINotificationView>(
				_options.ToastItemResourcePath,
				Vector3.zero,
				Quaternion.identity,
				_notificationsView.List);

			if (!view)
			{
				throw new MissingReferenceException("UINotificationView prefab must contain UINotificationView component.");
			}

			view.gameObject.SetActive(true);
			PlayNotificationSound(isNegative);

			try
			{
				await view.PlayAsync(message, isNegative);
			}
			finally
			{
				if (view)
				{
					UnityEngine.Object.Destroy(view.gameObject);
				}
			}
		}

		private void PlayNotificationSound(bool isNegative)
		{
			if (_audioService == null)
			{
				return;
			}

			var soundEvent = isNegative
				? _options.NegativeNotificationSound
				: _options.PositiveNotificationSound;
			if (string.IsNullOrWhiteSpace(soundEvent))
			{
				return;
			}

			_audioService.PlaySound(soundEvent);
		}

		private void ThrowIfDisposed()
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException(nameof(GlobalNotificationService));
			}
		}

		private void ThrowIfNotInitialized()
		{
			if (!_isInitialized)
			{
				throw new Exception("GlobalNotificationService is not initialized.");
			}
		}
	}
}
