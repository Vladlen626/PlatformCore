using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;
using PlatformCore.Services;
using Unity.Cinemachine;
using UnityEngine;

namespace PlatformCore.Gameplay.Camera
{
	public sealed class FirstPersonCameraController : IBaseController, IActivatable, IDeactivatable
	{
		private readonly ICameraService _cameraService;
		private readonly string _cameraId;
		private Transform _anchor;
		private bool _isActive;

		public FirstPersonCameraController(ICameraService cameraService, string cameraId = CameraIds.Primary)
		{
			_cameraService = cameraService;
			_cameraId = cameraId;
		}

		public void Bind(Transform anchor)
		{
			_anchor = anchor;

			if (_isActive)
			{
				Apply();
			}
		}

		public void Unbind()
		{
			if (_isActive)
			{
				Detach();
			}

			_anchor = null;
		}

		public void Activate()
		{
			if (_isActive)
			{
				return;
			}

			_isActive = true;
			Apply();
		}

		public void Deactivate()
		{
			if (_isActive == false)
			{
				return;
			}

			Detach();
			_isActive = false;
		}

		private void Apply()
		{
			if (!_anchor)
			{
				return;
			}

			_cameraService.SetActiveCamera(_cameraId);
			var cameraTransform = _cameraService.GetCameraTransform();
			if (!cameraTransform)
			{
				return;
			}

			cameraTransform.SetParent(_anchor);
			cameraTransform.localPosition = Vector3.zero;
			cameraTransform.localRotation = Quaternion.identity;

			var cinemachineCamera = cameraTransform.GetComponent<CinemachineCamera>();
			if (cinemachineCamera)
			{
				cinemachineCamera.Follow = null;
				cinemachineCamera.LookAt = null;
			}
		}

		private void Detach()
		{
			var cameraTransform = _cameraService.GetCameraTransform();
			if (!cameraTransform)
			{
				return;
			}

			cameraTransform.SetParent(null);
		}
	}
}
