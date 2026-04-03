using Cysharp.Threading.Tasks;
using PlatformCore.Services.UI;
using PlatformCore.Services.UI.Styles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformCore.Services.Notifications
{
	public class UIGlobalNotificationView : UIBaseElement
	{
		[SerializeField] private RectTransform container;
		[SerializeField] private TextMeshProUGUI messageText;
		[SerializeField] private UIBackgroundSizer backgroundSizer;
		[SerializeField] private Image backgroundImage;
		[SerializeField] private ColorStyleRef positiveColor;
		[SerializeField] private ColorStyleRef negativeColor;
		[SerializeField] private float fadeInDuration = 0.18f;
		[SerializeField] private float fadeOutDuration = 0.18f;
		[SerializeField] private float scaleIn = 0.96f;
		[SerializeField] private float popScale = 1.04f;
		[SerializeField] private float settleDuration = 0.08f;
		[SerializeField] private float slideOffset = 18f;

		private Vector2 baseAnchoredPosition;
		private bool hasBaseAnchoredPosition;
		private int animationVersion;

		public async UniTask PlayAsync(string message, float holdSeconds, bool isNegative = false)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			if (!messageText || !_group)
			{
				return;
			}

			messageText.text = message;
			ApplyToneColor(isNegative);
			if (backgroundSizer)
			{
				backgroundSizer.Refresh();
			}
			Show();

			_group.alpha = 0f;
			_group.interactable = false;
			_group.blocksRaycasts = false;

			var rect = ResolveContainerRect();
			CacheBaseAnchoredPosition(rect);
			if (rect)
			{
				rect.localScale = Vector3.one * scaleIn;
				rect.anchoredPosition = baseAnchoredPosition + Vector2.down * slideOffset;
			}

			var version = ++animationVersion;
			await AnimateAsync(rect, fadeInDuration, 0f, 1f, baseAnchoredPosition + Vector2.down * slideOffset, baseAnchoredPosition, scaleIn, popScale, version);
			await AnimateAsync(rect, settleDuration, 1f, 1f, baseAnchoredPosition, baseAnchoredPosition, popScale, 1f, version);
			await UniTask.Delay((int)(Mathf.Max(0.2f, holdSeconds) * 1000f));
			if (version != animationVersion)
			{
				return;
			}

			await AnimateAsync(rect, fadeOutDuration, 1f, 0f, baseAnchoredPosition, baseAnchoredPosition + Vector2.up * (slideOffset * 0.4f), 1f, scaleIn, version);
			if (version != animationVersion)
			{
				return;
			}

			ResetContainerTransform();
			Hide();
		}

		private void ApplyToneColor(bool isNegative)
		{
			if (!backgroundImage)
			{
				Debug.LogError("[UIGlobalNotificationView] Background image is not assigned.");
				return;
			}

			var style = isNegative ? negativeColor : positiveColor;
			if (string.IsNullOrWhiteSpace(style.Id))
			{
				var tone = isNegative ? "Negative" : "Positive";
				Debug.LogError($"[UIGlobalNotificationView] {tone} color style is not assigned.");
				return;
			}

			backgroundImage.color = style.Value;
		}

		public void Interrupt()
		{
			animationVersion++;
			ResetContainerTransform();
			Hide();
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			CacheBaseAnchoredPosition(ResolveContainerRect());
			if (_group)
			{
				_group.interactable = false;
				_group.blocksRaycasts = false;
			}
		}

		protected override void OnHide()
		{
			animationVersion++;
			ResetContainerTransform();
			base.OnHide();
		}

		private RectTransform ResolveContainerRect()
		{
			return container ? container : GetComponent<RectTransform>();
		}

		private void CacheBaseAnchoredPosition(RectTransform rect)
		{
			if (!rect || hasBaseAnchoredPosition)
			{
				return;
			}

			baseAnchoredPosition = rect.anchoredPosition;
			hasBaseAnchoredPosition = true;
		}

		private void ResetContainerTransform()
		{
			var rect = ResolveContainerRect();
			CacheBaseAnchoredPosition(rect);
			if (!rect)
			{
				return;
			}

			rect.anchoredPosition = baseAnchoredPosition;
			rect.localScale = Vector3.one;
		}

		private async UniTask AnimateAsync(RectTransform rect, float duration, float startAlpha, float endAlpha, Vector2 startPos, Vector2 endPos, float startScale, float endScale, int version)
		{
			if (duration <= 0f)
			{
				_group.alpha = endAlpha;
				if (rect)
				{
					rect.anchoredPosition = endPos;
					rect.localScale = Vector3.one * endScale;
				}
				return;
			}

			var elapsed = 0f;
			while (elapsed < duration && version == animationVersion)
			{
				elapsed += Time.unscaledDeltaTime;
				var t = Mathf.Clamp01(elapsed / duration);
				_group.alpha = Mathf.LerpUnclamped(startAlpha, endAlpha, t);
				if (rect)
				{
					rect.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, t);
					rect.localScale = Vector3.one * Mathf.LerpUnclamped(startScale, endScale, t);
				}
				await UniTask.Yield();
			}
		}
	}
}
