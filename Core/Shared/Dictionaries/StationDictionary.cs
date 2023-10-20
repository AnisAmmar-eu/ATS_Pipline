using Org.BouncyCastle.Security;

namespace Core.Shared.Dictionaries;

public static class Station
{
	public const string Station1 = "S1";
	public const string Station2 = "S2";
	public const string Station3 = "S3";
	public const string Station4 = "S4";
	public const string Station5 = "S5";

	public static string Name
	{
		set
		{
			if (_name != null)
				throw new InvalidOperationException("Station name has already been defined.");
			_name = value;
			Type = StationNameToType();
			ID = StationNameToID();
		}
		get => _name ?? throw new InvalidOperationException("Station name has not been defined.");
	}

	private static string? _name { get; set; }

	public static StationType Type { get; private set; }
	public static int ID { get; private set; }


	private static StationType StationNameToType()
	{
		return Name switch
		{
			Station1 or Station2 => StationType.S1S2,
			Station3 or Station4 => StationType.S3S4,
			Station5 => StationType.S5,
			_ => throw new InvalidParameterException("Unknown station name.")
		};
	}

	private static int StationNameToID()
	{
		return Name switch
		{
			Station1 => 1,
			Station2 => 2,
			Station3 => 3,
			Station4 => 4,
			Station5 => 5,
			_ => throw new InvalidParameterException("Unknown station name.")
		};
	}
}

public enum StationType
{
	S1S2,
	S3S4,
	S5
}