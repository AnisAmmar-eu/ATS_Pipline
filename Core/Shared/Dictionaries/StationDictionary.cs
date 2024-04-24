namespace Core.Shared.Dictionaries;

/// <summary>
/// Provides a singleton-like class by being initialised at the start of every API allowing for any process to know on
/// which station it operates or if it is on the server.
/// </summary>
public static class Station
{
	public const string Station1 = "S1";
	public const string Station2 = "S2";
	public const string Station3 = "S3";
	public const string Station4 = "S4";
	public const string Station5 = "S5";
	public const string Server = "Server";

	public static List<string> Stations = ["S1","S2","S3", "S4", "S5"];

	public static string Name
	{
		set
		{
			if (NameSub is not null)
				throw new InvalidOperationException("Station name has already been defined.");

			NameSub = value;
			IsServer = NameSub == Server;
			Type = StationNameToType(Name);
			ID = StationNameToID(Name);
		}
		get => NameSub ?? throw new InvalidOperationException("Station name has not been defined.");
	}

	public static bool IsServer { get; private set; }

	private static string? NameSub { get; set; }

	public static StationType Type { get; private set; }

	public static int ID { get; set; }

	public static string ServerAddress
	{
		set
		{
			if (ServerAddressSub is not null)
				throw new InvalidOperationException("Station serverAddress has already been defined.");

			ServerAddressSub = value;
		}
		get => ServerAddressSub ?? throw new InvalidOperationException("Station serverAddress has not been defined.");
	}

	private static string? ServerAddressSub { get; set; }

	public static StationType StationNameToType(string stationName)
	{
		return stationName switch {
			Station1 or Station2 => StationType.S1S2,
			Station3 or Station4 => StationType.S3S4,
			Station5 => StationType.S5,
			Server => StationType.Server,
			_ => throw new InvalidOperationException("Unknown station name."),
		};
	}

	public static int StationNameToID(string stationName)
	{
		return stationName switch {
			Station1 => 1,
			Station2 => 2,
			Station3 => 3,
			Station4 => 4,
			Station5 => 5,
			Server => 99,
			_ => throw new InvalidOperationException("Unknown station name."),
		};
	}

	public static bool IsMatchStation(int StationID)
	{
		return StationID >= StationNameToID(Station3);
	}
}

public enum StationType
{
	S1S2 = 0,
	S3S4 = 1,
	S5 = 2,
	Server = 3,
}