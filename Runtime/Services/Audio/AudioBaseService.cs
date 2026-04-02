#if FMOD_PRESENT
﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMOD;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

namespace PlatformCore.Services.Audio
{
	public class AudioBaseService : IAudioService, IService
	{
		private readonly ILoggerService _logger;
		private readonly AudioBaseServiceOptions _options;

		private EventInstance _currentMusic;
		private float _masterVolume = 0.8f;
		private float _musicVolume = 0.5f;
		private float _sfxVolume = 0.5f;
		private bool _isMuted;
		
		private Dictionary<string, EventInstance> _eventInstances = new ();
		private readonly HashSet<string> _prewarmedEvents = new(StringComparer.Ordinal);

		public bool IsMuted => _isMuted;
		public float MasterVolume => _masterVolume;
		public float MusicVolume => _musicVolume;
		public float SfxVolume => _sfxVolume;

		public AudioBaseService(ILoggerService logger, AudioBaseServiceOptions options = null)
		{
			_logger = logger;
			_options = options ?? new AudioBaseServiceOptions();
		}

		public async UniTask PrewarmEventAsync(string eventPath)
		{
			if (string.IsNullOrWhiteSpace(eventPath) || _prewarmedEvents.Contains(eventPath))
			{
				return;
			}

			try
			{
				var eventDescription = RuntimeManager.GetEventDescription(eventPath);
				if (!eventDescription.isValid())
				{
					_logger?.LogError($"[AudioService] Failed to prewarm event, invalid description: {eventPath}");
					return;
				}

				var loadResult = eventDescription.loadSampleData();
				if (loadResult != RESULT.OK)
				{
					_logger?.Log($"[AudioService] Sample prewarm request returned {loadResult} for {eventPath}");
				}

				const int maxFramesToWait = 120;
				for (int frame = 0; frame < maxFramesToWait; frame++)
				{
					var stateResult = eventDescription.getSampleLoadingState(out var loadingState);
					if (stateResult != RESULT.OK)
					{
						_logger?.LogError($"[AudioService] Failed to check sample loading state for {eventPath}: {stateResult}");
						return;
					}

					if (loadingState == LOADING_STATE.LOADED)
					{
						_prewarmedEvents.Add(eventPath);
						return;
					}

					if (loadingState == LOADING_STATE.UNLOADED)
					{
						break;
					}

					await UniTask.Yield();
				}

				_prewarmedEvents.Add(eventPath);
			}
			catch (Exception ex)
			{
				_logger?.LogError($"[AudioService] Failed to prewarm event {eventPath}: {ex.Message}");
			}
		}

