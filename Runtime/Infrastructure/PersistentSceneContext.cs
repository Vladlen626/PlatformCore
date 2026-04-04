using System;
using System.Collections.Generic;
using PlatformCore.Services;
using PlatformCore.Services.UI;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace PlatformCore.Infrastructure
{
	public class PersistentSceneContext : MonoBehaviour, ISceneContext
	{
		[SerializeField] private UIRuntimeContext _uiRuntimeContext;

		public UnityScene Scene { get; private set; }
		public IReadOnlyList<UICanvasEntry> UICanvasEntries
		{
			get
			{
				if (!_uiRuntimeContext)
				{
					return Array.Empty<UICanvasEntry>();
				}

				return _uiRuntimeContext.CanvasEntries;
			}
		}

		private void Awake()
		{
			Scene = gameObject.scene;
		}
	}
}
