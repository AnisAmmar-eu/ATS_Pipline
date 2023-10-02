using System.Linq.Expressions;
using System.Text;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.SignalR;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.Entities.Alarms.AlarmsLog.Services;

public class AlarmLogService : ServiceBaseEntity<IAlarmLogRepository, AlarmLog, DTOAlarmLog>, IAlarmLogService
{
	private readonly IConfiguration _configuration;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IHubContext<AlarmHub, IAlarmHub> _hubContext;
	private readonly ISignalRService _signalRService;


	public AlarmLogService(IAlarmUOW alarmUOW, IConfiguration configuration, ISignalRService signalRService,
		IHttpContextAccessor httpContextAccessor, IHubContext<AlarmHub, IAlarmHub> hubContext) : base(alarmUOW)
	{
		_configuration = configuration;
		_signalRService = signalRService;
		_httpContextAccessor = httpContextAccessor;
		_hubContext = hubContext;
		//_myHub = myHub;
	}

	public async Task<IEnumerable<DTOAlarmPLC>> Collect()
	{
		IConfigurationSection? appSettingsSection = _configuration.GetSection("stationConfig");

		List<AlarmPLC> allAlarmsPLC = await _alarmUOW.AlarmPLC.GetAll(withTracking: false);
		if (allAlarmsPLC.Count == 0) return Array.Empty<DTOAlarmPLC>();
		await _alarmUOW.StartTransaction();
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
				_alarmUOW.Commit();
			}
			catch (EntityNotFoundException)
			{
				if (!allAlarmsPLC[index].IsActive) continue; // alarmLog is already inactive or cleared.

				// If an alarmLog doesn't exist, this alarm just raised.
				AlarmLog newAlarmLog = new(await _alarmUOW.AlarmC.GetById(allAlarmsPLC[index].AlarmID));
				newAlarmLog.Station = appSettingsSection["nameStation"];
				newAlarmLog.AlarmID = allAlarmsPLC[index].AlarmID;
				newAlarmLog.TS = DateTime.Now;
				newAlarmLog.HasBeenSent = false;
				if (allAlarmsPLC[index].IsOneShot)
				{
					newAlarmLog.IsActive = false;
					newAlarmLog.TSClear = allAlarmsPLC[index].TS;
				}
				else
				{
					newAlarmLog.IsActive = true;
				}

				newAlarmLog.TSRaised = allAlarmsPLC[index].TS;
				await _alarmUOW.AlarmLog.Add(newAlarmLog);
				_alarmUOW.Commit();
			}

			_alarmUOW.AlarmPLC.Remove(allAlarmsPLC[index]);
			_alarmUOW.Commit();
		}

		// await  _myHub.RequestAlarmLogData();
		await _alarmUOW.CommitTransaction();
		await _hubContext.Clients.All.RefreshAlarmRT();
		await _hubContext.Clients.All.RefreshAlarmLog();
		return allAlarmsPLC.ConvertAll(alarmPLC => alarmPLC.ToDTO());
	}

	public async Task<int> CollectCyc(int nbSeconds)
	{
		int nbCyc = 0;
		DateTime startTime = DateTime.Now;
		TimeSpan duration = TimeSpan.FromSeconds(nbSeconds);

		while (DateTime.Now - startTime < duration)
		{
			nbCyc++;
			await Collect();
		}

		return nbCyc;
	}


	public async Task<List<DTOFAlarmLog>> AckAlarmLogs(int[] idAlarmLogs)
	{
		List<DTOFAlarmLog> ackAlarmLogs = new();
		await _alarmUOW.StartTransaction();
		foreach (int idAlarmLog in idAlarmLogs)
		{
			AlarmLog alarmLogToAck = await _alarmUOW.AlarmLog.GetByIdWithIncludes(idAlarmLog,
				new Expression<Func<AlarmLog, bool>>[]
				{
					alarmLog => !alarmLog.IsAck
				});
			alarmLogToAck.IsAck = true;
			alarmLogToAck.TSRead = DateTime.Now;
			ackAlarmLogs.Add(alarmLogToAck.ToDTOF());
			_alarmUOW.Commit();
		}

		await _alarmUOW.CommitTransaction();
		await _hubContext.Clients.All.RefreshAlarmRT();
		await _hubContext.Clients.All.RefreshAlarmLog();
		return ackAlarmLogs;
	}


	public async Task<List<DTOFAlarmLog>> GetAllForFront()
	{
		List<AlarmLog> allAlarmLogs = await _alarmUOW.AlarmLog.GetAllWithIncludes();
		return allAlarmLogs.ConvertAll(alarmLog => alarmLog.ToDTOF());
	}

	public async Task<List<DTOFAlarmLog>> GetByClassID(int alarmID)
	{
		return (await _alarmUOW.AlarmLog.GetAllWithIncludes(new Expression<Func<AlarmLog, bool>>[]
		{
			alarmLog => alarmLog.AlarmID == alarmID
		})).ConvertAll(alarmLog => alarmLog.ToDTOF());
	}

	public async Task<HttpResponseMessage> SendLogsToServer()
	{
		const string api2Url = "https://localhost:7207/api/receive/alarm-log";
		List<AlarmLog> alarmLogs = await _alarmUOW.AlarmLog.GetAllWithIncludes(
			new Expression<Func<AlarmLog, bool>>[]
			{
				alarmLog => !alarmLog.HasBeenSent
			});
		string jsonData = JsonConvert.SerializeObject(alarmLogs.ConvertAll(alarmLog => alarmLog.ToDTOS()));
		StringContent content = new(jsonData, Encoding.UTF8, "application/json");

		using (HttpClient httpClient = new())
		{
			HttpResponseMessage response = await httpClient.PostAsync(api2Url, content);

			if (!response.IsSuccessStatusCode) return response;

			await _alarmUOW.StartTransaction();
			alarmLogs.ForEach(alarmLog =>
			{
				alarmLog.HasBeenSent = true;
				_alarmUOW.AlarmLog.Update(alarmLog);
			});
			_alarmUOW.Commit();
			await _alarmUOW.CommitTransaction();

			return response;
		}
	}
}