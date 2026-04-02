using System;
using System.Collections.Generic;
using PlatformCore.Core;

namespace PlatformCore.Infrastructure.Composition
{
	public sealed class CompositeBuilder
	{
		private readonly List<IBaseController> _controllers;
		private readonly List<IDisposable> _ownedDisposables;

		internal CompositeBuilder(List<IBaseController> controllers, List<IDisposable> ownedDisposables)
		{
			_controllers = controllers;
			_ownedDisposables = ownedDisposables;
		}

		public void AddController(IBaseController controller)
		{
			if (controller == null)
			{
				return;
			}

			_controllers.Add(controller);
		}

		public void AddOwnedDisposable(IDisposable disposable)
		{
			if (disposable == null)
			{
				return;
			}

			_ownedDisposables.Add(disposable);
		}
	}
}
