using System.Linq.Expressions;
using System.Text;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Services;

public class AlarmLogService : IAlarmLogService
{
	private readonly IAlarmUOW _alarmUOW;
	private readonly IConfiguration _configuration;


	public AlarmLogService(IAlarmUOW alarmUOW, IConfiguration configuration)
	{
		_configuration = configuration;
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
		return allAlarmsPLC.ConvertAll(alarmPLC => alarmPLC.ToDTO());
	}


	/*
	public async Task<IEnumerable<DTOAlarmPLC>> AddAlarmLogFromPush(AlarmLog alarmLog)
	{
		var appSettingsSection = _configuration.GetSection("stationConfig");

		var allAlarmLogs = await _alarmUOW.AlarmLog.GetAll();
		await _alarmUOW.StartTransaction();

		if (allAlarmLogs.Count == 0)
		{
			var newAlarmLog = new AlarmLog();
			newAlarmLog.Station = alarmLog.Station;
			newAlarmLog.IDAlarm = alarmLog.IDAlarm;
			newAlarmLog.Status1 = alarmLog.Status1;
			newAlarmLog.TS1 = alarmLog.TS;
			await _alarmUOW.AlarmLog.Add(newAlarmLog);
		}

		try
		{
			var alarmWithStatus1 = await _alarmUOW.AlarmLog.GetBy(new Expression<Func<AlarmLog, bool>>[]
			{
				alarm => alarm.Status1 == 1 && alarm.Status0 != 0 && alarm.IDAlarm == alarmLog.IDAlarm
			}, query => query.OrderByDescending(j => j.ID));

			alarmWithStatus1.Station = alarmLog.Station;
			alarmWithStatus1.Status0 = alarmLog.Status0;
			alarmWithStatus1.TS0 = alarmLog.TS0;
			alarmWithStatus1.IsRead = 0;
		}
		catch (EntityNotFoundException)
		{
			var newAlarmLog = new AlarmLog();
			newAlarmLog.Station = alarmLog.Station;
			newAlarmLog.IDAlarm = alarmLog.IDAlarm;
			newAlarmLog.Status1 = alarmLog.Status1;
			newAlarmLog.TS1 = alarmLog.TS;
			await _alarmUOW.AlarmLog.Add(newAlarmLog);
		}

		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return (IEnumerable<DTOAlarmPLC>)allAlarmLogs;
	}
	*/


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


	public async Task<DTOAlarmLog> ReadAlarmLog(int idAlarmLog)
	{
		var alarmLogToRead = await _alarmUOW.AlarmLog.GetByIdWithIncludes(idAlarmLog,
			new Expression<Func<AlarmLog, bool>>[]
			{
				alarmLog => !alarmLog.IsAck
			});
		await _alarmUOW.StartTransaction();
		alarmLogToRead.IsAck = true;
		alarmLogToRead.TSRead = DateTime.Now;

		/* int countNbLus = _AlarmesDbContext.AlarmLog.Count(p => p.Lu == 0 && p.IdAlarme == JournalToRead.IdAlarme);
		 if(countNbLus == 0)
		 {
		     var AlarmeToRead = _AlarmesDbContext.AlarmeTR.Where(p => p.IdAlarme == AlarmLogToRead.IdAlarme).FirstOrDefault();
		     AlarmeToRead.Lu = 1;
		     _AlarmesDbContext.SaveChanges();
		 }*/

		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return alarmLogToRead.ToDTO();
	}


	public async Task<List<DTOAlarmLog>> GetAllAlarmLog()
	{
		var allAlarmLogs = await _alarmUOW.AlarmLog.GetAllWithIncludes();
		return allAlarmLogs.ConvertAll(alarmLog => alarmLog.ToDTO());
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