using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.KPIData.KPIs.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPIData.KPIs.Services;

public class KPIService :
	BaseEntityService<IKPIRepository, KPI, DTOKPI>,
	IKPIService
{
	public KPIService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}