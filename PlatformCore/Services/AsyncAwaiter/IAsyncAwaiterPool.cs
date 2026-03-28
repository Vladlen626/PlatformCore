using Cysharp.Threading.Tasks;

namespace PlatformCore.Services.AsyncAwaiter
{
	public interface IAsyncAwaiterPool
	{
		int PendingCount { get; }

		void Add();

		void Done();

		UniTask WaitForEmptyAsync();
	}
}
