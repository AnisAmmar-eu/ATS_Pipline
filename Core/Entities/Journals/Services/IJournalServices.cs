using Core.Entities.AlarmesPLC.Models.DB;
using Core.Entities.Journals.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Journals.Services
{
   public interface IJournalServices
    {
        Task<Journal> addJournal(Journal journal);
        Task<IEnumerable<AlarmePLC>> AddJournalFromPush(Journal journal);

        Task<IEnumerable<AlarmePLC>> Collect();

        Task<int> CollectCyc(int nbSeconde);

        List<Journal> GetAllJournal();

        Task<Journal> LuJournal(int idJournal);

    }
}
