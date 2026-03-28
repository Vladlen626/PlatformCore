using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class LocalizationServiceBase : BaseAsyncService, ILocalizationService
{
	private readonly ConfigService configService;
	private readonly LocalizationServiceOptions options;

	private readonly Dictionary<string, Dictionary<string, string>> localeCache = new();

	public string CurrentLocale { get; private set; }
	public event System.Action<string> LocaleChanged;

	public LocalizationServiceBase(ConfigService configService, LocalizationServiceOptions options = null)
	{
		this.configService = configService;
		this.options = options ?? new LocalizationServiceOptions();
		CurrentLocale = this.options.DefaultLocale;
	}

	protected override UniTask OnPreInitializeAsync(CancellationToken ct)
	{
		return SetLocaleAsync(CurrentLocale);
	}

	public async UniTask SetLocaleAsync(string locale)
	{
		if (string.IsNullOrWhiteSpace(locale))
		{
			return;
		}

		if (!localeCache.ContainsKey(locale))
		{
			await LoadLocaleAsync(locale);
		}

		if (!localeCache.ContainsKey(locale))
		{
			return;
		}

		if (CurrentLocale == locale)
		{
			return;
		}

		CurrentLocale = locale;
		LocaleChanged?.Invoke(CurrentLocale);
	}

	public bool TryGet(string key, out string value)
	{
		value = key;
		if (string.IsNullOrWhiteSpace(key))
		{
			return false;
		}

		if (!localeCache.TryGetValue(CurrentLocale, out var currentLocaleMap))
		{
			return false;
		}

		if (!currentLocaleMap.TryGetValue(key, out value))
		{
			value = key;
			return false;
		}

		return true;
	}

	public string Get(string key)
	{
		TryGet(key, out var value);
		return value;
	}

	public string Get(string key, IReadOnlyList<string> args)
	{
		var value = Get(key);
		if (args == null || args.Count == 0)
		{
			return value;
		}

		return string.Format(value, args as object[] ?? ToObjectArray(args));
	}

	private async UniTask LoadLocaleAsync(string locale)
	{
		if (!options.LocaleResourcePaths.TryGetValue(locale, out var resourcePath) || string.IsNullOrWhiteSpace(resourcePath))
		{
			return;
		}

		var textsConfig = await configService.GetFirstOrDefaultAsync<TextsConfig>(resourcePath);
		if (textsConfig?.texts == null)
		{
			return;
		}

		localeCache[locale] = new Dictionary<string, string>(textsConfig.texts);
	}

	private static object[] ToObjectArray(IReadOnlyList<string> args)
	{
		var data = new object[args.Count];
		for (var i = 0; i < args.Count; i++)
		{
			data[i] = args[i];
		}

		return data;
	}
}
