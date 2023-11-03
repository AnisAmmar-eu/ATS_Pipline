﻿using System.Linq.Expressions;
using System.Text;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Core.Entities.Alarms.AlarmsLog.Services;

public class AlarmLogService : ServiceBaseEntity<IAlarmLogRepository, AlarmLog, DTOAlarmLog>, IAlarmLogService
{
	private readonly IHubContext<AlarmHub, IAlarmHub> _hubContext;


	public AlarmLogService(IAnodeUOW anodeUOW, IHubContext<AlarmHub, IAlarmHub> hubContext) : base(anodeUOW)
	{
		_hubContext = hubContext;
		//_myHub = myHub;
	}

	public async Task<IEnumerable<DTOAlarmPLC>> Collect()
	{
		List<AlarmPLC> allAlarmsPLC = await AnodeUOW.AlarmPLC.GetAll(withTracking: false);
		if (allAlarmsPLC.Count == 0) return Array.Empty<DTOAlarmPLC>();
		await AnodeUOW.StartTransaction();
		for (int index = 0; index < allAlarmsPLC.Count; index++)
		{
			try
			{
				int i = index; // Copied here because of try catch scoping to remove a warning.

				// If an active alarmLog already exists, this alarm is active and waiting to be cleared.
				AlarmLog alarmWithStatus1 = await AnodeUOW.AlarmLog.GetByWithIncludes(
					new Expression<Func<AlarmLog, bool>>[]
					{
						alarm => alarm.IsActive && alarm.AlarmID == allAlarmsPLC[i].AlarmID
					},
					query => query.OrderByDescending(alarmLog => alarmLog.ID));
				if (allAlarmsPLC[i].IsActive) continue; // alarmLog is already active.
				alarmWithStatus1.IsActive = false;
				alarmWithStatus1.TSClear = allAlarmsPLC[index].TS;
				alarmWithStatus1.TS = DateTime.Now;
				alarmWithStatus1.IsAck = false;
				alarmWithStatus1.HasBeenSent = false;
				AnodeUOW.AlarmLog.Update(alarmWithStatus1);
			}
			catch (EntityNotFoundException)
			{
				if (!allAlarmsPLC[index].IsActive) continue; // alarmLog is already inactive or cleared.

				// If an alarmLog doesn't exist, this alarm just raised.
				AlarmLog newAlarmLog = new(await AnodeUOW.AlarmC.GetById(allAlarmsPLC[index].AlarmID))
				{
					AlarmID = allAlarmsPLC[index].AlarmID,
					TS = DateTime.Now,
					HasBeenSent = false
				};
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
				await AnodeUOW.AlarmLog.Add(newAlarmLog);
			}

			AnodeUOW.AlarmPLC.Remove(allAlarmsPLC[index]);
			AnodeUOW.Commit();
		}

		// await  _myHub.RequestAlarmLogData();
		await AnodeUOW.CommitTransaction();
		await _hubContext.Clients.All.RefreshAlarmRT();
		await _hubContext.Clients.All.RefreshAlarmLog();
		return allAlarmsPLC.ConvertAll(alarmPLC => alarmPLC.ToDTO());
	}

	public async Task<List<DTOFAlarmLog>> AckAlarmLogs(int[] idAlarmLogs)
	{
		List<DTOFAlarmLog> ackAlarmLogs = new();
		await AnodeUOW.StartTransaction();
		foreach (int idAlarmLog in idAlarmLogs)
		{
			AlarmLog alarmLogToAck = await AnodeUOW.AlarmLog.GetByIdWithIncludes(idAlarmLog,
				new Expression<Func<AlarmLog, bool>>[]
				{
					alarmLog => !alarmLog.IsAck
				});
			alarmLogToAck.IsAck = true;
			alarmLogToAck.TSRead = DateTime.Now;
			ackAlarmLogs.Add(alarmLogToAck.ToDTOF());
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		await _hubContext.Clients.All.RefreshAlarmRT();
		await _hubContext.Clients.All.RefreshAlarmLog();
		return ackAlarmLogs;
	}


	public async Task<List<DTOFAlarmLog>> GetAllForFront()
	{
		List<AlarmLog> allAlarmLogs = await AnodeUOW.AlarmLog.GetAllWithIncludes();
		return allAlarmLogs.ConvertAll(alarmLog => alarmLog.ToDTOF());
	}

	public async Task<List<DTOFAlarmLog>> GetByClassID(int alarmID)
	{
		return (await AnodeUOW.AlarmLog.GetAllWithIncludes(new Expression<Func<AlarmLog, bool>>[]
		{
			alarmLog => alarmLog.AlarmID == alarmID
		})).ConvertAll(alarmLog => alarmLog.ToDTOF());
	}

	public async Task<HttpResponseMessage> SendLogsToServer()
	{
		const string api2Url = "https://localhost:7207/apiServerReceive/alarmsLog";
		List<AlarmLog> alarmLogs = await AnodeUOW.AlarmLog.GetAllWithIncludes(
			new Expression<Func<AlarmLog, bool>>[]
			{
				alarmLog => !alarmLog.HasBeenSent
			});
		string jsonData = JsonConvert.SerializeObject(alarmLogs.ConvertAll(alarmLog => alarmLog.ToDTOS()));
		StringContent content = new(jsonData, Encoding.UTF8, "application/json");

		using HttpClient httpClient = new();
		HttpResponseMessage response = await httpClient.PostAsync(api2Url, content);

		if (!response.IsSuccessStatusCode || !alarmLogs.Any()) return response;

		await AnodeUOW.StartTransaction();
		alarmLogs.ForEach(alarmLog =>
		{
			alarmLog.HasBeenSent = true;
			AnodeUOW.AlarmLog.Update(alarmLog);
		});
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();

		return response;
	}
}