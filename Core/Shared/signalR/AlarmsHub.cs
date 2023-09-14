using Core.Shared.Data;
using Microsoft.AspNetCore.SignalR;

namespace Core.Shared.signalR;

public class AlarmsHub : Hub
{
	private readonly AlarmCTX _alarmCtx;


	public AlarmsHub(AlarmCTX alarmCtx)
	{
		_alarmCtx = alarmCtx;
	}


	public async Task RequestJournalData()
	{
		var journalData = _alarmCtx.AlarmLog.ToList();
		await Clients.All.SendAsync("ReceiveAlarm", journalData);
	}
}