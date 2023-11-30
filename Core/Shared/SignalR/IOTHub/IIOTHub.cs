namespace Core.Shared.SignalR.IOTHub;

public interface IIOTHub : IBaseHub
{
	/// <summary>
	///     Refreshes IOTTags.
	/// </summary>
	Task RefreshIOTTag();

	Task RefreshDevices();
}