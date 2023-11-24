using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Anodes.Services;

public interface IAnodeService : IBaseEntityService<Anode, DTOAnode>;