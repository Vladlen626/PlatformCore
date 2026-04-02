using System;
using TMPro;
using UnityEngine;

namespace PlatformCore.Services.UI.Styles
{
	[ExecuteAlways]
	public class TextStyleApplier : MonoBehaviour
	{
		[SerializeField]
		private TextStyleRef style;

		private TextMeshProUGUI target;

		private void OnEnable()
		{
			Apply();
		}

		private void OnValidate()
		{
			Apply();
		}

		public void Apply()
		{
			if (!target)
			{
				target = GetComponent<TextMeshProUGUI>();
			}

			if (!target)
			{
				throw new InvalidOperationException("TextStyleApplier target is missing.");
			}

			style.ApplyTo(target);
		}

	}
}
