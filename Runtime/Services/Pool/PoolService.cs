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
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
			_poolRoot = poolRoot ?? throw new ArgumentNullException(nameof(poolRoot));
		}

		public async UniTask CreatePoolAsync<T>(string key, string prefabPath, int initialSize = 10, Transform parent = null)
			where T : Component
		{
			var poolId = (typeof(T), key);
			if (_pools.ContainsKey(poolId))
			{
				throw new InvalidOperationException($"Pool for type '{typeof(T).Name}' and key '{key}' is already registered.");
			}

			var prefab = await _resourceService.LoadAsync<GameObject>(prefabPath);
			if (!prefab)
			{
				throw new InvalidOperationException($"Pool prefab not found for type '{typeof(T).Name}' and key '{key}'. Path: '{prefabPath}'.");
			}

			if (!prefab.GetComponent<T>())
			{
				throw new InvalidOperationException($"Pool prefab at '{prefabPath}' is missing required component '{typeof(T).Name}' for key '{key}'.");
			}

			var poolParent = parent ?? CreatePoolParent<T>(key);
			var pool = new ObjectPool<T>(prefab, initialSize, poolParent, _logger);
			_pools.Add(poolId, pool);
		}

		public T Rent<T>(string key, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
			where T : Component
		{
			var pool = GetRequiredPool<T>(key);
			var obj = pool.Rent();

			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.transform.SetParent(parent);
			return obj;
		}

		public void Return<T>(string key, T component) where T : Component
		{
			var pool = GetRequiredPool<T>(key);
			pool.Return(component);
		}

		public (int active, int inactive) GetPoolStats<T>(string key) where T : Component
		{
			var pool = GetRequiredPool<T>(key);
			return pool.GetStats();
		}

		public void ClearPool<T>(string key) where T : Component
		{
			var pool = GetRequiredPool<T>(key);
			pool.Clear();
			_pools.Remove((typeof(T), key));
		}

		public void Dispose()
		{
			foreach (var pool in _pools.Values)
			{
				pool.Clear();
			}

			_pools.Clear();
		}

		private ObjectPool<T> GetRequiredPool<T>(string key) where T : Component
		{
			if (_pools.TryGetValue((typeof(T), key), out var pool) == false)
			{
				throw new InvalidOperationException($"Pool for type '{typeof(T).Name}' and key '{key}' was not found.");
			}

			return (ObjectPool<T>)pool;
		}

		private Transform CreatePoolParent<T>(string key) where T : Component
		{
			var poolName = $"Pool_{typeof(T).Name}_{key.Replace('/', '_')}";
			var poolObject = new GameObject(poolName);
			poolObject.transform.SetParent(_poolRoot);
			return poolObject.transform;
		}
	}
}
