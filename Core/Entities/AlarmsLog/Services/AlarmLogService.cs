using System.Linq.Expressions;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
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

	public async Task<DTOAlarmLog> AddJournal(AlarmLog alarmLog)
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
				AlarmLog alarmWithStatus1 = await _alarmUOW.AlarmLog.GetBy(
					new Expression<Func<AlarmLog, bool>>[]
					{
						alarm => alarm.IsActive && alarm.AlarmID == allAlarmsPLC[i].AlarmID
					},
					query => query.OrderByDescending(journal => journal.ID));
				if (allAlarmsPLC[i].IsActive) continue; // alarmLog is already active.
				alarmWithStatus1.Station = appSettingsSection["nameStation"];
				alarmWithStatus1.IsActive = false;
				alarmWithStatus1.TSClear = allAlarmsPLC[index].TS;
				alarmWithStatus1.TS = DateTime.Now;
				alarmWithStatus1.IsAck = false;
				_alarmUOW.AlarmLog.Update(alarmWithStatus1);
			}
			catch (EntityNotFoundException)
			{
				if (!allAlarmsPLC[index].IsActive) continue; // alarmLog is already inactive or cleared.
				
				// If an alarmLog doesn't exist, this alarm just raised.
				AlarmLog newJournal = new AlarmLog(await _alarmUOW.AlarmC.GetById(allAlarmsPLC[index].AlarmID));
				newJournal.Station = appSettingsSection["nameStation"];
				newJournal.AlarmID = allAlarmsPLC[index].AlarmID;
				newJournal.IsActive = true;
				newJournal.TSRaised = allAlarmsPLC[index].TS;
				newJournal.TS = DateTime.Now;
				await _alarmUOW.AlarmLog.Add(newJournal);
			}

			_alarmUOW.AlarmPLC.Remove(allAlarmsPLC[index]);
		}

		// await  _myHub.RequestJournalData();
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return allAlarmsPLC.ConvertAll(alarmPLC => alarmPLC.ToDTO());
	}


	/*
	public async Task<IEnumerable<DTOAlarmPLC>> AddJournalFromPush(AlarmLog alarmLog)
	{
		var appSettingsSection = _configuration.GetSection("stationConfig");

		var allJournals = await _alarmUOW.AlarmLog.GetAll();
		await _alarmUOW.StartTransaction();

		if (allJournals.Count == 0)
		{
			var newJournal = new AlarmLog();
			newJournal.Station = alarmLog.Station;
			newJournal.IDAlarm = alarmLog.IDAlarm;
			newJournal.Status1 = alarmLog.Status1;
			newJournal.TS1 = alarmLog.TS;
			await _alarmUOW.AlarmLog.Add(newJournal);
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
			var newJournal = new AlarmLog();
			newJournal.Station = alarmLog.Station;
			newJournal.IDAlarm = alarmLog.IDAlarm;
			newJournal.Status1 = alarmLog.Status1;
			newJournal.TS1 = alarmLog.TS;
			await _alarmUOW.AlarmLog.Add(newJournal);
		}

		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return (IEnumerable<DTOAlarmPLC>)allJournals;
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


	public async Task<DTOAlarmLog> ReadAlarmLog(int idJournal)
	{
		var journalToRead = await _alarmUOW.AlarmLog.GetById(idJournal,
			new Expression<Func<AlarmLog, bool>>[]
			{
				journal => !journal.IsAck
			});
		await _alarmUOW.StartTransaction();
		journalToRead.IsAck = true;
		journalToRead.TSRead = DateTime.Now;

		/* int countNbLus = _AlarmesDbContext.Journal.Count(p => p.Lu == 0 && p.IdAlarme == JournalToRead.IdAlarme);
		 if(countNbLus == 0)
		 {
		     var AlarmeToRead = _AlarmesDbContext.AlarmeTR.Where(p => p.IdAlarme == JournalToRead.IdAlarme).FirstOrDefault();
		     AlarmeToRead.Lu = 1;
		     _AlarmesDbContext.SaveChanges();
		 }*/

		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return journalToRead.ToDTO();
	}


	public async Task<List<DTOAlarmLog>> GetAllAlarmLog()
	{
		var allJournals = await _alarmUOW.AlarmLog.GetAll();
		return allJournals.ConvertAll(journal => journal.ToDTO());
	}
}