using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlatformCore.Services.Pool
{
	public interface IPoolService
	{
		UniTask CreatePoolAsync<T>(string key, string prefabPath, int initialSize = 10, Transform parent = null)
			where T : Component;

		T Rent<T>(string key, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
			where T : Component;

		void Return<T>(string key, T component) where T : Component;
		
		void ReturnDelayed<T>(string key, T component, float delay) where T : Component;
		
		(int active, int inactive) GetPoolStats<T>(string key) where T : Component;
		
		void ClearPool<T>(string key) where T : Component;
	}
}
