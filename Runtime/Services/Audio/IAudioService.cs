using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlatformCore.Services.Audio
{
	public interface IAudioService
	{
		UniTask PrewarmEventAsync(string eventPath);
		UniTask PlayMusicAsync(string eventPath, float fadeTime = 0f);
		UniTask StopMusicAsync(float fadeTime = 0f);
		void PlaySoundParallel(string eventPath);
		void StopParallelSound(string eventPath);
		void SetMusicVolume(float volume);

		void PlaySound(string eventPath);
		void PlaySoundAt(string eventPath, Vector3 position);

		void SetMasterVolume(float volume);
		void SetSfxVolume(float volume);
		void SetMuted(bool muted);
		void ApplyVolumeSettings(float masterVolume, float musicVolume, float sfxVolume, bool muted);

		bool IsMuted { get; }

		float MasterVolume { get; }
		float MusicVolume { get; }
		float SfxVolume { get; }
	}
}
