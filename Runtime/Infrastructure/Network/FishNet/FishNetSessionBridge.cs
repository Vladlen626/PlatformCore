using System;
using PlatformCore.Services.Network;

namespace PlatformCore.Infrastructure.Network.FishNet
{
	public sealed class FishNetSessionBridge : INetworkSessionBridge
	{
		public NetworkSessionSnapshot Snapshot { get; private set; } = NetworkSessionSnapshot.Offline;

		public event Action<NetworkSessionSnapshot> SessionStateChanged;

		public void UpdateSessionState(NetworkSessionSnapshot snapshot)
		{
			if (Snapshot.IsServerStarted == snapshot.IsServerStarted && Snapshot.IsClientStarted == snapshot.IsClientStarted)
			{
				return;
			}

			Snapshot = snapshot;
			SessionStateChanged?.Invoke(snapshot);
		}

		public void Dispose()
		{
			SessionStateChanged = null;
		}
	}
}
