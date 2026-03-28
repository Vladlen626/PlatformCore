using System;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;
using PlatformCore.Services;
using PlatformCore.Services.Network;

namespace PlatformCore.Infrastructure.Network.FishNet
{
	public sealed class NetworkSessionService : INetworkSessionService, IBaseController, IActivatable, IDeactivatable
	{
		private readonly INetworkSessionBridge _networkSessionBridge;
		private readonly ILoggerService _loggerService;
		private bool _isSubscribed;

		public NetworkSessionService(INetworkSessionBridge networkSessionBridge, ILoggerService loggerService)
		{
			_networkSessionBridge = networkSessionBridge;
			_loggerService = loggerService;
			Snapshot = NetworkSessionSnapshot.Offline;
		}

		public NetworkSessionSnapshot Snapshot { get; private set; }

		public event Action<NetworkSessionSnapshot> SessionStateChanged;

		public void Activate()
		{
			if (_isSubscribed)
			{
				return;
			}

			_networkSessionBridge.SessionStateChanged += OnSessionStateChanged;
			_isSubscribed = true;
			ApplySnapshot(_networkSessionBridge.Snapshot);
			_loggerService?.Log("[NetworkSessionService] FishNet session hooks activated.");
		}

		public void Deactivate()
		{
			if (_isSubscribed == false)
			{
				return;
			}

			_networkSessionBridge.SessionStateChanged -= OnSessionStateChanged;
			_isSubscribed = false;
			_loggerService?.Log("[NetworkSessionService] FishNet session hooks deactivated.");
		}

		public void Dispose()
		{
			Deactivate();
			SessionStateChanged = null;
		}

		private void OnSessionStateChanged(NetworkSessionSnapshot snapshot)
		{
			ApplySnapshot(snapshot);
		}

		private void ApplySnapshot(NetworkSessionSnapshot snapshot)
		{
			Snapshot = snapshot;
			SessionStateChanged?.Invoke(snapshot);
		}
	}
}
