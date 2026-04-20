using PlatformCore.Services.UI;
using UnityEngine;

namespace PlatformCore.Services.Notifications
{
	public class UINotificationsView : UIBaseElement
	{
		[SerializeField] private Transform list;

		public Transform List => list;
	}
}
