using System.Collections.Generic;
using PlatformCore.Core;

namespace PlatformCore.Services.AsyncAwaiter
{
	public sealed class AsyncAwaiterService : IAsyncAwaiterService, IService
	{
		public const string DefaultPoolId = "default";

		private readonly Dictionary<string, AsyncAwaiterPool> _pools = new();
		private readonly object _sync = new object();

		public AsyncAwaiterService()
		{
			_pools[DefaultPoolId] = new AsyncAwaiterPool();
		}

		public IAsyncAwaiterPool GetPool(string poolId = DefaultPoolId)
		{
			poolId = string.IsNullOrEmpty(poolId) ? DefaultPoolId : poolId;

			lock (_sync)
			{
				if (_pools.TryGetValue(poolId, out var pool))
				{
					return pool;
				}

				pool = new AsyncAwaiterPool();
				_pools[poolId] = pool;
				return pool;
			}
		}

		public void Dispose()
		{
			lock (_sync)
			{
				foreach (var pool in _pools.Values)
				{
					pool.CompleteAndReset();
				}

				_pools.Clear();
			}
		}
	}
}
