using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Core.Shared.SignalR;

public interface ISignalRService
{
	T ExceptCaller<THub, T>(HttpContext? httpContext, IHubContext<THub, T> hubContext)
		where THub : Hub<T>
		where T : class;
}