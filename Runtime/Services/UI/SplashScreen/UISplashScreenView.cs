using UnityEngine;

namespace PlatformCore.Services.UI.SplashScreen
{
	public sealed class UISplashScreenView : UIBaseElement
	{
		public float Alpha => _group.alpha;

		protected override void OnAwake()
		{
			SetInputBlocked(false);
			SetAlpha(0f);
		}

		public void SetAlpha(float alpha)
		{
			_group.alpha = Mathf.Clamp01(alpha);
		}

		public void SetInputBlocked(bool isBlocked)
		{
			_group.interactable = isBlocked;
			_group.blocksRaycasts = isBlocked;
		}

		public void SetVisible(bool isVisible)
		{
			if (gameObject.activeSelf != isVisible)
			{
				gameObject.SetActive(isVisible);
			}
		}
	}
}
