using Core.Shared.Models.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Core.Shared.SignalR;

[Authorize]
public class BaseHub<T> : Hub<T> where T : class
{
	/// <summary>
	///     User connections
	/// </summary>
	public static readonly UserConnectionManager<string> Connections = new();


	/// <summary>
	///     Connect to the hub
	/// </summary>
	/// <returns></returns>
	public override Task OnConnectedAsync()
	{
		string? id = Context.User?.Claims.Where(x => x.Type == "Id").Select(c => c.Value).FirstOrDefault();

		if (id == null)
			return Task.CompletedTask;

		Connections.Add(id, Context.ConnectionId);

		base.OnConnectedAsync();

		return Task.CompletedTask;
	}

	/// <summary>
	///     Disconnect from the hub
	/// </summary>
	/// <param name="exception"></param>
	/// <returns></returns>
	public override Task OnDisconnectedAsync(Exception? exception)
	{
		string? id = Context.User?.Claims.Where(x => x.Type == "Id").Select(c => c.Value).FirstOrDefault();

		if (id == null)
			return Task.CompletedTask;

		Connections.Remove(id, Context.ConnectionId);

		base.OnDisconnectedAsync(exception);

		return Task.CompletedTask;
	}
}