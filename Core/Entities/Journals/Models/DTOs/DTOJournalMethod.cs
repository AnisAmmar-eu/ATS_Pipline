using Core.Entities.Journals.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Journals.Models.DTOs
{
    public partial class DTOJournal
    {
        public DTOJournal()
        {
            TS = DateTime.Now;
            Lu = 0;
        }


        public DTOJournal(Journal journal)
        {
            TS = journal.TS;
            TS0 = journal.TS0;
            TS1 = journal.TS1;
            Status0 = journal.Status0;
            Status1 = journal.Status1;
            IdAlarme= journal.IdAlarme;


        }
    }
}
