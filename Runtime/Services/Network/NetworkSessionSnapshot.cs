namespace PlatformCore.Services.Network
{
	public readonly struct NetworkSessionSnapshot
	{
		public static NetworkSessionSnapshot Offline => new NetworkSessionSnapshot(false, false);

		public NetworkSessionSnapshot(bool isServerStarted, bool isClientStarted)
		{
			IsServerStarted = isServerStarted;
			IsClientStarted = isClientStarted;
		}

		public bool IsServerStarted { get; }
		public bool IsClientStarted { get; }
		public bool IsAnySessionStarted => IsServerStarted || IsClientStarted;
	}
}
