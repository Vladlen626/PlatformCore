using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace PlatformCore.Infrastructure.SimpleTweens
{
	/// <summary>
	/// Reusable motion tween primitives for simple paths and grouped movement.
	/// </summary>
	public static class SimpleTweensMotion
	{
		/// <summary>
		/// Moves a transform in local space to target position along a simple procedural arc.
		/// Useful for lightweight jump-like movement without dedicated path systems.
		/// The arc is calculated as linear interpolation plus a vertical parabola offset: <c>4 * arcHeight * t * (1 - t)</c>.
		/// This animation is finite.
		/// Returns a <see cref="Tween"/> handle.
		/// </summary>
		public static Tween ArcMove(Transform target, Vector3 targetLocalPosition, float arcHeight = 1f, float duration = 0.5f, Ease ease = Ease.InOutSine)
		{
			var startLocalPosition = target.localPosition;

			return Tween.Custom(
				target,
				0f,
				1f,
				duration,
				(currentTarget, progress) =>
				{
					var position = Vector3.LerpUnclamped(startLocalPosition, targetLocalPosition, progress);
					position.y += 4f * arcHeight * progress * (1f - progress);
					currentTarget.localPosition = position;
				},
				ease: ease);
		}

		/// <summary>
		/// Moves a group of transforms in a continuous local-space sine wave with per-item phase shift.
		/// Useful for lightweight crowd, chain, or decorative wave motion.
		/// This animation is a loop.
		/// Returns a single <see cref="Tween"/> handle controlling the whole group.
		/// </summary>
		public static Tween WaveMove(IReadOnlyList<Transform> targets, Vector3 localOffset, float cycleDuration = 1.2f, float phaseShift = 0.35f)
		{
			if (targets is null || targets.Count == 0)
			{
				return default;
			}

			var targetCount = targets.Count;
			var baseLocalPositions = new Vector3[targetCount];
			for (var i = 0; i < targetCount; i++)
			{
				var target = targets[i];
				if (!target)
				{
					continue;
				}

				baseLocalPositions[i] = target.localPosition;
			}

			return Tween.Custom(
				targets,
				0f,
				Mathf.PI * 2f,
				cycleDuration,
				(currentTargets, phase) =>
				{
					var count = Mathf.Min(currentTargets.Count, baseLocalPositions.Length);
					for (var i = 0; i < count; i++)
					{
						var currentTarget = currentTargets[i];
						if (!currentTarget)
						{
							continue;
						}

						var wave = Mathf.Sin(phase + i * phaseShift);
						currentTarget.localPosition = baseLocalPositions[i] + localOffset * wave;
					}
				},
				ease: Ease.Linear,
				cycles: -1,
				cycleMode: CycleMode.Restart);
		}
	}
}
