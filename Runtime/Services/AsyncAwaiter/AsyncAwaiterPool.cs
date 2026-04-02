using Cysharp.Threading.Tasks;

namespace PlatformCore.Services.AsyncAwaiter
{
	public sealed class AsyncAwaiterPool : IAsyncAwaiterPool
	{
		private readonly object _sync = new object();
		private int _pendingCount;
		private UniTaskCompletionSource _waitForEmptySource;

		public int PendingCount
		{
			get
			{
				lock (_sync)
				{
					return _pendingCount;
				}
			}
		}

		public void Add()
		{
			lock (_sync)
			{
				_pendingCount++;
			}
		}

		public void Done()
		{
			UniTaskCompletionSource waitForEmptySource = null;

			lock (_sync)
			{
				_pendingCount--;
				if (_pendingCount <= 0)
				{
					_pendingCount = 0;
					waitForEmptySource = _waitForEmptySource;
					_waitForEmptySource = null;
				}
			}

			waitForEmptySource?.TrySetResult();
		}

		public UniTask WaitForEmptyAsync()
		{
			lock (_sync)
			{
				if (_pendingCount == 0)
				{
					return UniTask.CompletedTask;
				}

				if (_waitForEmptySource == null)
				{
					_waitForEmptySource = new UniTaskCompletionSource();
				}

				return _waitForEmptySource.Task;
			}
		}

		internal void CompleteAndReset()
		{
			UniTaskCompletionSource waitForEmptySource;
			lock (_sync)
			{
				_pendingCount = 0;
				waitForEmptySource = _waitForEmptySource;
				_waitForEmptySource = null;
			}

			waitForEmptySource?.TrySetResult();
		}
	}
}
