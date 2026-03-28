using Cysharp.Threading.Tasks;
using PlatformCore.Core;
using PlatformCore.Core.Lifecycle;

namespace PlatformCore.Services.UI
{
	public class BaseContextController<T> : IBaseController, IActivatable, IPreloadable where T : UIBaseElement
	{
		protected readonly IUIService _uiService;
		protected T _context;

		protected BaseContextController(IUIService uiService)
		{
			_uiService = uiService;
		}

		public async UniTask PreloadAsync()
		{
			await _uiService.PreloadAsync<T>();
			await OnPreloadAsync();
		}

		public void Activate()
		{
			_context = _uiService.Show<T>();
			OnActivate();
		}
		
		public void Deactivate()
		{
			OnDeactivate();
			if (ShouldUnloadOnDeactivate)
			{
				_uiService.Unload<T>();
			}
			else
			{
				_uiService.Hide<T>();
			}

			_context = null;
		}
		
		protected virtual bool ShouldUnloadOnDeactivate => true;
		protected virtual void OnActivate(){}
		protected virtual void OnDeactivate(){}

		protected virtual UniTask OnPreloadAsync()
		{
			return UniTask.CompletedTask;
		}
	}
}
