namespace Core.Shared.SignalR.IOTTagHub;

public interface IIOTTagHub : IBaseHub
{
	/// <summary>
	///     Refreshes real time alarms.
	/// </summary>
	/// <returns></returns>
	Task RefreshIOTTag();
}