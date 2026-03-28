using System;
using PlatformCore.Core;

namespace PlatformCore.Services.Network
{
	public interface INetworkSessionService : IService
	{
		NetworkSessionSnapshot Snapshot { get; }
		event Action<NetworkSessionSnapshot> SessionStateChanged;
	}
}
