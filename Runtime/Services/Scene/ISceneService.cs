using System.Threading;
using Cysharp.Threading.Tasks;

namespace PlatformCore.Services
{
	public interface ISceneService
	{
		string PersistentSceneName { get; }
		ISceneContext PersistentContext { get; }

		UniTask LoadSceneAsync(string sceneName, CancellationToken ct = default);
		UniTask LoadAndSetActiveSceneAsync(string sceneName, CancellationToken ct = default);
		UniTask UnloadSceneAsync(string sceneName, CancellationToken ct = default);

		bool TrySetActiveScene(string sceneName);
		string GetActiveSceneName();
		bool IsSceneLoaded(string sceneName);
		bool TryGetSceneContext(string sceneName, out ISceneContext sceneContext);
	}
}
