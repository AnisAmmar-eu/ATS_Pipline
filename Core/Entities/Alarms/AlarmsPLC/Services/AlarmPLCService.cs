using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Alarms.AlarmsPLC.Services;

public class AlarmPLCService : IAlarmPLCService
{
	private readonly IAlarmUOW _alarmUOW;

	public AlarmPLCService(IAlarmUOW alarmUOW)
	{
		_alarmUOW = alarmUOW;
	}

	public async Task AddAlarmPLC(AlarmPLC alarmPLC)
	{
		await _alarmUOW.StartTransaction();
		await _alarmUOW.AlarmPLC.Add(alarmPLC);
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
	}
}