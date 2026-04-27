using System;
using PlatformCore.Services.UI;
using UnityEngine;

namespace PlatformCore.Services.Notifications
{
	public class UINotificationsView : UIBaseElement
	{
		[SerializeField] private Transform list;

		public Transform List
		{
			get
			{
				ValidateReferences();
				return list;
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			ValidateReferences();
		}

		private void ValidateReferences()
		{
			if (!list)
			{
				throw new InvalidOperationException("UINotificationsView requires list reference.");
			}
		}
	}
}
