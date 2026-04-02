using System.Collections;
using UnityEngine;

namespace PlatformCore.Services.UI.SplashScreen
{
	public class SplashScreenElement : UIBaseElement
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;
		private Coroutine _fadeRoutine;

		public void OnShow(float duration)
		{
			StartFade(1f, duration);
		}

		public void OnHide(float duration)
		{
			StartFade(0f, duration);
		}

		private void StartFade(float target, float duration)
		{
			if (_fadeRoutine != null)
			{
				StopCoroutine(_fadeRoutine);
			}

			_fadeRoutine = StartCoroutine(FadeRoutine(target, duration));
		}

		private IEnumerator FadeRoutine(float target, float duration)
		{
			if (duration <= 0f)
			{
				_canvasGroup.alpha = target;
				yield break;
			}

			var start = _canvasGroup.alpha;
			var elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				var t = Mathf.Clamp01(elapsed / duration);
				_canvasGroup.alpha = Mathf.Lerp(start, target, t);
				yield return null;
			}

			_canvasGroup.alpha = target;
			_fadeRoutine = null;
		}
	}
}
