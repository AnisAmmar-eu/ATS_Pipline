using System.Linq.Expressions;
using System.Text;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Shared.Exceptions;
using Core.Shared.SignalR;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Services;

public class AlarmLogService : IAlarmLogService
{
	private readonly IAlarmUOW _alarmUOW;
	private readonly IConfiguration _configuration;
	private readonly ISignalRService _signalRService;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IHubContext<AlarmHub, IAlarmHub> _hubContext;


	public AlarmLogService(IAlarmUOW alarmUOW, IConfiguration configuration, ISignalRService signalRService,
		IHttpContextAccessor httpContextAccessor, IHubContext<AlarmHub, IAlarmHub> hubContext)
	{
		_configuration = configuration;
		_signalRService = signalRService;
		_httpContextAccessor = httpContextAccessor;
		_hubContext = hubContext;
		_alarmUOW = alarmUOW;
		//_myHub = myHub;
	}

	public async Task<DTOAlarmLog> AddAlarmLog(AlarmLog alarmLog)
	{
		await _alarmUOW.AlarmLog.Add(alarmLog);
		_alarmUOW.Commit();
		return alarmLog.ToDTO();
	}

	public async Task<IEnumerable<DTOAlarmPLC>> Collect()
	{
		var appSettingsSection = _configuration.GetSection("stationConfig");

		await _alarmUOW.StartTransaction();
		List<AlarmPLC> allAlarmsPLC = await _alarmUOW.AlarmPLC.GetAll(withTracking: false);
		for (int index = 0; index < allAlarmsPLC.Count; index++)
		{
			try
			{
				int i = index; // Copied here because of try catch scoping to remove a warning.

				// If an active alarmLog already exists, this alarm is active and waiting to be cleared.
				AlarmLog alarmWithStatus1 = await _alarmUOW.AlarmLog.GetByWithIncludes(
					new Expression<Func<AlarmLog, bool>>[]
					{
						alarm => alarm.IsActive && alarm.AlarmID == allAlarmsPLC[i].AlarmID
					},
					query => query.OrderByDescending(alarmLog => alarmLog.ID));
				if (allAlarmsPLC[i].IsActive) continue; // alarmLog is already active.
				alarmWithStatus1.Station = appSettingsSection["nameStation"];
				alarmWithStatus1.IsActive = false;
				alarmWithStatus1.TSClear = allAlarmsPLC[index].TS;
				alarmWithStatus1.TS = DateTime.Now;
				alarmWithStatus1.IsAck = false;
				alarmWithStatus1.HasBeenSent = false;
				_alarmUOW.AlarmLog.Update(alarmWithStatus1);
			}
			catch (EntityNotFoundException)
			{
				if (!allAlarmsPLC[index].IsActive) continue; // alarmLog is already inactive or cleared.

				// If an alarmLog doesn't exist, this alarm just raised.
				AlarmLog newAlarmLog = new AlarmLog(await _alarmUOW.AlarmC.GetById(allAlarmsPLC[index].AlarmID));
				newAlarmLog.Station = appSettingsSection["nameStation"];
				newAlarmLog.AlarmID = allAlarmsPLC[index].AlarmID;
				newAlarmLog.IsActive = true;
				newAlarmLog.TSRaised = allAlarmsPLC[index].TS;
				newAlarmLog.TS = DateTime.Now;
				newAlarmLog.HasBeenSent = false;
				await _alarmUOW.AlarmLog.Add(newAlarmLog);
			}

			_alarmUOW.AlarmPLC.Remove(allAlarmsPLC[index]);
		}

		// await  _myHub.RequestAlarmLogData();
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		await _hubContext.Clients.All.RefreshAlarmRT();
		await _hubContext.Clients.All.RefreshAlarmLog();
		return allAlarmsPLC.ConvertAll(alarmPLC => alarmPLC.ToDTO());
	}

	public async Task<int> CollectCyc(int nbSeconds)
	{
		var nbCyc = 0;
		var startTime = DateTime.Now;
		var duration = TimeSpan.FromSeconds(nbSeconds);

		while (DateTime.Now - startTime < duration)
		{
			nbCyc++;
			await Collect();
		}

		return nbCyc;
	}


	public async Task<List<DTOAlarmLog>> AckAlarmLogs(int[] idAlarmLogs)
	{
		List<DTOAlarmLog> ackAlarmLogs = new List<DTOAlarmLog>();
		await _alarmUOW.StartTransaction();
		foreach (int idAlarmLog in idAlarmLogs)
		{
			var alarmLogToAck = await _alarmUOW.AlarmLog.GetByIdWithIncludes(idAlarmLog,
				new Expression<Func<AlarmLog, bool>>[]
				{
					alarmLog => !alarmLog.IsAck
				});
			alarmLogToAck.IsAck = true;
			alarmLogToAck.TSRead = DateTime.Now;
			ackAlarmLogs.Add(alarmLogToAck.ToDTO());
		}
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		await _hubContext.Clients.All.RefreshAlarmRT();
		await _hubContext.Clients.All.RefreshAlarmLog();
		return ackAlarmLogs;
	}


	public async Task<List<DTOAlarmLog>> GetAll()
	{
		var allAlarmLogs = await _alarmUOW.AlarmLog.GetAllWithIncludes();
		return allAlarmLogs.ConvertAll(alarmLog => alarmLog.ToDTO());
	}

	public async Task<List<DTOAlarmLog>> GetByClassID(int alarmID)
	{
		return (await _alarmUOW.AlarmLog.GetAllWithIncludes(filters: new Expression<Func<AlarmLog, bool>>[]
		{
			alarmLog => alarmLog.AlarmID == alarmID
		})).ConvertAll(alarmLog => alarmLog.ToDTO());
	}

	public async Task MarkLogsAsSent(List<DTOAlarmLog> dtoAlarmLogs)
	{
		await _alarmUOW.StartTransaction();
		dtoAlarmLogs.ForEach(dtoAlarmLog =>
		{
			dtoAlarmLog.HasBeenSent = true;
			_alarmUOW.AlarmLog.Update(dtoAlarmLog.ToModel());
		});
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
	}

	public async Task<HttpResponseMessage> SendLogsToServer()
	{
		var api2Url = "https://localhost:7207/api/Receive/endpoint";
		var alarmLogs = await _alarmUOW.AlarmLog.GetAllWithIncludes(filters: new Expression<Func<AlarmLog, bool>>[]
		{
			alarmLog => !alarmLog.HasBeenSent
		});
		var jsonData = JsonConvert.SerializeObject(alarmLogs.ConvertAll(alarmLog => alarmLog.ToDTO()));
		var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

		using (var httpClient = new HttpClient())
		{
			var response = await httpClient.PostAsync(api2Url, content);
			if (response.IsSuccessStatusCode)
			{
				await _alarmUOW.StartTransaction();
				alarmLogs.ForEach(alarmLog =>
				{
					alarmLog.HasBeenSent = true;
					_alarmUOW.AlarmLog.Update(alarmLog);
				});
				_alarmUOW.Commit();
				await _alarmUOW.CommitTransaction();
			}

			return response;
		}
	}
}