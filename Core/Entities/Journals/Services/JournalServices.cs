using Core.Entities.Alarmes_C.Models.DB;
using Core.Entities.AlarmesPLC.Models.DB;
using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Services;
using Core.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;

namespace Core.Entities.Journals.Services
{
    public class JournalServices : IJournalServices
    {
        private readonly AlarmesDbContext _AlarmesDbContext;
        private readonly IConfiguration _configuration;


        public JournalServices(AlarmesDbContext alarmesDbContext, IConfiguration configuration)
        {
            _AlarmesDbContext = alarmesDbContext;
            _configuration = configuration;

        }

        public async Task<Journal> addJournal(Journal journal)
        {         
            await _AlarmesDbContext.Journal.AddAsync(journal);
           _AlarmesDbContext.SaveChanges();
            return journal;
        }

     





        public async Task<IEnumerable<AlarmePLC>> Collect()
        {
            var appSettingsSection = _configuration.GetSection("stationConfig");

            var AllalarmsPLC = _AlarmesDbContext.AlarmePLC.ToList();

            for (int i =0; i < AllalarmsPLC.Count; i++)
            {
          
                var AlarmWithStatus1 = _AlarmesDbContext.Journal
                 .Where(alarme => alarme.Status1 == 1 && alarme.Status0 != 0 && alarme.IdAlarme == AllalarmsPLC[i].IdAlarme)
                 .OrderByDescending(j => j.Id)
                 .FirstOrDefault();
              

                if (AlarmWithStatus1 != null)
                {
                    AlarmWithStatus1.Station = appSettingsSection["nameStation"];
                    AlarmWithStatus1.Status0 = AllalarmsPLC[i].Status;
                    AlarmWithStatus1.TS0 = AllalarmsPLC[i].TS;
                    AlarmWithStatus1.TS = DateTime.Now;
                    AlarmWithStatus1.Lu = 0;
                    _AlarmesDbContext.SaveChanges();

                }
                else 
                {                  

                    Journal newJournal = new Journal();
                    newJournal.Station = appSettingsSection["nameStation"];
                    newJournal.IdAlarme = AllalarmsPLC[i].IdAlarme;
                    newJournal.Status1 = AllalarmsPLC[i].Status;
                    newJournal.TS1 = AllalarmsPLC[i].TS;
                    newJournal.TS = DateTime.Now;

                    _AlarmesDbContext.Journal.Add(newJournal);
                    _AlarmesDbContext.SaveChanges();
                }
                _AlarmesDbContext.AlarmePLC.Remove(AllalarmsPLC[i]);
                _AlarmesDbContext.SaveChanges();

                
            }




             return (IEnumerable<AlarmePLC>)AllalarmsPLC;
        }



        public async Task<IEnumerable<AlarmePLC>> AddJournalFromPush(Journal journal)
        {
            var appSettingsSection = _configuration.GetSection("stationConfig");

            var AllJournal = _AlarmesDbContext.Journal.ToList();

            if (AllJournal.Count == 0)
            {
                Journal newJournal = new Journal();
                newJournal.Station = journal.Station;
                newJournal.IdAlarme = journal.IdAlarme;
                newJournal.Status1 = journal.Status1;
                newJournal.TS1 = journal.TS;
                _AlarmesDbContext.Journal.Add(newJournal);
                _AlarmesDbContext.SaveChanges();
            }
          

                var AlarmWithStatus1 = _AlarmesDbContext.Journal
                 .Where(alarme => alarme.Status1 == 1 && alarme.Status0 != 0 && alarme.IdAlarme == journal.IdAlarme)
                 .OrderByDescending(j => j.Id)
                 .FirstOrDefault();

                if (AlarmWithStatus1 != null)
                {
                    AlarmWithStatus1.Station = journal.Station;
                    AlarmWithStatus1.Status0 = journal.Status0;
                AlarmWithStatus1.TS0 = journal.TS0;
                AlarmWithStatus1.Lu = 0;

                _AlarmesDbContext.SaveChanges();

                }
                else
                {

                    Journal newJournal = new Journal();
                    newJournal.Station = journal.Station;
                    newJournal.IdAlarme = journal.IdAlarme;
                    newJournal.Status1 = journal.Status1;
                    newJournal.TS1 = journal.TS;
                 

                    _AlarmesDbContext.Journal.Add(newJournal);
                    _AlarmesDbContext.SaveChanges();
                }
            
                _AlarmesDbContext.SaveChanges();

                     

            return (IEnumerable<AlarmePLC>)AllJournal;
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


        public async Task<Journal> LuJournal(int idJournal)
        {
            var JournalToRead = _AlarmesDbContext.Journal
               .Where(journal => journal.Id == idJournal && journal.Lu == 0)             
               .FirstOrDefault();
            JournalToRead.Lu = 1;

           /* int countNbLus = _AlarmesDbContext.Journal.Count(p => p.Lu == 0 && p.IdAlarme == JournalToRead.IdAlarme);
            if(countNbLus == 0)
            {
                var AlarmeToRead = _AlarmesDbContext.AlarmeTR.Where(p => p.IdAlarme == JournalToRead.IdAlarme).FirstOrDefault();
                AlarmeToRead.Lu = 1;
                _AlarmesDbContext.SaveChanges();
            }*/

            _AlarmesDbContext.SaveChanges();
            return JournalToRead;
        }



        public List<Journal> GetAllJournal()
        {
            var allJournals = _AlarmesDbContext.Journal.ToList();
            return allJournals;
        }

    }
}
