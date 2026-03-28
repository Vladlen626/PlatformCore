using System.Collections.Generic;

public sealed class LocalizationServiceOptions
{
	public string DefaultLocale { get; set; } = "en";
	public IReadOnlyDictionary<string, string> LocaleResourcePaths { get; set; } =
		new Dictionary<string, string>();
}
