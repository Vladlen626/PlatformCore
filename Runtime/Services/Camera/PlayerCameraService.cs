using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlatformCore.Services.Factory;
using Unity.Cinemachine;
using UnityEngine;

namespace PlatformCore.Services
{
	public static class CameraIds
	{
		public const string Primary = "primary";
	}

	public class CameraService : BaseAsyncService, ICameraService
	{
		private const string PlayerCamera = "PlayerCamera";
		private readonly IObjectFactory _objectFactory;
		private readonly Transform _cameraParent;

		private CinemachineBasicMultiChannelPerlin _noise;
		private CinemachineBasicMultiChannelPerlin _activeShakeNoise;
		private CancellationTokenSource _shakeCts;
		public bool IsShaking { get; private set; }

		private CinemachineCamera currentCamera;
		private CinemachineBrain brain;
		public string ActiveCameraId { get; private set; }
		public event Action<string> ActiveCameraChanged;

		private readonly Dictionary<string, CinemachineCamera> allCameras =
			new Dictionary<string, CinemachineCamera>();

		public CameraService(IObjectFactory objectFactory, Transform cameraParent = null)
		{
			_objectFactory = objectFactory;
			_cameraParent = cameraParent;
		}

		protected override async UniTask OnPreInitializeAsync(CancellationToken ct)
		{
			brain = Camera.main.GetComponent<CinemachineBrain>();

			var _camera = await _objectFactory.CreateAsync<CinemachineCamera>(ResourcePaths.Sample.Player.CinemachineCamera,
				Vector3.zero, Quaternion.identity, _cameraParent);
			_noise = (CinemachineBasicMultiChannelPerlin)_camera.GetCinemachineComponent(CinemachineCore.Stage.Noise);
			_camera.name = PlayerCamera;
			allCameras.Add(CameraIds.Primary, _camera);

			SetActiveCamera(CameraIds.Primary);
		}

		public override void Dispose()
		{
			StopShake();

			foreach (var cam in allCameras.Values)
			{
				if (cam)
				{
					_objectFactory.Destroy(cam.gameObject);
				}
			}

			allCameras.Clear();
			currentCamera = null;
		}

		public void AttachPrimaryCameraTo(Transform target)
		{
			if (!allCameras.TryGetValue(CameraIds.Primary, out var _camera))
			{
				return;
			}

			if (_camera == null || target == null)
			{
				return;
			}

			_camera.transform.SetParent(target);
			_camera.transform.localPosition = Vector3.zero;
			_camera.transform.localRotation = Quaternion.identity;

			_camera.Follow = null;
			_camera.LookAt = null;
		}
		
		public async UniTask SetActiveCameraAsync(string cameraId, CancellationToken ct = default)
		{
			if (allCameras == null || !allCameras.ContainsKey(cameraId))
			{
				Debug.LogWarning($"Camera {cameraId} not found!");
				return;
			}
			
			if (brain == null)
			{
				Debug.LogWarning("No CinemachineBrain found on main camera!");
				SetActiveCamera(cameraId);
				return;
			}

			SetActiveCamera(cameraId);

			await UniTask.WaitUntil(() => !brain.IsBlending, cancellationToken: ct);
		}

		public void SetActiveCamera(string cameraId)
		{
			if (allCameras == null || !allCameras.ContainsKey(cameraId))
			{
				Debug.LogWarning($"Camera {cameraId} not found!");
				return;
			}

			if (currentCamera == allCameras[cameraId] && ActiveCameraId == cameraId)
			{
				return;
			}

			StopShake();

			if (currentCamera != null)
			{
				currentCamera.gameObject.SetActive(false);
			}

			currentCamera = allCameras[cameraId];
			currentCamera.gameObject.SetActive(true);
			ActiveCameraId = cameraId;
			ActiveCameraChanged?.Invoke(cameraId);


			_noise = (CinemachineBasicMultiChannelPerlin)currentCamera.GetCinemachineComponent(CinemachineCore.Stage
				.Noise);
		}

		public void AddCamera(string cameraId, CinemachineCamera camera)
		{
			if (string.IsNullOrWhiteSpace(cameraId) || camera == null || allCameras.ContainsKey(cameraId))
			{
				return;
			}

			camera.gameObject.SetActive(false);
			allCameras.Add(cameraId, camera);
		}

		public Transform GetCameraTransform()
		{
			return currentCamera?.transform;
		}


		public void SetFOV(float fov)
		{
			if (currentCamera != null)
			{
				currentCamera.Lens.FieldOfView = fov;
			}
		}

		public float GetFOV()
		{
			return currentCamera != null ? currentCamera.Lens.FieldOfView : 60f;
		}

		public void SetDutch(float degrees)
		{
			if (currentCamera != null)
				currentCamera.Lens.Dutch = degrees;
		}

		public float GetDutch()
		{
			return currentCamera != null ? currentCamera.Lens.Dutch : 0f;
		}


		// ReSharper disable Unity.PerformanceAnalysis
		public async UniTask ShakeAsync(float intensity, float duration)
		{
			var noise = _noise;
			if (noise == null || IsShaking)
			{
				return;
			}

			IsShaking = true;
			_shakeCts?.Cancel();
			_shakeCts?.Dispose();
			_shakeCts = new CancellationTokenSource();
			var shakeCts = _shakeCts;
			_activeShakeNoise = noise;

			noise.AmplitudeGain = intensity;
			noise.FrequencyGain = intensity * 1.5f;

			try
			{
				await UniTask.WaitForSeconds(duration, cancellationToken: shakeCts.Token);
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				ResetNoise(noise);

				if (ReferenceEquals(_shakeCts, shakeCts))
				{
					_shakeCts.Dispose();
					_shakeCts = null;
					_activeShakeNoise = null;
					IsShaking = false;
				}
			}
		}

		public void StopShake()
		{
			_shakeCts?.Cancel();
			_shakeCts?.Dispose();
			_shakeCts = null;

			ResetNoise(_activeShakeNoise);

			if (!ReferenceEquals(_activeShakeNoise, _noise))
			{
				ResetNoise(_noise);
			}

			_activeShakeNoise = null;
			IsShaking = false;
		}

		private static void ResetNoise(CinemachineBasicMultiChannelPerlin noise)
		{
			if (noise == null)
			{
				return;
			}

			noise.AmplitudeGain = 0;
			noise.FrequencyGain = 0;
		}
	}
}
