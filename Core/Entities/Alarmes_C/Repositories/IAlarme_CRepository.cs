using Core.Entities.Alarmes_C.Models.DB;
using Core.Entities.Alarmes_C.Models.DTOs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Alarmes_C.Repositories;

public interface IAlarme_CRepository: IRepositoryBaseEntity<Alarme_C, DTOAlarme_C>
{
}