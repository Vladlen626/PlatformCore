using System;
using DG.Tweening;
using PlatformCore.Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Main.Scripts.UI;

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
	private Sequence fullSequence;

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
			throw new InvalidOperationException("Notification background image is not assigned.");
		}

		var style = isNegative ? negativeColor : positiveColor;
		if (string.IsNullOrWhiteSpace(style.Id))
		{
			var tone = isNegative ? "Negative" : "Positive";
			throw new InvalidOperationException($"{tone} notification color style is not assigned.");
		}

		backgroundImage.color = style.Value;
	}

	protected override void OnAwake()
	{
		base.OnAwake();
		canvasGroup.alpha = 0f;
		text.gameObject.SetActive(false);
	}

	protected override void OnShow()
	{
		base.OnShow();

		originalPos = contentRoot.anchoredPosition;
		contentRoot.anchoredPosition = originalPos + Vector2.down * initialShift;
		canvasGroup.alpha = 0f;
		text.gameObject.SetActive(true);

		fullSequence?.Kill();
		fullSequence = DOTween.Sequence();
		fullSequence.Append(contentRoot.DOAnchorPos(originalPos, smoothDuration).SetEase(Ease.InOutQuad));
		fullSequence.Join(canvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad));

		fullSequence.AppendInterval(showDelay);
		fullSequence.Append(contentRoot.DOAnchorPosX(originalPos.x + 50f, fadeDuration).SetEase(Ease.InQuad));
		fullSequence.Join(canvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.InQuad));

		fullSequence.OnComplete(() =>
		{
			text.gameObject.SetActive(false);
			Showed?.Invoke(this);
			base.OnHide();
		});
	}

	protected override void OnHide()
	{
		fullSequence?.Kill();
		base.OnHide();
	}
}
