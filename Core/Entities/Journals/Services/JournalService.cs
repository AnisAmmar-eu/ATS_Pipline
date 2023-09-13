using System.Linq.Expressions;
using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTO;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Entities.Journals.Services;

public class JournalService : IJournalService
{
    private readonly IAlarmUOW _alarmUOW;
    private readonly IConfiguration _configuration;


    public JournalService(IAlarmUOW alarmUOW, IConfiguration configuration)
    {
        _configuration = configuration;
        _alarmUOW = alarmUOW;
        //_myHub = myHub;
    }

    public async Task<DTOJournal> AddJournal(Journal journal)
    {
        await _alarmUOW.Journal.Add(journal);
        _alarmUOW.Commit();
        return journal.ToDTO();
    }


    public async Task<IEnumerable<DTOAlarmPLC>> Collect()
    {
        var appSettingsSection = _configuration.GetSection("stationConfig");

        await _alarmUOW.StartTransaction();
        var allAlarmsPLC = await _alarmUOW.AlarmPLC.GetAll(withTracking: false);
        for (var i = 0; i < allAlarmsPLC.Count; i++)
        {
            try
            {
                var alarmWithStatus1 = await _alarmUOW.Journal.GetBy(
                    new Expression<Func<Journal, bool>>[]
                    {
                        alarm => alarm.Status1 == 1 && alarm.Status0 != 0 &&
                                 alarm.IDAlarm == allAlarmsPLC[i].IDAlarm
                    },
                    query => query.OrderByDescending(journal => journal.ID));
                alarmWithStatus1.Station = appSettingsSection["nameStation"];
                alarmWithStatus1.Status0 = allAlarmsPLC[i].Status;
                alarmWithStatus1.TS0 = allAlarmsPLC[i].TS;
                alarmWithStatus1.TS = DateTime.Now;
                // AlarmWithStatus1.Lu = 0;
                _alarmUOW.Journal.Update(alarmWithStatus1);
            }
            catch (EntityNotFoundException)
            {
                var newJournal = new Journal();
                newJournal.Station = appSettingsSection["nameStation"];
                newJournal.IDAlarm = allAlarmsPLC[i].IDAlarm;
                newJournal.Status1 = allAlarmsPLC[i].Status;
                newJournal.TS1 = allAlarmsPLC[i].TS;
                newJournal.TS = DateTime.Now;
                await _alarmUOW.Journal.Add(newJournal);
            }

            _alarmUOW.AlarmPLC.Remove(allAlarmsPLC[i]);
        }

        // await  _myHub.RequestJournalData();
        _alarmUOW.Commit();
        await _alarmUOW.CommitTransaction();
        return allAlarmsPLC.ConvertAll(alarmPLC => alarmPLC.ToDTO());
    }


    public async Task<IEnumerable<DTOAlarmPLC>> AddJournalFromPush(Journal journal)
    {
        var appSettingsSection = _configuration.GetSection("stationConfig");

        var allJournals = await _alarmUOW.Journal.GetAll();
        await _alarmUOW.StartTransaction();

        if (allJournals.Count == 0)
        {
            var newJournal = new Journal();
            newJournal.Station = journal.Station;
            newJournal.IDAlarm = journal.IDAlarm;
            newJournal.Status1 = journal.Status1;
            newJournal.TS1 = journal.TS;
            await _alarmUOW.Journal.Add(newJournal);
        }

        try
        {
            var alarmWithStatus1 = await _alarmUOW.Journal.GetBy(new Expression<Func<Journal, bool>>[]
            {
                alarm => alarm.Status1 == 1 && alarm.Status0 != 0 && alarm.IDAlarm == journal.IDAlarm
            }, query => query.OrderByDescending(j => j.ID));

            alarmWithStatus1.Station = journal.Station;
            alarmWithStatus1.Status0 = journal.Status0;
            alarmWithStatus1.TS0 = journal.TS0;
            alarmWithStatus1.IsRead = 0;
        }
        catch (EntityNotFoundException)
        {
            var newJournal = new Journal();
            newJournal.Station = journal.Station;
            newJournal.IDAlarm = journal.IDAlarm;
            newJournal.Status1 = journal.Status1;
            newJournal.TS1 = journal.TS;
            await _alarmUOW.Journal.Add(newJournal);
        }

        _alarmUOW.Commit();
        await _alarmUOW.CommitTransaction();
        return (IEnumerable<DTOAlarmPLC>)allJournals;
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


    public async Task<DTOJournal> ReadJournal(int idJournal)
    {
        var journalToRead = await _alarmUOW.Journal.GetById(idJournal,
            new Expression<Func<Journal, bool>>[]
            {
                journal => journal.IsRead == 0
            });
        await _alarmUOW.StartTransaction();
        journalToRead.IsRead = 1;
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


    public async Task<List<DTOJournal>> GetAllJournal()
    {
        var allJournals = await _alarmUOW.Journal.GetAll();
        return allJournals.ConvertAll(journal => journal.ToDTO());
    }
}