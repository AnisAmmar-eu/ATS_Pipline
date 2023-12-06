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
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(connectionId);

        lock (_connections)
		{
			if (_connections.TryGetValue(key, out HashSet<string>? connections))
			{
				lock (connections)
                    connections.Add(connectionId);
            }
			else
			{
				connections = [connectionId];
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
		ArgumentNullException.ThrowIfNull(key);

		return (_connections.TryGetValue(key, out HashSet<string>? connections))
            ? connections
			: Enumerable.Empty<string>();
	}

	/// <summary>
	///     Get the count of all connections.
	/// </summary>
	/// <return>A count</return>
	public int GetConnectionsCount()
	{
		return _connections.Values.Sum(values => values.Count);
	}

	/// <summary>
	///     Removes a connection for a given user.
	/// </summary>
	/// <param name="key">The key of the user.</param>
	/// <param name="connectionId">The SignalR connection ID.</param>
	/// <exception cref="ArgumentNullException">The key or connectionId parameter is null.</exception>
	public void Remove(T key, string connectionId)
	{
		ArgumentNullException.ThrowIfNull(key);
		ArgumentNullException.ThrowIfNull(connectionId);

		lock (_connections)
		{
			if (_connections.TryGetValue(key, out HashSet<string>? connections))
            {
                lock (connections)
				{
					connections.Remove(connectionId);

					if (connections.Count == 0)
						_connections.Remove(key);
				}
            }
        }
	}
}