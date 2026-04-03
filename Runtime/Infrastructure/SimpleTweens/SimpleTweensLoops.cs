using PrimeTween;
using UnityEngine;

namespace PlatformCore.Infrastructure.SimpleTweens
{
	/// <summary>
	/// Reusable loop tween primitives for continuous idle motion.
	/// </summary>
	public static class SimpleTweensLoops
	{
		/// <summary>
		/// Rotates a transform forever around a local axis.
		/// Useful for continuous spinning objects.
		/// This animation is a loop.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween SpinLoop(Transform target, Vector3 localAxis, float degreesPerCycle = 360f, float duration = 1.2f)
		{
			var startEuler = target.localEulerAngles;
			var endEuler = startEuler + localAxis.normalized * degreesPerCycle;

			return Tween.LocalRotation(target, startEuler, endEuler, duration, ease: Ease.Linear, cycles: -1, cycleMode: CycleMode.Incremental);
		}

		/// <summary>
		/// Moves a transform back and forth forever between current local position and local position plus offset.
		/// Useful for floating and hovering idle motion.
		/// This animation is a loop.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween HoverLoop(Transform target, Vector3 localOffset, float halfCycleDuration = 0.9f, Ease ease = Ease.InOutSine)
		{
			var startPosition = target.localPosition;
			var endPosition = startPosition + localOffset;

			return Tween.LocalPosition(target, startPosition, endPosition, halfCycleDuration, ease: ease, cycles: -1, cycleMode: CycleMode.Yoyo);
		}

		/// <summary>
		/// Scales a transform back and forth forever between current local scale and multiplied local scale.
		/// Useful for soft breathing or pulsing idle motion.
		/// This animation is a loop.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween BreathLoop(Transform target, float scaleMultiplier = 1.06f, float halfCycleDuration = 0.9f, Ease ease = Ease.InOutSine)
		{
			var startScale = target.localScale;
			var endScale = startScale * scaleMultiplier;

			return Tween.Scale(target, startScale, endScale, halfCycleDuration, ease: ease, cycles: -1, cycleMode: CycleMode.Yoyo);
		}

		/// <summary>
		/// Rotates a transform back and forth forever between current local rotation and local rotation plus euler offset.
		/// Useful for subtle wobble idle motion.
		/// This animation is a loop.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween RotateWobble(Transform target, Vector3 localEulerOffset, float halfCycleDuration = 0.6f, Ease ease = Ease.InOutSine)
		{
			var startEuler = target.localEulerAngles;
			var endEuler = startEuler + localEulerOffset;

			return Tween.LocalRotation(target, startEuler, endEuler, halfCycleDuration, ease: ease, cycles: -1, cycleMode: CycleMode.Yoyo);
		}
	}
}
