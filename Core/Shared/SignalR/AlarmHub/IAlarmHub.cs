namespace Core.Shared.SignalR.AlarmHub;

public interface IAlarmHub : IBaseHub
{
	/// <summary>
	///     Refreshes real time alarms.
	/// </summary>
	/// <returns></returns>
	Task RefreshAlarmRT();

	/// <summary>
	///     Refreshes alarm logs.
	/// </summary>
	/// <returns></returns>
	Task RefreshAlarmLog();
}