using System;
using PlatformCore.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlatformCore.Services.Input
{
	public sealed class InputBaseService : IInputService, ISyncInitializable
	{
		public event Action OnJumpPressed;
		public event Action OnJumpReleased;
		public event Action OnPausePressed;
		public event Action OnUISubmitPressed;
		public event Action OnUICancelPressed;
		public event Action<Vector2> OnMoved;
		public event Action<Vector2> OnLooked;

		private InputActionMap _playerMap;
		private InputActionMap _uiMap;

		private InputAction _moveAction;
		private InputAction _lookAction;
		private InputAction _jumpAction;
		private InputAction _pauseAction;
		private InputAction _uiSubmitAction;
		private InputAction _uiCancelAction;

		public Vector2 Move => _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
		public Vector2 Look => _lookAction != null ? _lookAction.ReadValue<Vector2>() : Vector2.zero;
		public bool IsJumping => _jumpAction != null && _jumpAction.IsPressed();
		public bool IsJumpPressedThisFrame => _jumpAction != null && _jumpAction.WasPressedThisFrame();
		public bool IsPausePressedThisFrame => _pauseAction != null && _pauseAction.WasPressedThisFrame();
		public bool IsUISubmitPressedThisFrame => _uiSubmitAction != null && _uiSubmitAction.WasPressedThisFrame();
		public bool IsUICancelPressedThisFrame => _uiCancelAction != null && _uiCancelAction.WasPressedThisFrame();

		public void Initialize()
		{
			_playerMap = new InputActionMap("Player");
			_uiMap = new InputActionMap("UI");

			_moveAction = _playerMap.AddAction("Move", InputActionType.Value);
			_moveAction.AddCompositeBinding("2DVector")
				.With("Up", "<Keyboard>/w")
				.With("Down", "<Keyboard>/s")
				.With("Left", "<Keyboard>/a")
				.With("Right", "<Keyboard>/d");

			_lookAction = _playerMap.AddAction("Look", InputActionType.Value, "<Mouse>/delta");
			_jumpAction = _playerMap.AddAction("Jump", InputActionType.Button, "<Keyboard>/space");
			_pauseAction = _playerMap.AddAction("Pause", InputActionType.Button, "<Keyboard>/escape");

			_uiSubmitAction = _uiMap.AddAction("Submit", InputActionType.Button);
			_uiSubmitAction.AddBinding("<Keyboard>/enter");
			_uiSubmitAction.AddBinding("<Keyboard>/numpadEnter");

			_uiCancelAction = _uiMap.AddAction("Cancel", InputActionType.Button, "<Keyboard>/escape");

			_moveAction.performed += OnMovePerformedHandler;
			_moveAction.canceled += OnMoveCanceledHandler;

			_lookAction.performed += OnLookPerformedHandler;
			_lookAction.canceled += OnLookCanceledHandler;

			_jumpAction.performed += OnJumpPerformedHandler;
			_jumpAction.canceled += OnJumpCanceledHandler;

			_pauseAction.performed += OnPausePerformedHandler;
			_uiSubmitAction.performed += OnUISubmitPerformedHandler;
			_uiCancelAction.performed += OnUICancelPerformedHandler;

			_playerMap.Enable();
			_uiMap.Enable();
		}

		public void Dispose()
		{
			if (_moveAction != null)
			{
				_moveAction.performed -= OnMovePerformedHandler;
				_moveAction.canceled -= OnMoveCanceledHandler;
			}

			if (_lookAction != null)
			{
				_lookAction.performed -= OnLookPerformedHandler;
				_lookAction.canceled -= OnLookCanceledHandler;
			}

			if (_jumpAction != null)
			{
				_jumpAction.performed -= OnJumpPerformedHandler;
				_jumpAction.canceled -= OnJumpCanceledHandler;
			}

			if (_pauseAction != null)
			{
				_pauseAction.performed -= OnPausePerformedHandler;
			}

			if (_uiSubmitAction != null)
			{
				_uiSubmitAction.performed -= OnUISubmitPerformedHandler;
			}

			if (_uiCancelAction != null)
			{
				_uiCancelAction.performed -= OnUICancelPerformedHandler;
			}

			if (_playerMap != null)
			{
				_playerMap.Disable();
				_playerMap.Dispose();
				_playerMap = null;
			}

			if (_uiMap != null)
			{
				_uiMap.Disable();
				_uiMap.Dispose();
				_uiMap = null;
			}

			_moveAction = null;
			_lookAction = null;
			_jumpAction = null;
			_pauseAction = null;
			_uiSubmitAction = null;
			_uiCancelAction = null;
		}

		private void OnMovePerformedHandler(InputAction.CallbackContext context)
		{
			OnMoved?.Invoke(context.ReadValue<Vector2>());
		}

		private void OnMoveCanceledHandler(InputAction.CallbackContext context)
		{
			OnMoved?.Invoke(Vector2.zero);
		}

		private void OnLookPerformedHandler(InputAction.CallbackContext context)
		{
			OnLooked?.Invoke(context.ReadValue<Vector2>());
		}

		private void OnLookCanceledHandler(InputAction.CallbackContext context)
		{
			OnLooked?.Invoke(Vector2.zero);
		}

		private void OnJumpPerformedHandler(InputAction.CallbackContext context)
		{
			OnJumpPressed?.Invoke();
		}

		private void OnJumpCanceledHandler(InputAction.CallbackContext context)
		{
			OnJumpReleased?.Invoke();
		}

		private void OnPausePerformedHandler(InputAction.CallbackContext context)
		{
			OnPausePressed?.Invoke();
		}

		private void OnUISubmitPerformedHandler(InputAction.CallbackContext context)
		{
			OnUISubmitPressed?.Invoke();
		}

		private void OnUICancelPerformedHandler(InputAction.CallbackContext context)
		{
			OnUICancelPressed?.Invoke();
		}
	}
}
