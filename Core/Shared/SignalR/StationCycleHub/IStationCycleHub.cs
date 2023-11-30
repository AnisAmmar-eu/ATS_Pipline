namespace Core.Shared.SignalR.StationCycleHub;

public interface IStationCycleHub : IBaseHub
{
	/// <summary>
	///     Refreshes StationCycles
	/// </summary>
	Task RefreshStationCycle();
}