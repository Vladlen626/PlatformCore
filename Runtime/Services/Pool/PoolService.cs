using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlatformCore.Core;
using PlatformCore.Services.Factory;
using UnityEngine;

namespace PlatformCore.Services.Pool
{
	public class PoolService : IPoolService, IService
	{
		private readonly ILoggerService _logger;
		private readonly IResourceService _resourceService;
		private readonly Transform _poolRoot;
		private readonly Dictionary<(Type, string), IObjectPool> _pools = new();

		public PoolService(ILoggerService logger, IResourceService resourceService, Transform poolRoot)
		{
			_logger = logger;
			_resourceService = resourceService;
			_poolRoot = poolRoot;
		}

		public async UniTask CreatePoolAsync<T>(string key, string prefabPath, int initialSize = 10, Transform parent = null)
			where T : Component
		{
			var prefab = await _resourceService.LoadAsync<GameObject>(prefabPath);
			var poolParent = parent ?? CreatePoolParent<T>(key);
			var pool = new ObjectPool<T>(prefab, initialSize, poolParent, _logger);
			_pools.Add((typeof(T), key), pool);
		}

		public T Rent<T>(string key, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
			where T : Component
		{
			var pool = (ObjectPool<T>)_pools[(typeof(T), key)];
			var obj = pool.Rent();

			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.transform.SetParent(parent);

			return obj;
		}

		public void Return<T>(string key, T component) where T : Component
		{
			var pool = (ObjectPool<T>)_pools[(typeof(T), key)];
			pool.Return(component);
		}

		public void ReturnDelayed<T>(string key, T component, float delay) where T : Component
		{
			ReturnDelayedAsync(key, component, delay).Forget();
		}

		private async UniTask ReturnDelayedAsync<T>(string key, T component, float delay) where T : Component
		{
			await UniTask.Delay(TimeSpan.FromSeconds(delay));
			Return(key, component);
		}

		public (int active, int inactive) GetPoolStats<T>(string key) where T : Component
		{
			var pool = (ObjectPool<T>)_pools[(typeof(T), key)];
			return pool.GetStats();
		}

		public void ClearPool<T>(string key) where T : Component
		{
			var pool = (ObjectPool<T>)_pools[(typeof(T), key)];
			pool.Clear();
			_pools.Remove((typeof(T), key));
		}

		private Transform CreatePoolParent<T>(string key) where T : Component
		{
			var poolName = $"Pool_{typeof(T).Name}_{key.Replace('/', '_')}";
			var poolObject = new GameObject(poolName);
			poolObject.transform.SetParent(_poolRoot);
			return poolObject.transform;
		}

		public void Dispose()
		{
			foreach (var pool in _pools.Values)
			{
				pool.Clear();
			}

			_pools.Clear();
		}
	}
}
