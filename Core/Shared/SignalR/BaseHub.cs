using Core.Shared.Models.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Core.Shared.SignalR;

public class BaseHub<T> : Hub<T> where T : class
{
	/// <summary>
	///     User connections
	/// </summary>
	public static readonly UserConnectionManager<string> Connections = new();

	/// <summary>
	///     Connect to the hub
	/// </summary>
	public override Task OnConnectedAsync()
	{
		string? id = Context.User?.Claims.Where(x => x.Type == "Id").Select(c => c.Value).FirstOrDefault();

		if (id is null)
			return Task.CompletedTask;

		Connections.Add(id, Context.ConnectionId);

		base.OnConnectedAsync();

		return Task.CompletedTask;
	}

	/// <summary>
	///     Disconnect from the hub
	/// </summary>
	/// <param name="exception"></param>
	public override Task OnDisconnectedAsync(Exception? exception)
	{
		string? id = Context.User?.Claims.Where(x => x.Type == "Id").Select(c => c.Value).FirstOrDefault();

		if (id is null)
			return Task.CompletedTask;

		Connections.Remove(id, Context.ConnectionId);

		base.OnDisconnectedAsync(exception);

		return Task.CompletedTask;
	}
}