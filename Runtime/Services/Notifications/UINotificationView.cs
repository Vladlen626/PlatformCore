using System;
using Cysharp.Threading.Tasks;
using PlatformCore.Services.UI;
using PlatformCore.Services.UI.Styles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformCore.Services.Notifications
{
	public class UINotificationView : UIBaseElement
	{
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

		private Vector2 _basePosition;
		private int _animationVersion;

		public async UniTask PlayAsync(string value, bool isNegative = false)
		{
			ValidateReferences();
			text.text = value;
			ApplyToneColor(isNegative);
			backgroundSizer.Refresh();

			Show();
			_group.alpha = 0f;
			_group.interactable = false;
			_group.blocksRaycasts = false;
			text.gameObject.SetActive(true);
			contentRoot.anchoredPosition = _basePosition + Vector2.down * initialShift;

			var version = ++_animationVersion;
			await AnimateAsync(smoothDuration, 0f, 1f, _basePosition + Vector2.down * initialShift, _basePosition, version);
			await UniTask.Delay((int)(Mathf.Max(0f, showDelay) * 1000f), DelayType.UnscaledDeltaTime);
			if (version != _animationVersion)
			{
				return;
			}

			await AnimateAsync(fadeDuration, 1f, 0f, _basePosition, new Vector2(_basePosition.x + initialShift, _basePosition.y), version);
			if (version != _animationVersion)
			{
				return;
			}

			text.gameObject.SetActive(false);
			Hide();
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			ValidateReferences();
			_basePosition = contentRoot.anchoredPosition;
			_group.alpha = 0f;
			text.gameObject.SetActive(false);
		}

		protected override void OnHide()
		{
			_animationVersion++;
			contentRoot.anchoredPosition = _basePosition;
			contentRoot.localScale = Vector3.one;
			text.gameObject.SetActive(false);
			base.OnHide();
		}

		private void ApplyToneColor(bool isNegative)
		{
			backgroundImage.color = (isNegative ? negativeColor : positiveColor).Value;
		}

		private void ValidateReferences()
		{
			if (!contentRoot)
			{
				throw new InvalidOperationException("UINotificationView requires contentRoot reference.");
			}

			if (!backgroundSizer)
			{
				throw new InvalidOperationException("UINotificationView requires backgroundSizer reference.");
			}

			if (!backgroundImage)
			{
				throw new InvalidOperationException("UINotificationView requires backgroundImage reference.");
			}

			if (!text)
			{
				throw new InvalidOperationException("UINotificationView requires text reference.");
			}
		}

		private async UniTask AnimateAsync(float duration, float fromAlpha, float toAlpha, Vector2 fromPos, Vector2 toPos, int version)
		{
			if (duration <= 0f)
			{
				contentRoot.anchoredPosition = toPos;
				_group.alpha = toAlpha;
				return;
			}

			var elapsed = 0f;
			while (elapsed < duration && version == _animationVersion)
			{
				elapsed += Time.unscaledDeltaTime;
				var t = Mathf.Clamp01(elapsed / duration);
				contentRoot.anchoredPosition = Vector2.LerpUnclamped(fromPos, toPos, t);
				_group.alpha = Mathf.LerpUnclamped(fromAlpha, toAlpha, t);
				await UniTask.Yield();
			}
		}
	}
}
