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
		public const string MainMenu = "main_menu";
	}

	public static class CameraTransitions
	{
		public static readonly CinemachineBlendDefinition Instant =
			new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);
	}

	public sealed class CameraServiceOptions
	{
		public string PrimaryCameraResourcePath { get; set; } = "CinemachineCamera";
	}

	public class CameraService : BaseAsyncService, ICameraService
	{
		private const string PlayerCamera = "PlayerCamera";
		private readonly IObjectFactory _objectFactory;
		private readonly Transform _cameraParent;
		private readonly CameraServiceOptions _options;

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

		public CameraService(
			IObjectFactory objectFactory,
			Transform cameraParent = null,
			CameraServiceOptions options = null)
		{
			_objectFactory = objectFactory;
			_cameraParent = cameraParent;
			_options = options ?? new CameraServiceOptions();
		}

		protected override async UniTask OnPreInitializeAsync(CancellationToken ct)
		{
			var mainCamera = Camera.main;
			if (mainCamera)
			{
				brain = mainCamera.GetComponent<CinemachineBrain>();
			}

			if (string.IsNullOrWhiteSpace(_options.PrimaryCameraResourcePath))
			{
				Debug.LogError("[CameraService] PrimaryCameraResourcePath is empty.");
				return;
			}

			var camera = await _objectFactory.CreateAsync<CinemachineCamera>(
				_options.PrimaryCameraResourcePath,
				Vector3.zero,
				Quaternion.identity,
				_cameraParent);
			if (!camera)
			{
				Debug.LogError("[CameraService] Failed to create primary camera.");
				return;
			}

			_noise = (CinemachineBasicMultiChannelPerlin)camera.GetCinemachineComponent(CinemachineCore.Stage.Noise);
			camera.name = PlayerCamera;
			allCameras.Add(CameraIds.Primary, camera);

			await SetActiveCameraAsync(CameraIds.Primary, ct);
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

			if (!_camera || !target)
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
			await SetActiveCameraAsyncInternal(cameraId, null, ct);
		}

		public async UniTask SetActiveCameraAsync(
			string cameraId,
			CinemachineBlendDefinition transitionBlend,
			CancellationToken ct = default)
		{
			await SetActiveCameraAsyncInternal(cameraId, transitionBlend, ct);
		}

		public void SetActiveCamera(string cameraId)
		{
			if (!allCameras.TryGetValue(cameraId, out var nextCamera))
			{
				Debug.LogWarning($"Camera {cameraId} not found!");
				return;
			}

			ActivateCamera(nextCamera, cameraId);
		}

		public void SetActiveCamera(string cameraId, CinemachineBlendDefinition transitionBlend)
		{
			SwitchCameraWithBlendAsync(cameraId, transitionBlend).Forget();
		}

		private async UniTask SetActiveCameraAsyncInternal(
			string cameraId,
			CinemachineBlendDefinition? transitionBlend,
			CancellationToken ct)
		{
			if (!allCameras.TryGetValue(cameraId, out var nextCamera))
			{
				Debug.LogWarning($"Camera {cameraId} not found!");
				return;
			}

			if (!brain)
			{
				ActivateCamera(nextCamera, cameraId);
				return;
			}

			if (!transitionBlend.HasValue)
			{
				ActivateCamera(nextCamera, cameraId);
				await UniTask.WaitUntil(() => !brain.IsBlending, cancellationToken: ct);
				return;
			}

			var previousBlend = brain.DefaultBlend;
			try
			{
				brain.DefaultBlend = transitionBlend.Value;
				ActivateCamera(nextCamera, cameraId);

				if (transitionBlend.Value.Style != CinemachineBlendDefinition.Styles.Cut)
				{
					await UniTask.WaitUntil(() => !brain.IsBlending, cancellationToken: ct);
				}
			}
			finally
			{
				brain.DefaultBlend = previousBlend;
			}
		}

		private async UniTask SwitchCameraWithBlendAsync(string cameraId, CinemachineBlendDefinition transitionBlend)
		{
			if (!allCameras.TryGetValue(cameraId, out var nextCamera))
			{
				Debug.LogWarning($"Camera {cameraId} not found!");
				return;
			}

			if (!brain)
			{
				ActivateCamera(nextCamera, cameraId);
				return;
			}

			var previousBlend = brain.DefaultBlend;
			try
			{
				brain.DefaultBlend = transitionBlend;
				ActivateCamera(nextCamera, cameraId);

				if (transitionBlend.Style != CinemachineBlendDefinition.Styles.Cut)
				{
					await UniTask.WaitUntil(() => !brain.IsBlending);
				}
			}
			finally
			{
				brain.DefaultBlend = previousBlend;
			}
		}

		private void ActivateCamera(CinemachineCamera nextCamera, string cameraId)
		{
			if (currentCamera == nextCamera && ActiveCameraId == cameraId)
			{
				return;
			}

			StopShake();

			if (currentCamera)
			{
				currentCamera.gameObject.SetActive(false);
			}

			currentCamera = nextCamera;
			currentCamera.gameObject.SetActive(true);
			ActiveCameraId = cameraId;
			ActiveCameraChanged?.Invoke(cameraId);


			_noise = (CinemachineBasicMultiChannelPerlin)currentCamera.GetCinemachineComponent(CinemachineCore.Stage
				.Noise);
		}

		public void AddCamera(string cameraId, CinemachineCamera camera)
		{
			if (string.IsNullOrWhiteSpace(cameraId) || !camera || allCameras.ContainsKey(cameraId))
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
			if (currentCamera)
			{
				currentCamera.Lens.FieldOfView = fov;
			}
		}

		public float GetFOV()
		{
			if (!currentCamera)
			{
				return 60f;
			}

			return currentCamera.Lens.FieldOfView;
		}

		public void SetDutch(float degrees)
		{
			if (currentCamera)
			{
				currentCamera.Lens.Dutch = degrees;
			}
		}

		public float GetDutch()
		{
			if (!currentCamera)
			{
				return 0f;
			}

			return currentCamera.Lens.Dutch;
		}


		// ReSharper disable Unity.PerformanceAnalysis
		public async UniTask ShakeAsync(float intensity, float duration)
		{
			var noise = _noise;
			if (!noise || IsShaking)
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
			if (!noise)
			{
				return;
			}

			noise.AmplitudeGain = 0;
			noise.FrequencyGain = 0;
		}
	}
}
