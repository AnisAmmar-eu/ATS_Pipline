using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Repositories.ToMatchs;

public interface IToMatchRepository : IBaseEntityRepository<ToMatch, DTOToMatch>
{
}