using PlatformCore.Services.Factory;
using UnityEngine;

namespace PlatformCore.Services.UI.Styles
{
	public static class ColorStyleLibraryProvider
	{
		private static ColorStyleLibrary cached;

		public static ColorStyleLibrary GetDefault()
		{
			if (cached)
			{
				return cached;
			}

			cached = Resources.Load<ColorStyleLibrary>(ResourcePaths.Platform.UI.ColorStyleLibrary);
			return cached;
		}
	}
}
