namespace Core.Shared.Dictionaries;

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

public static class AnodeFormat
{
	public const string RIDFormat = "yyyyMMdd-HHmmss-fff";
}