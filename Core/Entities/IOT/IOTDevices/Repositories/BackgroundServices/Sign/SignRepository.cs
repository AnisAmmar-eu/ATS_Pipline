using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;
using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Signs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.IOT.IOTDevices.Repositories;

public class SignRepository : BaseEntityRepository<AnodeCTX, Sign, DTOSign>, ISignRepository
{
	public SignRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}