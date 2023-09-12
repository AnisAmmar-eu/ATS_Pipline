using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTOs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Journals.Repositories;

public class JournalRepository : RepositoryBaseEntity<AlarmesDbContext, Journal, DTOJournal>, IJournalRepository
{
   public JournalRepository(AlarmesDbContext context) : base(context)
   {
   }
}