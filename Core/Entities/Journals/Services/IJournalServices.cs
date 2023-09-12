using Core.Entities.AlarmesPLC.Models.DB;
using Core.Entities.Journals.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.AlarmesPLC.Models.DTO;
using Core.Entities.Journals.Models.DTOs;

namespace Core.Entities.Journals.Services
{
   public interface IJournalServices
    {
        Task<DTOJournal> addJournal(Journal journal);

        Task<IEnumerable<DTOAlarmePLC>> AddJournalFromPush(Journal journal);

        Task<IEnumerable<DTOAlarmePLC>> Collect();

        Task<int> CollectCyc(int nbSeconde);

        Task<List<DTOJournal>> GetAllJournal();


        Task<DTOJournal> LuJournal(int idJournal);

    }
}
