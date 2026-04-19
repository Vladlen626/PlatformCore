using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlatformCore.Services;
using UnityEngine;

namespace PlatformCore.Services.UI.SplashScreen
{
	public sealed class SplashScreenService : BaseAsyncService, ISplashScreenService
	{
		private readonly IUIService _uiService;
		private readonly ILoggerService _loggerService;

		private UISplashScreenView _view;
		private bool _isTransitioning;

		public bool IsTransitioning => _isTransitioning;

		public SplashScreenService(IUIService uiService, ILoggerService loggerService = null)
		{
			_uiService = uiService;
			_loggerService = loggerService;
		}

		protected override async UniTask OnPostInitializeAsync(CancellationToken serviceToken)
		{
			await EnsureViewAsync();
			if (_view)
			{
				_view.SetVisible(false);
				_view.SetInputBlocked(false);
				_view.SetAlpha(0f);
			}
		}

		public async UniTask FadeInAsync(float duration = 1f)
		{
			await WaitForTransitionSlotAsync();
			_isTransitioning = true;
			try
			{
				if (await EnsureViewAsync() == false)
				{
					return;
				}

				_view.SetVisible(true);
				_view.SetInputBlocked(true);
				await FadeToAsync(1f, duration);
			}
			finally
			{
				_isTransitioning = false;
			}
		}

		public async UniTask FadeOutAsync(float duration = 1f)
		{
			await WaitForTransitionSlotAsync();
			_isTransitioning = true;
			try
			{
				if (await EnsureViewAsync() == false)
				{
					return;
				}

				_view.SetVisible(true);
				await FadeToAsync(0f, duration);
				_view.SetInputBlocked(false);
				_view.SetVisible(false);
			}
			finally
			{
				_isTransitioning = false;
			}
		}

		public async UniTask ShowSplashAsync(float duration = 2f, float fadeIn = 0.5f, float fadeOut = 0.5f)
		{
			await FadeInAsync(fadeIn);
			if (duration > 0f)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(duration));
			}

			await FadeOutAsync(fadeOut);
		}

		private async UniTask<bool> EnsureViewAsync()
		{
			if (_view)
			{
				return true;
			}

			await _uiService.PreloadAsync<UISplashScreenView>();
			_view = _uiService.GetWindow<UISplashScreenView>();
			if (!_view)
			{
				_loggerService?.LogWarning("[SplashScreen] UISplashScreenView is missing. Splash transitions are disabled.");
				return false;
			}

			return true;
		}

		private async UniTask WaitForTransitionSlotAsync()
		{
			while (_isTransitioning)
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
		}

		private async UniTask FadeToAsync(float targetAlpha, float duration)
		{
			var target = Mathf.Clamp01(targetAlpha);
			var safeDuration = Mathf.Max(0f, duration);
			if (safeDuration <= 0f)
			{
				_view.SetAlpha(target);
				return;
			}

			var start = _view.Alpha;
			var elapsed = 0f;
			while (elapsed < safeDuration)
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				elapsed += Time.unscaledDeltaTime;
				var t = Mathf.Clamp01(elapsed / safeDuration);
				_view.SetAlpha(Mathf.Lerp(start, target, t));
			}

			_view.SetAlpha(target);
		}
	}
}
