namespace PlatformCore.Services.Factory
{
	public static partial class ResourcePaths
	{
		[System.Obsolete("Use ResourcePaths.Sample.GameAnalytics for game-specific resources.")]
		public static class GameAnalytics
		{
			public const string Settings = Sample.GameAnalytics.Settings;
		}

		[System.Obsolete("Use ResourcePaths.Sample.Images for game-specific resources.")]
		public static class Images
		{
			public const string TrainArriving = Sample.Images.TrainArriving;
			public const string TrainLeaving = Sample.Images.TrainLeaving;
		}

		[System.Obsolete("Use ResourcePaths.Sample.Items for game-specific resources.")]
		public static class Items
		{
			public const string DicePrefab = Sample.Items.DicePrefab;
			public const string ExtraDiceCapItem = Sample.Items.ExtraDiceCapItem;
			public const string ItemBase = Sample.Items.ItemBase;
			public const string ModifierSilencerItem = Sample.Items.ModifierSilencerItem;
			public const string PassMultiplierItem = Sample.Items.PassMultiplierItem;
			public const string RerollSelectedItem = Sample.Items.RerollSelectedItem;
			public const string StepUpItem = Sample.Items.StepUpItem;
		}

		[System.Obsolete("Use ResourcePaths.Sample.Json for game-specific resources.")]
		public static class Json
		{
			public const string dice_game_modifiers_schedule = Sample.Json.dice_game_modifiers_schedule;
			public const string dice_game_rules = Sample.Json.dice_game_rules;
			public const string dice_types = Sample.Json.dice_types;
			public const string enemy_ai_scenario_schedule = Sample.Json.enemy_ai_scenario_schedule;
			public const string enemy_ai_scenarios = Sample.Json.enemy_ai_scenarios;
			public const string items_catalog = Sample.Json.items_catalog;
			public const string modifiers_ui = Sample.Json.modifiers_ui;
			public const string player = Sample.Json.player;
			public const string run_rules = Sample.Json.run_rules;
			public const string shop = Sample.Json.shop;
			public const string stations = Sample.Json.stations;
			public const string texts_eng = Sample.Json.texts_eng;
			public const string texts_ru = Sample.Json.texts_ru;
		}

		[System.Obsolete("Use ResourcePaths.Sample.Player for game-specific resources.")]
		public static class Player
		{
			public const string CinemachineCamera = Sample.Player.CinemachineCamera;
			public const string NpcBase = Sample.Player.NpcBase;
			public const string NpcConductor = Sample.Player.NpcConductor;
			public const string NpcEnemy = Sample.Player.NpcEnemy;
			public const string NpcPassenger = Sample.Player.NpcPassenger;
			public const string NpcShopKeeper = Sample.Player.NpcShopKeeper;
			public const string PlayerBase = Sample.Player.PlayerBase;
			public const string PlayerInventory = Sample.Player.PlayerInventory;
		}

		[System.Obsolete("Use ResourcePaths.Platform.Root or ResourcePaths.Sample.Root.")]
		public static class Root
		{
			public const string ComboUpgradesConfig = Sample.Root.ComboUpgradesConfig;
			public const string DiceScoringConfig = Sample.Root.DiceScoringConfig;
			public const string PrimeTweenSettings = Platform.Root.PrimeTweenSettings;
		}

		[System.Obsolete("Use ResourcePaths.Sample.Shop for game-specific resources.")]
		public static class Shop
		{
			public const string ShopItemDice = Sample.Shop.ShopItemDice;
			public const string TradeItem = Sample.Shop.TradeItem;
		}

		[System.Obsolete("Use ResourcePaths.Platform.UI or ResourcePaths.Sample.UI depending on ownership.")]
		public static class UI
		{
			public const string ColorStyleLibrary = Platform.UI.ColorStyleLibrary;
			public const string ElementBackground = Sample.UI.ElementBackground;
			public const string TextStyleLibrary = Platform.UI.TextStyleLibrary;
			public const string DefaultTMP = Sample.UI.DefaultTMP;
			public const string DescriptionTMP = Sample.UI.DescriptionTMP;
			public const string HeaderTMP = Sample.UI.HeaderTMP;
			public const string TextsVerticalSizebable = Sample.UI.TextsVerticalSizebable;
			public const string UICursorView = Platform.UI.UICursorView;
			public const string UIDiceUpgradeVariantView = Sample.UI.UIDiceUpgradeVariantView;
			public const string UIDiceUpgradeView = Sample.UI.UIDiceUpgradeView;
			public const string UIEndView = Sample.UI.UIEndView;
			public const string UIGlobalNotificationView = Sample.UI.UIGlobalNotificationView;
			public const string UIHintView = Sample.UI.UIHintView;
			public const string UIMainMenu = Sample.UI.UIMainMenu;
			public const string UIModifierDarkViewVariant = Sample.UI.UIModifierDarkViewVariant;
			public const string UIModifiersView = Sample.UI.UIModifiersView;
			public const string UIModifierView = Sample.UI.UIModifierView;
			public const string UINotificationsView = Sample.UI.UINotificationsView;
			public const string UINotificationView = Sample.UI.UINotificationView;
			public const string UIPlayerHud = Sample.UI.UIPlayerHud;
			public const string UIQuestsView = Sample.UI.UIQuestsView;
			public const string UIQuestView = Sample.UI.UIQuestView;
			public const string UISettings = Sample.UI.UISettings;
			public const string UISleepView = Sample.UI.UISleepView;
			public const string UISpeechView = Sample.UI.UISpeechView;
			public const string UIStatsView = Sample.UI.UIStatsView;
			public const string UITooltip = Sample.UI.UITooltip;
			public const string UITransitionView = Sample.UI.UITransitionView;
		}
	}
}
