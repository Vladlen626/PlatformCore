using PlatformCore.Services;
using PlatformCore.Services.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformCore.Infrastructure
{
	public class PersistentSceneContext : MonoBehaviour, ISceneContext
	{
		[SerializeField] private UICanvasEntry[] _uiCanvases;

		public Scene Scene { get; private set; }
		public UICanvasEntry[] UICanvases => _uiCanvases;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
			Scene = gameObject.scene;
		}
	}
}
