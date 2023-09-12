using Core.Entities.Journals.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Journals.Models.DTOs
{
    public partial class DTOJournal: DTOBaseEntity, IDTO<Journal, DTOJournal>
    {
        public DTOJournal()
        {
            TS = DateTime.Now;
            Lu = 0;
        }


        public DTOJournal(Journal journal)
        {
            ID = journal.ID;
            TS = journal.TS;
            TS0 = journal.TS0;
            TS1 = journal.TS1;
            Status0 = journal.Status0;
            Status1 = journal.Status1;
            IdAlarme= journal.IdAlarme;
            Alarme = journal.Alarme;
            Station = journal.Station;
            Lu = journal.Lu;
        }

        public Journal ToModel()
        {
            return new();
        }
    }
}
