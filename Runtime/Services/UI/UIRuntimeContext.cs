using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCore.Services.UI
{
	public class UIRuntimeContext : MonoBehaviour, IUIRuntimeContext
	{
		[SerializeField] private UICanvasEntry[] _canvasEntries;

		public IReadOnlyList<UICanvasEntry> CanvasEntries => _canvasEntries ?? Array.Empty<UICanvasEntry>();
	}
}
