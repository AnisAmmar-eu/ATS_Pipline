using Core.Entities.Alarmes_C.Models.DB;
using Core.Entities.AlarmesPLC.Models.DB;
using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Services;
using Core.Shared.Data;
using Core.Shared.signalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using Core.Entities.AlarmesPLC.Models.DTO;
using Core.Entities.Journals.Models.DTOs;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace Core.Entities.Journals.Services
{
    public class JournalServices : IJournalServices
    {
        private readonly IUnitOfWorkAlarm _unitOfWork;
        private readonly IConfiguration _configuration;


        public JournalServices(IUnitOfWorkAlarm unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            //_myHub = myHub;
        }

        public async Task<DTOJournal> addJournal(Journal journal)
        {
            await _unitOfWork.Journal.Add(journal);
            _unitOfWork.Commit();
            return journal.ToDTO();
        }


        public async Task<IEnumerable<DTOAlarmePLC>> Collect()
        {
            var appSettingsSection = _configuration.GetSection("stationConfig");

            await _unitOfWork.StartTransaction();
            List<AlarmePLC> AllalarmsPLC = await _unitOfWork.AlarmePLC.GetAll(withTracking: false);
            for (int i = 0; i < AllalarmsPLC.Count; i++)
            {
                try
                {
                    var AlarmWithStatus1 = await _unitOfWork.Journal.GetBy(
                        filters: new Expression<Func<Journal, bool>>[]
                        {
                            alarm => alarm.Status1 == 1 && alarm.Status0 != 0 &&
                                              alarm.IdAlarme == AllalarmsPLC[i].IdAlarme
                        },
                        orderBy: query => query.OrderByDescending(j => j.ID));
                    AlarmWithStatus1.Station = appSettingsSection["nameStation"];
                    AlarmWithStatus1.Status0 = AllalarmsPLC[i].Status;
                    AlarmWithStatus1.TS0 = AllalarmsPLC[i].TS;
                    AlarmWithStatus1.TS = DateTime.Now;
                    // AlarmWithStatus1.Lu = 0;
                    _unitOfWork.Journal.Update(AlarmWithStatus1);
                }
                catch (EntityNotFoundException e)
                {
                    Journal newJournal = new Journal();
                    newJournal.Station = appSettingsSection["nameStation"];
                    newJournal.IdAlarme = AllalarmsPLC[i].IdAlarme;
                    newJournal.Status1 = AllalarmsPLC[i].Status;
                    newJournal.TS1 = AllalarmsPLC[i].TS;
                    newJournal.TS = DateTime.Now;
                    await _unitOfWork.Journal.Add(newJournal);
                }
                _unitOfWork.AlarmePLC.Remove(AllalarmsPLC[i]);
            }
            //  await  _myHub.RequestJournalData();
            _unitOfWork.Commit();
            await _unitOfWork.CommitTransaction();
            return AllalarmsPLC.ConvertAll(alarmePLC => alarmePLC.ToDTO());
        }


        public async Task<IEnumerable<DTOAlarmePLC>> AddJournalFromPush(Journal journal)
        {
            var appSettingsSection = _configuration.GetSection("stationConfig");

            var AllJournal = await _unitOfWork.Journal.GetAll();
            await _unitOfWork.StartTransaction();

            if (AllJournal.Count == 0)
            {
                Journal newJournal = new Journal();
                newJournal.Station = journal.Station;
                newJournal.IdAlarme = journal.IdAlarme;
                newJournal.Status1 = journal.Status1;
                newJournal.TS1 = journal.TS;
                await _unitOfWork.Journal.Add(newJournal);
            }

            var AlarmWithStatus1 = await _unitOfWork.Journal.GetBy(filters: new Expression<Func<Journal, bool>>[]
            {
                alarm => alarm.Status1 == 1 && alarm.Status0 != 0 && alarm.IdAlarme == journal.IdAlarme
            }, orderBy: query => query.OrderByDescending(j => j.ID));

            if (AlarmWithStatus1 != null)
            {
                AlarmWithStatus1.Station = journal.Station;
                AlarmWithStatus1.Status0 = journal.Status0;
                AlarmWithStatus1.TS0 = journal.TS0;
                AlarmWithStatus1.Lu = 0;
            }
            else
            {
                Journal newJournal = new Journal();
                newJournal.Station = journal.Station;
                newJournal.IdAlarme = journal.IdAlarme;
                newJournal.Status1 = journal.Status1;
                newJournal.TS1 = journal.TS;
                await _unitOfWork.Journal.Add(newJournal);
            }

            _unitOfWork.Commit();
            await _unitOfWork.CommitTransaction();
            return (IEnumerable<DTOAlarmePLC>)AllJournal;
        }


        public async Task<int> CollectCyc(int nbSeconde)
        {
            int nbCyc = 0;
            DateTime startTime = DateTime.Now;
            TimeSpan duration = TimeSpan.FromSeconds(nbSeconde);

            while (DateTime.Now - startTime < duration)
            {
                nbCyc++;
                await Collect();
            }

            return nbCyc;
        }


        public async Task<DTOJournal> LuJournal(int idJournal)
        {
            var JournalToRead = await _unitOfWork.Journal.GetById(idJournal, filters: new Expression<Func<Journal, bool>>[]
            {
                journal => journal.Lu == 0
            });
            await _unitOfWork.StartTransaction();
            JournalToRead.Lu = 1;
            JournalToRead.TSLu = DateTime.Now;

            /* int countNbLus = _AlarmesDbContext.Journal.Count(p => p.Lu == 0 && p.IdAlarme == JournalToRead.IdAlarme);
             if(countNbLus == 0)
             {
                 var AlarmeToRead = _AlarmesDbContext.AlarmeTR.Where(p => p.IdAlarme == JournalToRead.IdAlarme).FirstOrDefault();
                 AlarmeToRead.Lu = 1;
                 _AlarmesDbContext.SaveChanges();
             }*/

            _unitOfWork.Commit();
            await _unitOfWork.CommitTransaction();
            return JournalToRead.ToDTO();
        }


        public async Task<List<DTOJournal>> GetAllJournal()
        {
            var allJournals = await _unitOfWork.Journal.GetAll();
            return allJournals.ConvertAll(journal => journal.ToDTO());
        }
    }
}