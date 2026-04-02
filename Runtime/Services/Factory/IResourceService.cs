using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlatformCore.Services.Factory
{
	/// <summary>
	/// Platform-level wrapper around Unity Resources API for runtime services.
	/// </summary>
	public interface IResourceService
	{
		UniTask<T>  LoadAsync<T>(string path) where T : Object;
		void Unload(Object obj);
		UniTask UnloadUnusedAssetsAsync();
	}
}