using PlatformCore.Infrastructure;
using Unity.Cinemachine;
using UnityEngine;

namespace PlatformCore.Services
{
	[RequireComponent(typeof(CinemachineCamera))]
	public class CinemachineCameraRegister : MonoBehaviour
	{
		[SerializeField]
		private string _cameraId = CameraIds.Primary;

		private void Awake()
		{
			var cinemachineCamera = GetComponent<CinemachineCamera>();
			Locator.Resolve<ICameraService>().AddCamera(_cameraId, cinemachineCamera);
		}
	}
}
