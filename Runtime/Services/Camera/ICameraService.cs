using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace PlatformCore.Services
{
	public interface ICameraService : ICameraShakeService
	{
		string ActiveCameraId { get; }
		event Action<string> ActiveCameraChanged;
		void SetActiveCamera(string cameraId);
		void SetActiveCamera(string cameraId, CinemachineBlendDefinition transitionBlend);
		UniTask SetActiveCameraAsync(string cameraId, CancellationToken ct = default);
		UniTask SetActiveCameraAsync(string cameraId, CinemachineBlendDefinition transitionBlend, CancellationToken ct = default);
		void AddCamera(string cameraId, CinemachineCamera camera);
		void AttachPrimaryCameraTo(Transform target);
		Transform GetCameraTransform();
		void SetFOV(float fov);
		float GetFOV();
		void SetDutch(float degrees);
		float GetDutch();
	}
}
