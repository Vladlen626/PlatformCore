using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;

namespace PlatformCore.Infrastructure.Network.FishNet
{
	public sealed class NetworkSessionLifecycleController : IBaseController, IActivatable, IDeactivatable
	{
		private readonly NetworkSessionService _networkSessionService;

		public NetworkSessionLifecycleController(NetworkSessionService networkSessionService)
		{
			_networkSessionService = networkSessionService;
		}

		public void Activate()
		{
			_networkSessionService.StartSessionTracking();
		}

		public void Deactivate()
		{
			_networkSessionService.StopSessionTracking();
		}
	}
}
