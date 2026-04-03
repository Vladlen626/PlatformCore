using System;
using PlatformCore.Services.UI;
using PlatformCore.Services.UI.Styles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformCore.Services.Notifications
{
	public class UINotificationView : UIBaseElement
	{
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private RectTransform contentRoot;
		[SerializeField] private UIBackgroundSizer backgroundSizer;
		[SerializeField] private Image backgroundImage;
		[SerializeField] private float initialShift = 50f;
		[SerializeField] private float smoothDuration = 0.5f;
		[SerializeField] private float fadeDuration = 0.3f;
		[SerializeField] private float showDelay = 2f;

		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private ColorStyleRef positiveColor;
		[SerializeField] private ColorStyleRef negativeColor;

		private Vector2 originalPos;
		private Coroutine animationRoutine;

		public event Action<UINotificationView> Showed;

		public void SetText(string value, bool isNegative = false)
		{
			text.text = value;
			ApplyToneColor(isNegative);
			if (backgroundSizer)
			{
				backgroundSizer.Refresh();
			}
		}

		private void ApplyToneColor(bool isNegative)
		{
			if (!backgroundImage)
			{
				Debug.LogError("[UINotificationView] Background image is not assigned.");
				return;
			}

			var style = isNegative ? negativeColor : positiveColor;
			if (string.IsNullOrWhiteSpace(style.Id))
			{
				var tone = isNegative ? "Negative" : "Positive";
				Debug.LogError($"[UINotificationView] {tone} color style is not assigned.");
				return;
			}

			backgroundImage.color = style.Value;
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			if (!canvasGroup || !text)
			{
				Debug.LogError("[UINotificationView] Required references are not assigned.");
				return;
			}

			canvasGroup.alpha = 0f;
			text.gameObject.SetActive(false);
		}

		protected override void OnShow()
		{
			base.OnShow();
			if (!contentRoot || !canvasGroup || !text)
			{
				return;
			}

			originalPos = contentRoot.anchoredPosition;
			contentRoot.anchoredPosition = originalPos + Vector2.down * initialShift;
			canvasGroup.alpha = 0f;
			text.gameObject.SetActive(true);

			if (animationRoutine != null)
			{
				StopCoroutine(animationRoutine);
			}

			animationRoutine = StartCoroutine(PlayAnimation());
		}

		protected override void OnHide()
		{
			if (animationRoutine != null)
			{
				StopCoroutine(animationRoutine);
				animationRoutine = null;
			}

			base.OnHide();
		}

		private System.Collections.IEnumerator PlayAnimation()
		{
			yield return FadeAndMove(contentRoot.anchoredPosition, originalPos, 0f, 1f, smoothDuration);
			yield return new WaitForSeconds(showDelay);
			yield return FadeAndMove(contentRoot.anchoredPosition, new Vector2(originalPos.x + 50f, originalPos.y), 1f, 0f, fadeDuration);

			text.gameObject.SetActive(false);
			Showed?.Invoke(this);
			base.OnHide();
			animationRoutine = null;
		}

		private System.Collections.IEnumerator FadeAndMove(Vector2 fromPos, Vector2 toPos, float fromAlpha, float toAlpha, float duration)
		{
			if (duration <= 0f)
			{
				contentRoot.anchoredPosition = toPos;
				canvasGroup.alpha = toAlpha;
				yield break;
			}

			var elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				var t = Mathf.Clamp01(elapsed / duration);
				contentRoot.anchoredPosition = Vector2.LerpUnclamped(fromPos, toPos, t);
				canvasGroup.alpha = Mathf.LerpUnclamped(fromAlpha, toAlpha, t);
				yield return null;
			}
		}
	}
}
