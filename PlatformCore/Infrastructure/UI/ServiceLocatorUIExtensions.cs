using PlatformCore.Services;
using PlatformCore.Services.Factory;
using PlatformCore.Services.UI;

namespace PlatformCore.Infrastructure.UI
{
	public static class ServiceLocatorUIExtensions
	{
		public static IUIService RegisterUIFoundation(this ServiceLocator serviceLocator,
			PersistentSceneContext persistentSceneContext,
			ILoggerService loggerService,
			IResourceService resourceService)
		{
			var uiService = new UIBaseService(loggerService, resourceService, persistentSceneContext.UICanvasEntries);
			serviceLocator.Register<IUIService, UIBaseService>(uiService);

			var cursorService = new CursorService(uiService, loggerService);
			serviceLocator.Register<ICursorService, CursorService>(cursorService);

			return uiService;
		}
	}
}
