namespace Core.Shared.SignalR.IOTTagHub;

public interface IIOTTagHub : IBaseHub
{
	/// <summary>
	///     Refreshes IOTTags.
	/// </summary>
	/// <returns></returns>
	Task RefreshIOTTag();
}