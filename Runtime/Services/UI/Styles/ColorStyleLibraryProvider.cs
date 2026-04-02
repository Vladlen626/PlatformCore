using UnityEngine;

namespace PlatformCore.Services.UI.Styles
{
	public static class ColorStyleLibraryProvider
	{
		private const string DefaultResourcePath = "UI/ColorStyleLibrary";
		private static ColorStyleLibrary cached;

		public static ColorStyleLibrary GetDefault()
		{
			if (cached)
			{
				return cached;
			}

			cached = Resources.Load<ColorStyleLibrary>(DefaultResourcePath);
			return cached;
		}
	}
}
