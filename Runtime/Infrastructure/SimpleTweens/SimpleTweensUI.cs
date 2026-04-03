using PrimeTween;
using UnityEngine;

namespace PlatformCore.Infrastructure.SimpleTweens
{
	/// <summary>
	/// Reusable UI tween primitives for simple fade and move transitions.
	/// </summary>
	public static class SimpleTweensUI
	{
		/// <summary>
		/// Fades a <see cref="CanvasGroup"/> alpha to the target value.
		/// Useful for common UI show or hide transitions.
		/// This animation is finite.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween UIFade(CanvasGroup canvasGroup, float targetAlpha, float duration = 0.2f, Ease ease = Ease.Linear)
		{
			return Tween.Alpha(canvasGroup, targetAlpha, duration, ease: ease);
		}

		/// <summary>
		/// Moves UI from current anchored position plus offset to the current anchored position while fading alpha from 0 to 1.
		/// Useful for basic enter transitions.
		/// This animation is finite.
		/// Returns a <see cref="Sequence"/> handle.
		/// </summary>
		public static Sequence UIMoveFade(RectTransform rectTransform, CanvasGroup canvasGroup, Vector2 startOffset, float duration = 0.25f)
		{
			return UIMoveFade(rectTransform, canvasGroup, startOffset, duration, 0f, 1f, Ease.OutCubic, Ease.Linear);
		}

		/// <summary>
		/// Moves UI from current anchored position plus offset to the current anchored position while fading from start alpha to target alpha.
		/// Useful when move and fade easings need independent control.
		/// This animation is finite.
		/// Returns a <see cref="Sequence"/> handle.
		/// </summary>
		public static Sequence UIMoveFade(
			RectTransform rectTransform,
			CanvasGroup canvasGroup,
			Vector2 startOffset,
			float duration,
			float startAlpha,
			float targetAlpha,
			Ease moveEase,
			Ease fadeEase)
		{
			var targetPosition = rectTransform.anchoredPosition;
			var startPosition = targetPosition + startOffset;

			rectTransform.anchoredPosition = startPosition;
			canvasGroup.alpha = startAlpha;

			var moveTween = Tween.UIAnchoredPosition(rectTransform, startPosition, targetPosition, duration, ease: moveEase);
			var fadeTween = Tween.Alpha(canvasGroup, startAlpha, targetAlpha, duration, ease: fadeEase);

			return Sequence.Create(moveTween).Group(fadeTween);
		}
	}
}
