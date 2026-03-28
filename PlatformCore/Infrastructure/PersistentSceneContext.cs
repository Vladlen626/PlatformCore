using PlatformCore.Services;
using PlatformCore.Services.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformCore.Infrastructure
{
	public class PersistentSceneContext : MonoBehaviour, ISceneContext
	{
		[SerializeField] private UIRuntimeContext _uiRuntimeContext;

		public Scene Scene { get; private set; }
		public UIRuntimeContext UIRuntimeContext => _uiRuntimeContext;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
			Scene = gameObject.scene;
		}
	}
}
