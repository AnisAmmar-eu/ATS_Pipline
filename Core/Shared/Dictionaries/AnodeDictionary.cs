namespace Core.Shared.Dictionaries;

/// <summary>
/// Provides a set of Anode types.
/// </summary>
public static class AnodeTypeDict
{
	public const string D20 = "D20";
	public const string DX = "DX";
	public const string Undefined = "Undefined";

	public static string AnodeTypeIntToString(int anodeType)
	{
		return anodeType switch {
			1 => D20,
			2 => DX,
			_ => Undefined,
		};
	}
}

/// <summary>
/// Universal format of any <see cref="DateTimeOffset"/> among the project for backend purposes such as RID creation.
/// </summary>
public static class AnodeFormat
{
	public const string RIDFormat = "yyyyMMdd-HHmmss-fff";
}