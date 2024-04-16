namespace Core.Shared.Dictionaries;

/// <summary>
/// Provides a singleton-like class by being initialised at the start of every API allowing for any process to know on
/// which station it operates or if it is on the server.
/// </summary>
public static class Server
{
	public static TimeSpan WatchdogDelay { get; set; }
}