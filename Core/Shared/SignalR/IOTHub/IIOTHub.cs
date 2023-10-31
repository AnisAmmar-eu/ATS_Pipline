namespace Core.Shared.SignalR.IOTHub;

public interface IIOTHub : IBaseHub
{
	/// <summary>
	///     Refreshes IOTTags.
	/// </summary>
	/// <returns></returns>
	Task RefreshIOTTag();

	Task RefreshDevices();
}