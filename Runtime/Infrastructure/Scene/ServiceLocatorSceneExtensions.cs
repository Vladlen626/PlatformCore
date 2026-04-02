using PlatformCore.Services;

namespace PlatformCore.Infrastructure.Scene
{
	public static class ServiceLocatorSceneExtensions
	{
		public static ISceneService RegisterSceneManagementFoundation(this ServiceLocator serviceLocator,
			PersistentSceneContext persistentSceneContext,
			ILoggerService loggerService)
		{
			var sceneService = new SceneService(loggerService, persistentSceneContext);
			serviceLocator.Register<ISceneService, SceneService>(sceneService);
			return sceneService;
		}
	}
}
