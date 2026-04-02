using System;
using PlatformCore.Core;

namespace PlatformCore.Services.Network
{
	public interface INetworkSessionBridge : IService
	{
		NetworkSessionSnapshot Snapshot { get; }
		event Action<NetworkSessionSnapshot> SessionStateChanged;
		void UpdateSessionState(NetworkSessionSnapshot snapshot);
	}
}
