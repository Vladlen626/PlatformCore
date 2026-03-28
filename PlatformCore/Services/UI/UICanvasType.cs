namespace PlatformCore.Services.UI
{
	public enum UICanvasType
	{
		Default = 0,

		Gameplay = 5,

		Screen = 10,
		ScreenOverlay = 11,

		Modal = 20,
		ModalOverlay = 21,
		Hint = 22,

		Cursor = 90,
		AbsoluteTop = 100,

		PlayerHud = Gameplay,
		Menu = Screen,
		MenuOverlap = ScreenOverlay,
		Popup = Modal,
		Overlay = ModalOverlay,
		Tooltip = Hint
	}
}
