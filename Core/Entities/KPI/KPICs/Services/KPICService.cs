using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Entities.KPI.KPICs.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPI.KPICs.Services;

public class KPICService : ServiceBaseEntity<KPICRepository, KPIC, DTOKPIC>, IKPICService
{
	public KPICService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}
}