using Core.Entities.Alarmes_C.Models.DB;
using Core.Entities.Alarmes_C.Models.DTOs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Alarmes_C.Repositories;

public class Alarme_CRepository: RepositoryBaseEntity<AlarmesDbContext, Alarme_C, DTOAlarme_C>, IAlarme_CRepository
{
    public Alarme_CRepository(AlarmesDbContext context) : base(context)
    {
    }
}