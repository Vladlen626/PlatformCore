using System;
using TMPro;
using UnityEngine;

namespace PlatformCore.Services.UI.Styles
{
	[Serializable]
	public struct TextStyleRef
	{
		[SerializeField]
		private string id;

		public string Id
		{
			get => id;
			set => id = value;
		}

		public TextStyleEntry Value
		{
			get
			{
				var library = TextStyleLibraryProvider.GetDefault();
				if (library == null || string.IsNullOrWhiteSpace(id))
				{
					return null;
				}

				var style = library.GetStyle(id);
				if (style == null)
				{
					return null;
				}

				return style;
			}
		}

		public Color Color => Value?.Color ?? Color.white;

		public void ApplyTo(TextMeshProUGUI text)
		{
			if (!text)
			{
				return;
			}

			var style = Value;
			if (style == null)
			{
				return;
			}

			text.color = style.Color;
			if (style.UseAdvanced)
			{
				if (style.FontSize > 0f)
				{
					text.fontSize = style.FontSize;
				}

				text.fontStyle = style.FontStyle;
			}
		}

		public TextStyleRef(string id)
		{
			this.id = id ?? string.Empty;
		}
	}
}
