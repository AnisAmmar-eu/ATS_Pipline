using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Journals.Repositories;

public interface IJournalRepository : IRepositoryBaseEntity<Journal, DTOJournal>
{
}