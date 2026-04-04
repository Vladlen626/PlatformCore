using PlatformCore.Services.Factory;
using UnityEngine;

namespace PlatformCore.Services.UI.Styles
{
	public static class TextStyleLibraryProvider
	{
		private static TextStyleLibrary cached;

		public static TextStyleLibrary GetDefault()
		{
			if (cached)
			{
				return cached;
			}

			cached = Resources.Load<TextStyleLibrary>(ResourcePaths.Platform.UI.TextStyleLibrary);
			return cached;
		}
	}
}
