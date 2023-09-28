using Core.Shared.SignalR;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
namespace Core.Entities.Alarms.AlarmsRT.Services;

public class AlarmRTService : IAlarmRTService
{
	private readonly IAlarmUOW _alarmUOW;

	public AlarmRTService(IAlarmUOW alarmUOW, ISignalRService signalRService, IHttpContextAccessor httpContextAccessor, IHubContext<AlarmHub, IAlarmHub> hubContext)
	{
		_alarmUOW = alarmUOW;
	}

	public async Task<List<DTOAlarmRT>> GetAll()
	{
		return (await _alarmUOW.AlarmRT.GetAllWithInclude()).ConvertAll(alarmRT => alarmRT.ToDTO());
	}
}