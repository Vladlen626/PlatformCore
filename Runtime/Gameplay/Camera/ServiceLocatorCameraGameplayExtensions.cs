using PlatformCore.Infrastructure;
using PlatformCore.Services;
using UnityEngine;

namespace PlatformCore.Gameplay.Camera
{
	public static class ServiceLocatorCameraGameplayExtensions
	{
		public static FirstPersonCameraController CreateFirstPersonCameraController(this ServiceLocator serviceLocator,
			string cameraId = CameraIds.Primary)
		{
			var cameraService = serviceLocator.Get<ICameraService>();
			return new FirstPersonCameraController(cameraService, cameraId);
		}

		public static ThirdPersonCameraController CreateThirdPersonCameraController(this ServiceLocator serviceLocator,
			Vector3 followOffset, string cameraId = CameraIds.Primary)
		{
			var cameraService = serviceLocator.Get<ICameraService>();
			return new ThirdPersonCameraController(cameraService, followOffset, cameraId);
		}
	}
}
