using PrimeTween;
using UnityEngine;

namespace PlatformCore.Infrastructure.SimpleTweens
{
	/// <summary>
	/// Reusable transform tween primitives for one-shot movement and impact effects.
	/// </summary>
	public static class SimpleTweensTransform
	{
		/// <summary>
		/// Scales a transform from a reduced scale back to its current local scale.
		/// Useful for quick pop-in effects.
		/// This animation is finite.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween Pop(Transform target, float startScaleMultiplier = 0.8f, float duration = 0.2f, Ease ease = Ease.OutBack)
		{
			var targetScale = target.localScale;
			var startScale = targetScale * startScaleMultiplier;

			target.localScale = startScale;

			return Tween.Scale(target, startScale, targetScale, duration, ease: ease);
		}

		/// <summary>
		/// Moves a transform from current local position plus offset to the current local position.
		/// Useful for simple local-space enter transitions.
		/// This animation is finite.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween MoveFromOffset(Transform target, Vector3 localOffset, float duration = 0.25f, Ease ease = Ease.OutCubic)
		{
			var targetPosition = target.localPosition;
			var startPosition = targetPosition + localOffset;

			target.localPosition = startPosition;

			return Tween.LocalPosition(target, startPosition, targetPosition, duration, ease: ease);
		}

		public static void MoveToLocalPosition(Transform target, Vector3 targetLocalPosition, float duration = 0.25f)
		{
			MoveToLocalPosition(target, targetLocalPosition, duration, Ease.InOutCubic);
		}

		public static void MoveToLocalPosition(Transform target, Vector3 targetLocalPosition, float duration, Ease ease)
		{
			Tween.StopAll(target);
			Tween.LocalPosition(target, target.localPosition, targetLocalPosition, duration, ease: ease);
		}

		public static void MoveToLocalPositionSmoothStop(Transform target, Vector3 targetLocalPosition, float duration = 0.25f)
		{
			MoveToLocalPosition(target, targetLocalPosition, duration, Ease.OutCubic);
		}

		public static void MoveToLocalPositionSmoothStart(Transform target, Vector3 targetLocalPosition, float duration = 0.25f)
		{
			MoveToLocalPosition(target, targetLocalPosition, duration, Ease.InCubic);
		}

		/// <summary>
		/// Plays a short scale punch around the current local scale.
		/// Useful for impact feedback and click responses.
		/// This animation is finite.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween PunchScale(Transform target, Vector3 strength, float duration = 0.2f, float frequency = 20f)
		{
			return Tween.PunchScale(target, strength, duration, frequency: frequency);
		}

		/// <summary>
		/// Plays a short local position punch around the current local position.
		/// Useful for small hit or bump feedback.
		/// This animation is finite.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween PunchPosition(Transform target, Vector3 strength, float duration = 0.2f, float frequency = 20f)
		{
			return Tween.PunchLocalPosition(target, strength, duration, frequency: frequency);
		}

		/// <summary>
		/// Plays a short local position shake around the current local position.
		/// Useful for generic shake feedback without feature-specific logic.
		/// This animation is finite.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween Shake(Transform target, Vector3 strength, float duration = 0.25f, float frequency = 24f)
		{
			return Tween.ShakeLocalPosition(target, strength, duration, frequency: frequency, enableFalloff: true);
		}
	}
}
