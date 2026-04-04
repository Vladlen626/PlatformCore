using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PlatformCore.Editor.Tools.Scenes
{
	[InitializeOnLoad]
	public static class PersistentPlayToggle
	{
		private const string PersistentSceneName = "Persistent";
		private const string MenuPath = "Len/Scenes/Use Persistent";
		private const string PrefKey = "Len.PlatformCore.Scenes.UsePersistent";

		static PersistentPlayToggle()
		{
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			EditorApplication.delayCall += SyncState;
		}

		[MenuItem(MenuPath, false, 0)]
		private static void Toggle()
		{
			var enabled = !EditorPrefs.GetBool(PrefKey, false);
			EditorPrefs.SetBool(PrefKey, enabled);
			Apply(enabled);
		}

		[MenuItem(MenuPath, true)]
		private static bool ToggleValidate()
		{
			Menu.SetChecked(MenuPath, EditorPrefs.GetBool(PrefKey, false));
			return true;
		}

		private static void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state != PlayModeStateChange.ExitingEditMode)
			{
				return;
			}

			Apply(EditorPrefs.GetBool(PrefKey, false));
		}

		private static void SyncState()
		{
			Apply(EditorPrefs.GetBool(PrefKey, false));
		}

		private static void Apply(bool enabled)
		{
			if (!enabled)
			{
				EditorSceneManager.playModeStartScene = null;
				return;
			}

			var persistentScenePath = FindPersistentScenePath();
			if (string.IsNullOrEmpty(persistentScenePath))
			{
				Debug.LogError($"[PersistentPlayToggle] Scene '{PersistentSceneName}' is not found in Build Settings.");
				return;
			}

			var persistentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(persistentScenePath);
			if (!persistentScene)
			{
				Debug.LogError($"[PersistentPlayToggle] Scene asset not found: {persistentScenePath}");
				return;
			}

			EditorSceneManager.playModeStartScene = persistentScene;
		}

		private static string FindPersistentScenePath()
		{
			var scenes = EditorBuildSettings.scenes;
			for (var i = 0; i < scenes.Length; i++)
			{
				var scene = scenes[i];
				if (!scene.enabled)
				{
					continue;
				}

				var sceneName = Path.GetFileNameWithoutExtension(scene.path);
				if (string.Equals(sceneName, PersistentSceneName, StringComparison.OrdinalIgnoreCase))
				{
					return scene.path;
				}
			}

			return null;
		}
	}
}
