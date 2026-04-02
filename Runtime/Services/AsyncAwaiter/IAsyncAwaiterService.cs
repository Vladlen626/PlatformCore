namespace PlatformCore.Services.AsyncAwaiter
{
	public interface IAsyncAwaiterService
	{
		IAsyncAwaiterPool GetPool(string poolId = AsyncAwaiterService.DefaultPoolId);
	}
}
