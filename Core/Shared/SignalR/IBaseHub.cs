namespace Core.Shared.SignalR;

public interface IBaseHub
{
	/// <summary>
	///     Connect to the hub
	/// </summary>
	Task OnConnectedAsync();

	/// <summary>
	///     Disconnect from the hub
	/// </summary>
	/// <param name="exception"></param>
	Task OnDisconnectedAsync(Exception? exception);
}