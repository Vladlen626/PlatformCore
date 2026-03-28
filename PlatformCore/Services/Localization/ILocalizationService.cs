using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface ILocalizationService
{
	string CurrentLocale { get; }
	event Action<string> LocaleChanged;

	UniTask SetLocaleAsync(string locale);
	bool TryGet(string key, out string value);
	string Get(string key);
	string Get(string key, IReadOnlyList<string> args);
}
