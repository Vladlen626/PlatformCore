using System;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;
using PlatformCore.Services;
using PlatformCore.Services.Network;
using FishNet;
using FishNet.Managing;
using FishNet.Transporting;

namespace PlatformCore.Infrastructure.Network.FishNet
{
	public sealed class FishNetRuntimeSessionAdapter : IBaseController, IActivatable, IDeactivatable, IUpdatable, IDisposable
	{
		private readonly INetworkSessionBridge _networkSessionBridge;
		private readonly ILoggerService _loggerService;

		private NetworkManager _networkManager;
		private bool _isSubscribed;

		public FishNetRuntimeSessionAdapter(INetworkSessionBridge networkSessionBridge, ILoggerService loggerService)
		{
			_networkSessionBridge = networkSessionBridge;
			_loggerService = loggerService;
		}

		public void Activate()
		{
			TryBindRuntimeCallbacks();
		}

		public void Deactivate()
		{
			UnsubscribeFromRuntimeCallbacks();
			_networkSessionBridge.UpdateSessionState(NetworkSessionSnapshot.Offline);
		}

		public void OnUpdate(float deltaTime)
		{
			if (_isSubscribed)
			{
				return;
			}

			TryBindRuntimeCallbacks();
		}

		public void Dispose()
		{
			Deactivate();
		}

		private void TryBindRuntimeCallbacks()
		{
			var currentNetworkManager = InstanceFinder.NetworkManager;
			if (!currentNetworkManager)
			{
				return;
			}

			if (_isSubscribed && _networkManager == currentNetworkManager)
			{
				return;
			}

			UnsubscribeFromRuntimeCallbacks();
			_networkManager = currentNetworkManager;
			_networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
			_networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
			_isSubscribed = true;
			ForwardSessionState();
			_loggerService?.Log("[FishNetRuntimeSessionAdapter] Runtime callbacks connected.");
		}

		private void UnsubscribeFromRuntimeCallbacks()
		{
			if (!_isSubscribed)
			{
				return;
			}

			if (_networkManager)
			{
				_networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
				_networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
			}

			_networkManager = null;
			_isSubscribed = false;
			_loggerService?.Log("[FishNetRuntimeSessionAdapter] Runtime callbacks disconnected.");
		}

		private void OnServerConnectionState(ServerConnectionStateArgs _) 
		{
			ForwardSessionState();
		}

		private void OnClientConnectionState(ClientConnectionStateArgs _)
		{
			ForwardSessionState();
		}

		private void ForwardSessionState()
		{
			if (!_networkManager)
			{
				_networkSessionBridge.UpdateSessionState(NetworkSessionSnapshot.Offline);
				return;
			}

			var snapshot = new NetworkSessionSnapshot(
				_networkManager.ServerManager.Started,
				_networkManager.ClientManager.Started);
			_networkSessionBridge.UpdateSessionState(snapshot);
		}
	}
}
