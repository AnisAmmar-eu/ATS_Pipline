using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Matchs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.IOT.IOTDevices.Repositories;

public class MatchRepository : BaseEntityRepository<AnodeCTX, Match, DTOMatch>, IMatchRepository
{
	public MatchRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}