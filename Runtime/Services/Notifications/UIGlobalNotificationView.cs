using System;
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
		private int animationVersion;

		public async UniTask PlayAsync(string message, float holdSeconds, bool isNegative = false)
		{
			ValidateReferences();
			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			messageText.text = message;
			ApplyToneColor(isNegative);
			backgroundSizer.Refresh();
			Show();

			_group.alpha = 0f;
			_group.interactable = false;
			_group.blocksRaycasts = false;

			container.localScale = Vector3.one * scaleIn;
			container.anchoredPosition = baseAnchoredPosition + Vector2.down * slideOffset;

			var version = ++animationVersion;
			await AnimateAsync(fadeInDuration, 0f, 1f, baseAnchoredPosition + Vector2.down * slideOffset, baseAnchoredPosition, scaleIn, popScale, version);
			await AnimateAsync(settleDuration, 1f, 1f, baseAnchoredPosition, baseAnchoredPosition, popScale, 1f, version);
			await UniTask.Delay((int)(Mathf.Max(0.2f, holdSeconds) * 1000f), DelayType.UnscaledDeltaTime);
			if (version != animationVersion)
			{
				return;
			}

			await AnimateAsync(fadeOutDuration, 1f, 0f, baseAnchoredPosition, baseAnchoredPosition + Vector2.up * (slideOffset * 0.4f), 1f, scaleIn, version);
			if (version != animationVersion)
			{
				return;
			}

			ResetContainerTransform();
			Hide();
		}

		public void Interrupt()
		{
			ValidateReferences();
			animationVersion++;
			ResetContainerTransform();
			Hide();
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			ValidateReferences();
			baseAnchoredPosition = container.anchoredPosition;
			_group.interactable = false;
			_group.blocksRaycasts = false;
		}

		protected override void OnHide()
		{
			animationVersion++;
			ResetContainerTransform();
			base.OnHide();
		}

		private void ApplyToneColor(bool isNegative)
		{
			backgroundImage.color = (isNegative ? negativeColor : positiveColor).Value;
		}

		private void ValidateReferences()
		{
			if (!container)
			{
				throw new MissingReferenceException("UIGlobalNotificationView requires container reference.");
			}

			if (!messageText)
			{
				throw new MissingReferenceException("UIGlobalNotificationView requires messageText reference.");
			}

			if (!backgroundSizer)
			{
				throw new MissingReferenceException("UIGlobalNotificationView requires backgroundSizer reference.");
			}

			if (!backgroundImage)
			{
				throw new MissingReferenceException("UIGlobalNotificationView requires backgroundImage reference.");
			}
		}

		private void ResetContainerTransform()
		{
			container.anchoredPosition = baseAnchoredPosition;
			container.localScale = Vector3.one;
		}

		private async UniTask AnimateAsync(float duration, float startAlpha, float endAlpha, Vector2 startPos, Vector2 endPos, float startScale, float endScale, int version)
		{
			if (duration <= 0f)
			{
				_group.alpha = endAlpha;
				container.anchoredPosition = endPos;
				container.localScale = Vector3.one * endScale;
				return;
			}

			var elapsed = 0f;
			while (elapsed < duration && version == animationVersion)
			{
				elapsed += Time.unscaledDeltaTime;
				var t = Mathf.Clamp01(elapsed / duration);
				_group.alpha = Mathf.LerpUnclamped(startAlpha, endAlpha, t);
				container.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, t);
				container.localScale = Vector3.one * Mathf.LerpUnclamped(startScale, endScale, t);
				await UniTask.Yield();
			}
		}
	}
}
