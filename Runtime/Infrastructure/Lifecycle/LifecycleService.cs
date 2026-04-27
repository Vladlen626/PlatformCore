using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;

namespace PlatformCore.Infrastructure
{
	public class LifecycleService : IService
	{
		private readonly List<IBaseController> _managedObjects = new List<IBaseController>();
		private readonly HashSet<IBaseController> _managedObjectsSet = new HashSet<IBaseController>();
		private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
		private readonly List<IFixedUpdatable> _fixedUpdatables = new List<IFixedUpdatable>();
		private readonly List<ILateUpdatable> _lateUpdatables = new List<ILateUpdatable>();
		private bool _isDisposed;

		public void Register(IBaseController controller)
		{
			if (_isDisposed)
			{
				return;
			}

			if (controller == null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			RegisterInternal(controller, activateOnly: true);
		}

		public async UniTask RegisterAsync(IBaseController controller)
		{
			if (_isDisposed)
			{
				return;
			}

			if (controller == null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			if (_managedObjectsSet.Contains(controller))
			{
				return;
			}

			if (controller is IPreloadable preloadable)
			{
				await preloadable.PreloadAsync();
			}

			RegisterInternal(controller, activateOnly: false);
		}

		public void Unregister(IBaseController controller)
		{
			if (controller == null)
			{
				return;
			}

			if (_managedObjectsSet.Remove(controller) == false)
			{
				return;
			}

			if (controller is IDeactivatable deactivatable)
			{
				deactivatable.Deactivate();
			}

			_managedObjects.Remove(controller);

			if (controller is IUpdatable updatable)
			{
				_updatables.Remove(updatable);
			}

			if (controller is IFixedUpdatable fixedUpdatable)
			{
				_fixedUpdatables.Remove(fixedUpdatable);
			}

			if (controller is ILateUpdatable lateUpdatable)
			{
				_lateUpdatables.Remove(lateUpdatable);
			}
		}

		public async UniTask RegisterControllersGroupAsync(IEnumerable<IBaseController> controllers)
		{
			if (_isDisposed)
			{
				return;
			}

			if (controllers == null)
			{
				throw new ArgumentNullException(nameof(controllers));
			}

			foreach (var controller in controllers)
			{
				await RegisterAsync(controller);
			}
		}

		public void UnregisterControllersGroup(IEnumerable<IBaseController> controllers)
		{
			if (controllers == null)
			{
				return;
			}

			foreach (var controller in controllers)
			{
				Unregister(controller);
			}
		}

		public void Update(float deltaTime)
		{
			for (int i = _updatables.Count - 1; i >= 0; i--)
			{
				if (i < _updatables.Count)
				{
					_updatables[i]?.OnUpdate(deltaTime);
				}
			}
		}

		public void FixedUpdate(float fixedDeltaTime)
		{
			for (int i = _fixedUpdatables.Count - 1; i >= 0; i--)
			{
				if (i < _fixedUpdatables.Count)
				{
					_fixedUpdatables[i]?.OnFixedUpdate(fixedDeltaTime);
				}
			}
		}

		public void LateUpdate(float deltaTime)
		{
			for (int i = _lateUpdatables.Count - 1; i >= 0; i--)
			{
				if (i < _lateUpdatables.Count)
				{
					_lateUpdatables[i]?.OnLateUpdate(deltaTime);
				}
			}
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			for (int i = _managedObjects.Count - 1; i >= 0; i--)
			{
				var obj = _managedObjects[i];

				if (obj is IDeactivatable deactivatable)
				{
					deactivatable.Deactivate();
				}

				if (obj is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}

			_managedObjects.Clear();
			_managedObjectsSet.Clear();
			_updatables.Clear();
			_fixedUpdatables.Clear();
			_lateUpdatables.Clear();
			_isDisposed = true;
		}

		private void RegisterInternal(IBaseController controller, bool activateOnly)
		{
			if (_managedObjectsSet.Add(controller) == false)
			{
				return;
			}

			var activated = false;
			try
			{
				if (activateOnly && controller is IPreloadable)
				{
					throw new InvalidOperationException($"Controller '{controller.GetType().Name}' requires RegisterAsync because it is preloadable.");
				}

				if (controller is IActivatable activatable)
				{
					activatable.Activate();
					activated = true;
				}

				_managedObjects.Add(controller);
				if (controller is IUpdatable updatable)
				{
					_updatables.Add(updatable);
				}

				if (controller is IFixedUpdatable fixedUpdatable)
				{
					_fixedUpdatables.Add(fixedUpdatable);
				}

				if (controller is ILateUpdatable lateUpdatable)
				{
					_lateUpdatables.Add(lateUpdatable);
				}
			}
			catch
			{
				_managedObjectsSet.Remove(controller);
				if (activated && controller is IDeactivatable deactivatable)
				{
					deactivatable.Deactivate();
				}

				throw;
			}
		}
	}
}
