using System;
using UnityEngine;

namespace PlatformCore.Services.UI.Styles
{
	[Serializable]
	public struct ColorStyleRef
	{
		[SerializeField]
		private string id;

		public string Id
		{
			get => id;
			set => id = value;
		}

		public Color Value
		{
			get
			{
				var library = ColorStyleLibraryProvider.GetDefault();
				if (library == null || string.IsNullOrWhiteSpace(id))
				{
					return Color.white;
				}

				var style = library.GetStyle(id);
				if (style == null)
				{
					return Color.white;
				}

				return style.Color;
			}
		}

		public ColorStyleRef(string id)
		{
			this.id = id ?? string.Empty;
		}
	}
}
