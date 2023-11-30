namespace Core.Shared.SignalR.AlarmHub;

public interface IAlarmHub : IBaseHub
{
	/// <summary>
	///     Refreshes real time alarms.
	/// </summary>
	Task RefreshAlarmRT();

	/// <summary>
	///     Refreshes alarm logs.
	/// </summary>
	Task RefreshAlarmLog();
}