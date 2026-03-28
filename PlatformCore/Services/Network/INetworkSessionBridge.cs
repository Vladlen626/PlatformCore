using System;

namespace PlatformCore.Services.Network
{
	public interface INetworkSessionBridge
	{
		NetworkSessionSnapshot Snapshot { get; }
		event Action<NetworkSessionSnapshot> SessionStateChanged;
	}
}
