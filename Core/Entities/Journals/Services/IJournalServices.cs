using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTO;

namespace Core.Entities.Journals.Services;

public interface IJournalServices
{
    Task<DTOJournal> AddJournal(Journal journal);

    Task<IEnumerable<DTOAlarmPLC>> AddJournalFromPush(Journal journal);

    Task<IEnumerable<DTOAlarmPLC>> Collect();

    Task<int> CollectCyc(int nbSeconds);

    Task<List<DTOJournal>> GetAllJournal();


    Task<DTOJournal> ReadJournal(int idJournal);
}