using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Anodes.Repositories;

public interface IAnodeRepository : IRepositoryBaseEntity<Anode, DTOAnode>;