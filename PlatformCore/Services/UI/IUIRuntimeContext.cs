using System.Collections.Generic;

namespace PlatformCore.Services.UI
{
	public interface IUIRuntimeContext
	{
		IReadOnlyList<UICanvasEntry> CanvasEntries { get; }
	}
}