		public async UniTask PlayMusicAsync(string eventPath, float fadeTime = 1f)
		{
			_logger?.Log($"[AudioService] Playing music: {eventPath}");

			try
			{
				await PrewarmEventAsync(eventPath);

				if (_currentMusic.isValid())
				{
					_currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					_currentMusic.release();
				}
				
				_currentMusic = RuntimeManager.CreateInstance(eventPath);
				_currentMusic.start();

				if (fadeTime > 0f)
				{
					await UniTask.Delay(TimeSpan.FromSeconds(fadeTime));
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError($"[AudioService] Failed to play music: {ex.Message}");
			}
		}

		public void PlaySoundParallel(string eventPath)
		{
			if (!_eventInstances.TryGetValue(eventPath, out var sound))
			{
				sound = RuntimeManager.CreateInstance(eventPath);
				_eventInstances.Add(eventPath, sound);
			}

			sound.start();
		}

		public void StopParallelSound(string eventPath)
		{
			if (!_eventInstances.TryGetValue(eventPath, out var sound))
			{
				_logger?.Log($"[AudioService] Failed to stop: {eventPath}");
				return;
			}

			sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		public async UniTask StopMusicAsync(float fadeTime = 1f)
		{
			await UniTask.Yield();
			if (!_currentMusic.isValid()) return;

			_logger?.Log("[AudioService] Stopping music");

			try
			{
				_currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

				_currentMusic.release();
				_currentMusic = new EventInstance();
			}
			catch (Exception ex)
			{
				_logger?.LogError($"[AudioService] Failed to stop music: {ex.Message}");
			}
		}

		// ReSharper disable Unity.PerformanceAnalysis
		public void PlaySound(string eventPath)
		{
			try
			{
				RuntimeManager.PlayOneShot(eventPath);
			}
			catch (Exception ex)
			{
				_logger?.LogError($"[AudioService] Failed to play sound {eventPath}: {ex.Message}");
			}
		}

		// ReSharper disable Unity.PerformanceAnalysis
		public void PlaySoundAt(string eventPath, Vector3 position)
		{
			try
			{
				RuntimeManager.PlayOneShot(eventPath, position);
			}
			catch (Exception ex)
			{
				_logger?.LogError($"[AudioService] Failed to play sound at position {eventPath}: {ex.Message}");
			}
		}

		public void SetMasterVolume(float volume)
		{
			ApplyVolumeSettings(volume, _musicVolume, _sfxVolume, _isMuted);
		}

		public void SetMusicVolume(float volume)
		{
			ApplyVolumeSettings(_masterVolume, volume, _sfxVolume, _isMuted);
		}

		public void SetSfxVolume(float volume)
		{
			ApplyVolumeSettings(_masterVolume, _musicVolume, volume, _isMuted);
		}

		public void SetMuted(bool muted)
		{
			ApplyVolumeSettings(_masterVolume, _musicVolume, _sfxVolume, muted);
			_logger?.Log($"[AudioService] Audio {(muted ? "muted" : "unmuted")}");
		}

		public void ApplyVolumeSettings(float masterVolume, float musicVolume, float sfxVolume, bool muted)
		{
			_masterVolume = Mathf.Clamp01(masterVolume);
			_musicVolume = Mathf.Clamp01(musicVolume);
			_sfxVolume = Mathf.Clamp01(sfxVolume);
			_isMuted = muted;
			ApplyVolume();
		}

		private void ApplyVolume()
		{
			try
			{
				float finalVolume = _isMuted ? 0f : _masterVolume;

				var masterBus = RuntimeManager.GetBus(_options.MasterBusPath);
				masterBus.setVolume(finalVolume);

				var musicBus = RuntimeManager.GetBus(_options.MusicBusPath);
				musicBus.setVolume(_musicVolume);

				var sfxBus = RuntimeManager.GetBus(_options.SfxBusPath);
				sfxBus.setVolume(_sfxVolume);
			}
			catch (Exception ex)
			{
				_logger?.LogError($"[AudioService] Failed to apply volume: {ex.Message}");
			}
		}

		public void Dispose()
		{
			if (_currentMusic.isValid())
			{
				_currentMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				_currentMusic.release();
			}

			_logger?.Log("[AudioService] Disposed");
		}
	}
}

#else
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlatformCore.Services.Audio
{
	public class AudioBaseService : IAudioService, IService
	{
		private readonly ILoggerService _logger;
		private float _masterVolume = 0.8f;
		private float _musicVolume = 0.5f;
		private float _sfxVolume = 0.5f;
		private bool _isMuted;

		public bool IsMuted => _isMuted;
		public float MasterVolume => _masterVolume;
		public float MusicVolume => _musicVolume;
		public float SfxVolume => _sfxVolume;

		public AudioBaseService(ILoggerService logger, AudioBaseServiceOptions options = null)
		{
			_logger = logger;
			_logger?.Log("[AudioService] FMOD is not installed. AudioBaseService is running in no-op mode.");
		}

		public UniTask PrewarmEventAsync(string eventPath) => UniTask.CompletedTask;
		public UniTask PlayMusicAsync(string eventPath, float fadeTime = 1f) => UniTask.CompletedTask;
		public UniTask StopMusicAsync(float fadeTime = 1f) => UniTask.CompletedTask;
		public void PlaySoundParallel(string eventPath) { }
		public void StopParallelSound(string eventPath) { }
		public void PlaySound(string eventPath) { }
		public void PlaySoundAt(string eventPath, Vector3 position) { }

		public void SetMasterVolume(float volume)
		{
			ApplyVolumeSettings(volume, _musicVolume, _sfxVolume, _isMuted);
		}

		public void SetMusicVolume(float volume)
		{
			ApplyVolumeSettings(_masterVolume, volume, _sfxVolume, _isMuted);
		}

		public void SetSfxVolume(float volume)
		{
			ApplyVolumeSettings(_masterVolume, _musicVolume, volume, _isMuted);
		}

		public void SetMuted(bool muted)
		{
			ApplyVolumeSettings(_masterVolume, _musicVolume, _sfxVolume, muted);
		}

		public void ApplyVolumeSettings(float masterVolume, float musicVolume, float sfxVolume, bool muted)
		{
			_masterVolume = Mathf.Clamp01(masterVolume);
			_musicVolume = Mathf.Clamp01(musicVolume);
			_sfxVolume = Mathf.Clamp01(sfxVolume);
			_isMuted = muted;
		}

		public void Dispose()
		{
		}
	}
}
#endif
