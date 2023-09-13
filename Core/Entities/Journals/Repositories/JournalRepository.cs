using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Journals.Repositories;

public class JournalRepository : RepositoryBaseEntity<AlarmCTX, Journal, DTOJournal>, IJournalRepository
{
    public JournalRepository(AlarmCTX context) : base(context)
    {
    }
}