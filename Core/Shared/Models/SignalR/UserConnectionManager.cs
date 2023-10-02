namespace Core.Shared.Models.SignalR;

/// <summary>
///     Manages user connections to SignalR connections.
/// </summary>
/// <typeparam name="T">The type of key used to map users to connections.</typeparam>
public class UserConnectionManager<T>
	where T : notnull
{
	private readonly Dictionary<T, HashSet<string>> _connections = new();

    /// <summary>
    ///     Gets the total number of connections in the collection.
    /// </summary>
    public int Count => _connections.Count;

    /// <summary>
    ///     Adds a new connection for a given user.
    /// </summary>
    /// <param name="key">The key of the user.</param>
    /// <param name="connectionId">The SignalR connection ID.</param>
    /// <exception cref="ArgumentNullException">The key or connectionId parameter is null.</exception>
    public void Add(T key, string connectionId)
	{
		if (key == null || connectionId == null) throw new ArgumentNullException();

		lock (_connections)
		{
			if (_connections.TryGetValue(key, out HashSet<string>? connections))
			{
				lock (connections)
				{
					connections.Add(connectionId);
				}
			}
			else
			{
				connections = new HashSet<string> { connectionId };
				_connections.TryAdd(key, connections);
			}
		}
	}

    /// <summary>
    ///     Retrieves all connections associated with a given user.
    /// </summary>
    /// <param name="key">The key of the user.</param>
    /// <returns>A collection of strings representing SignalR connection IDs.</returns>
    /// <exception cref="ArgumentNullException">The key parameter is null.</exception>
    public IEnumerable<string> GetConnections(T key)
	{
		if (key == null) throw new ArgumentNullException();

		return _connections.TryGetValue(key, out HashSet<string>? connections)
			? connections
			: Enumerable.Empty<string>();
	}

    /// <summary>
    ///     Get the count of all connections.
    /// </summary>
    /// <return>A count</return>
    /// <returns></returns>
    public int GetConnectionsCount()
	{
		int count = 0;

		foreach (HashSet<string> values in _connections.Values) count += values.Count;
		return count;
	}

    /// <summary>
    ///     Removes a connection for a given user.
    /// </summary>
    /// <param name="key">The key of the user.</param>
    /// <param name="connectionId">The SignalR connection ID.</param>
    /// <exception cref="ArgumentNullException">The key or connectionId parameter is null.</exception>
    public void Remove(T key, string connectionId)
	{
		if (key == null || connectionId == null) throw new ArgumentNullException();

		lock (_connections)
		{
			if (_connections.TryGetValue(key, out HashSet<string>? connections))
				lock (connections)
				{
					connections.Remove(connectionId);

					if (connections.Count == 0) _connections.Remove(key);
				}
		}
	}
}