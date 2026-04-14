using System;
using PlatformCore.Core;
using UnityEngine;

namespace PlatformCore.Services.Input
{
	public interface IInputService : IService
	{
		event Action OnJumpPressed;
		event Action OnJumpReleased;
		event Action OnPausePressed;
		event Action OnUISubmitPressed;
		event Action OnUICancelPressed;
		event Action<Vector2> OnMoved;
		event Action<Vector2> OnLooked;

		Vector2 Move { get; }
		Vector2 Look { get; }
		bool IsJumping { get; }
		bool IsSprintPressed { get; }
		bool IsJumpPressedThisFrame { get; }
		bool IsInteractPressedThisFrame { get; }
		bool IsPrimaryActionPressedThisFrame { get; }
		bool IsSecondaryActionPressedThisFrame { get; }
		bool IsPausePressedThisFrame { get; }
		bool IsUISubmitPressedThisFrame { get; }
		bool IsUICancelPressedThisFrame { get; }
	}
}
