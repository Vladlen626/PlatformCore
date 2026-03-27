using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using PlatformCore.Core;
using PlatformCore.Services;

namespace PlatformCore.Infrastructure.Lifecycle
{
	public class LifecycleService : IService
	{
		private readonly List<IBaseController> _managedObjects = new List<IBaseController>();
		private readonly HashSet<IBaseController> _managedObjectsSet = new HashSet<IBaseController>();
		private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
		private readonly List<IFixedUpdatable> _fixedUpdatables = new List<IFixedUpdatable>();
		private readonly List<ILateUpdatable> _lateUpdatables = new List<ILateUpdatable>();
		private bool _isDisposed;

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

			// Один и тот же инстанс контроллера можно зарегистрировать только один раз.
			if (_managedObjectsSet.Add(controller) == false)
			{
				return;
			}

			_managedObjects.Add(controller);

			if (controller is IPreloadable preloadable)
			{
				await preloadable.PreloadAsync();
			}

			if (controller is IActivatable activatable)
			{
				activatable.Activate();
			}

			// Контроллер может реализовывать сразу несколько update-интерфейсов.
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

		public void Unregister(IBaseController controller)
		{
			if (controller == null)
			{
				return;
			}

			// Idempotent Unregister: повторный вызов безопасно игнорируется.
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

		public async UniTask RegisterControllersGroupAsync(List<IBaseController> controllersList)
		{
			if (_isDisposed)
			{
				return;
			}

			if (controllersList == null)
			{
				throw new ArgumentNullException(nameof(controllersList));
			}

			var tasks = new List<UniTask>();
			foreach (var controller in controllersList)
			{
				tasks.Add(RegisterAsync(controller));
			}

			await UniTask.WhenAll(tasks);
		}

		public async UniTask RegisterControllersGroupAsync(IBaseController[] controllersArray)
		{
			if (_isDisposed)
			{
				return;
			}

			if (controllersArray == null)
			{
				throw new ArgumentNullException(nameof(controllersArray));
			}

			var tasks = new List<UniTask>();
			foreach (var controller in controllersArray)
			{
				tasks.Add(RegisterAsync(controller));
			}

			await UniTask.WhenAll(tasks);
		}

		public void UnregisterControllersGroup(List<IBaseController> controllersList)
		{
			if (controllersList == null)
			{
				return;
			}

			foreach (var controller in controllersList)
			{
				Unregister(controller);
			}
		}

		public void UnregisterControllersGroup(IBaseController[] controllersArray)
		{
			if (controllersArray == null)
			{
				return;
			}

			foreach (var controller in controllersArray)
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

		// ReSharper disable Unity.PerformanceAnalysis
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

			// Семантика Dispose: для каждого зарегистрированного контроллера вызываем Deactivate (если есть),
			// затем Dispose (если реализован), и только после этого очищаем все внутренние коллекции сервиса.
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
	}
}
