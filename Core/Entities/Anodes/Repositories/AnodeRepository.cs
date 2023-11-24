using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Anodes.Repositories;

public class AnodeRepository : BaseEntityRepository<AnodeCTX, Anode, DTOAnode>, IAnodeRepository
{
	public AnodeRepository(AnodeCTX context) : base(context)
	{
	}
}