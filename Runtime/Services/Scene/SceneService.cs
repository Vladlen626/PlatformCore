using System.Threading;
using Cysharp.Threading.Tasks;
using PlatformCore.Infrastructure;
using UnityEngine.SceneManagement;

namespace PlatformCore.Services
{
	public class SceneService : BaseAsyncService, ISceneService
	{
		private readonly ILoggerService _loggerService;
		private readonly PersistentSceneContext _persistentSceneContext;

		public SceneService(ILoggerService loggerService, PersistentSceneContext persistentSceneContext)
		{
			_loggerService = loggerService;
			_persistentSceneContext = persistentSceneContext;
		}

		public string PersistentSceneName
		{
			get
			{
				if (!_persistentSceneContext)
				{
					return string.Empty;
				}

				return _persistentSceneContext.Scene.name;
			}
		}

		public ISceneContext PersistentContext => _persistentSceneContext;

		public async UniTask LoadSceneAsync(string sceneName, CancellationToken ct = default)
		{
			if (IsSceneLoaded(sceneName))
			{
				_loggerService?.Log($"[SceneService] Scene already loaded: {sceneName}");
				return;
			}

			_loggerService?.Log($"[SceneService] Loading scene: {sceneName}");
			var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			await operation.ToUniTask(cancellationToken: ct);
			_loggerService?.Log($"[SceneService] Scene loaded successfully: {sceneName}");
		}

		public async UniTask LoadAndSetActiveSceneAsync(string sceneName, CancellationToken ct = default)
		{
			await LoadSceneAsync(sceneName, ct);
			TrySetActiveScene(sceneName);
		}

		public bool TrySetActiveScene(string sceneName)
		{
			if (!IsSceneLoaded(sceneName))
			{
				_loggerService?.LogWarning($"[SceneService] Cannot set active scene, scene is not loaded: {sceneName}");
				return false;
			}

			var scene = SceneManager.GetSceneByName(sceneName);
			var result = SceneManager.SetActiveScene(scene);
			if (result)
			{
				_loggerService?.Log($"[SceneService] Active scene set: {sceneName}");
			}
			else
			{
				_loggerService?.LogWarning($"[SceneService] Failed to set active scene: {sceneName}");
			}

			return result;
		}

		public async UniTask UnloadSceneAsync(string sceneName, CancellationToken ct = default)
		{
			if (!IsSceneLoaded(sceneName))
			{
				_loggerService?.LogWarning($"[SceneService] Scene not loaded, cannot unload: {sceneName}");
				return;
			}

			if (sceneName == PersistentSceneName)
			{
				_loggerService?.LogWarning($"[SceneService] Persistent scene cannot be unloaded: {sceneName}");
				return;
			}

			var activeSceneName = GetActiveSceneName();
			if (activeSceneName == sceneName)
			{
				TrySetActiveScene(PersistentSceneName);
			}

			_loggerService?.Log($"[SceneService] Unloading scene: {sceneName}");
			var operation = SceneManager.UnloadSceneAsync(sceneName);
			await operation.ToUniTask(cancellationToken: ct);
			_loggerService?.Log($"[SceneService] Scene unloaded successfully: {sceneName}");
		}

		public string GetActiveSceneName()
		{
			return SceneManager.GetActiveScene().name;
		}

		public bool TryGetSceneContext(string sceneName, out ISceneContext sceneContext)
		{
			sceneContext = null;

			if (sceneName == PersistentSceneName)
			{
				sceneContext = _persistentSceneContext;
				return sceneContext != null;
			}

			var scene = SceneManager.GetSceneByName(sceneName);
			if (!scene.IsValid() || !scene.isLoaded)
			{
				return false;
			}

			var roots = scene.GetRootGameObjects();
			for (var i = 0; i < roots.Length; i++)
			{
				if (roots[i].TryGetComponent<ISceneContext>(out var ctx))
				{
					sceneContext = ctx;
					return true;
				}

				sceneContext = roots[i].GetComponentInChildren<ISceneContext>(true);
				if (sceneContext != null)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsSceneLoaded(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				return false;
			}

			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);
				if (scene.name == sceneName && scene.isLoaded)
				{
					return true;
				}
			}

			return false;
		}
	}
}
