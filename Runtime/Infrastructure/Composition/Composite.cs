using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;

namespace PlatformCore.Infrastructure.Composition
{
	public abstract class Composite : IBaseController, IActivatable, IDeactivatable, IDisposable
	{
		private readonly LifecycleService _lifecycle;
		private readonly List<IBaseController> _controllers = new List<IBaseController>();
		private readonly List<IDisposable> _ownedDisposables = new List<IDisposable>();
		private bool _isBuilt;
		private bool _isActive;
		private bool _isDisposed;

		protected Composite(LifecycleService lifecycle)
		{
			_lifecycle = lifecycle;
		}

		public void Activate()
		{
			if (_isDisposed || _isActive)
			{
				return;
			}

			BuildIfRequired();
			_lifecycle.RegisterControllersGroupAsync(_controllers).Forget();
			_isActive = true;
		}

		public void Deactivate()
		{
			if (_isActive == false)
			{
				return;
			}

			_lifecycle.UnregisterControllersGroup(_controllers);
			_isActive = false;
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			Deactivate();

			for (int i = _ownedDisposables.Count - 1; i >= 0; i--)
			{
				_ownedDisposables[i]?.Dispose();
			}

			_ownedDisposables.Clear();
			_controllers.Clear();
			_isDisposed = true;
		}

		protected abstract CompositeInstaller CreateInstaller();

		private void BuildIfRequired()
		{
			if (_isBuilt)
			{
				return;
			}

			var installer = CreateInstaller();
			if (installer != null)
			{
				var builder = new CompositeBuilder(_controllers, _ownedDisposables);
				installer.Install(builder);
			}

			_isBuilt = true;
		}
	}
}
