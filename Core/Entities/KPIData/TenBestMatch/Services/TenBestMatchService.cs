using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Entities.KPIData.TenBestMatchs.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Services;

public class TenBestMatchService :
	BaseEntityService<ITenBestMatchRepository, TenBestMatch, DTOTenBestMatch>,
	ITenBestMatchService
{
	public TenBestMatchService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}