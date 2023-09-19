namespace Core.Shared.SignalR
{
	public interface IBaseHub
	{
		/// <summary>
		/// Connect to the hub
		/// </summary>
		/// <returns></returns>
		Task OnConnectedAsync();
		/// <summary>
		/// Disconnect from the hub
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		Task OnDisconnectedAsync(Exception? exception);
	}
}
