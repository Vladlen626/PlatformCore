using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;
using PlatformCore.Services;
using Unity.Cinemachine;
using UnityEngine;

namespace PlatformCore.Gameplay.Camera
{
	public sealed class ThirdPersonCameraController : IBaseController, IActivatable, IDeactivatable
	{
		private readonly ICameraService _cameraService;
		private readonly string _cameraId;
		private readonly Vector3 _followOffset;
		private Transform _followTarget;
		private Transform _lookAtTarget;
		private bool _isActive;

		public ThirdPersonCameraController(ICameraService cameraService, Vector3 followOffset, string cameraId = CameraIds.Primary)
		{
			_cameraService = cameraService;
			_cameraId = cameraId;
			_followOffset = followOffset;
		}

		public void Bind(Transform followTarget, Transform lookAtTarget = null)
		{
			_followTarget = followTarget;
			_lookAtTarget = lookAtTarget;

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

			_followTarget = null;
			_lookAtTarget = null;
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
			if (!_followTarget)
			{
				return;
			}

			_cameraService.SetActiveCamera(_cameraId);
			var cameraTransform = _cameraService.GetCameraTransform();
			if (!cameraTransform)
			{
				return;
			}

			cameraTransform.SetParent(null);
			cameraTransform.position = _followTarget.position + _followOffset;

			var cinemachineCamera = cameraTransform.GetComponent<CinemachineCamera>();
			if (!cinemachineCamera)
			{
				return;
			}

			cinemachineCamera.Follow = _followTarget;
			cinemachineCamera.LookAt = _lookAtTarget ? _lookAtTarget : _followTarget;
		}

		private void Detach()
		{
			var cameraTransform = _cameraService.GetCameraTransform();
			if (!cameraTransform)
			{
				return;
			}

			var cinemachineCamera = cameraTransform.GetComponent<CinemachineCamera>();
			if (cinemachineCamera)
			{
				cinemachineCamera.Follow = null;
				cinemachineCamera.LookAt = null;
			}
		}
	}
}
