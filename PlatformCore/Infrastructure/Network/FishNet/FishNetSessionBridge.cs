using System;
using PlatformCore.Services.Network;

namespace PlatformCore.Infrastructure.Network.FishNet
{
	public sealed class FishNetSessionBridge : INetworkSessionBridge
	{
		public NetworkSessionSnapshot Snapshot { get; private set; } = NetworkSessionSnapshot.Offline;

		public event Action<NetworkSessionSnapshot> SessionStateChanged;

		public void UpdateSessionState(bool isServerStarted, bool isClientStarted)
		{
			Snapshot = new NetworkSessionSnapshot(isServerStarted, isClientStarted);
			SessionStateChanged?.Invoke(Snapshot);
		}

		public void Dispose()
		{
			SessionStateChanged = null;
		}
	}
}
