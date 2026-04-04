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
		bool IsJumpPressedThisFrame { get; }
		bool IsPausePressedThisFrame { get; }
		bool IsUISubmitPressedThisFrame { get; }
		bool IsUICancelPressedThisFrame { get; }
	}
}
