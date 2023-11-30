using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Core.Shared.SignalR;

public class SignalRService : ISignalRService
{
	public T ExceptCaller<THub, T>(HttpContext? httpContext, IHubContext<THub, T> hubContext)
		where THub : Hub<T>
		where T : class
	{
		if (httpContext is null)
			return hubContext.Clients.All;

		string? connectionsId = httpContext.Request.Headers["x-connections-signalr"];
		if (connectionsId is null)
			return hubContext.Clients.All;

		string[] callerConnections = connectionsId.Split(',');
		return hubContext.Clients.AllExcept(callerConnections);
	}
}