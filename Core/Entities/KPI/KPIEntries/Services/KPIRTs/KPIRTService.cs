using System.Linq.Expressions;
using Core.Entities.KPI.KPICs.Services;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;
using Core.Entities.KPI.KPIEntries.Services.KPILogs;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPIRTs;

public class KPIRTService : ServiceBaseEntity<IKPIRTRepository, KPIRT, DTOKPIRT>, IKPIRTService
{
	private IKPILogService _kpiLogService;

	public KPIRTService(IAlarmUOW alarmUOW, IKPILogService kpiLogService) : base(alarmUOW)
	{
		_kpiLogService = kpiLogService;
	}

	public async Task<List<DTOKPILog>> SaveRTsToLogs(List<string> periodsToSave)
	{
		List<KPIRT> kpiRTs = await AlarmUOW.KPIRT.GetAll(new Expression<Func<KPIRT, bool>>[]
		{
			kpiRT => periodsToSave.Contains(kpiRT.Period)
		}, null, false, null);
		List<KPILog> kpiLogs = new();
		foreach (KPIRT kpiRT in kpiRTs)
			kpiLogs.Add(kpiRT.ToLog(await AlarmUOW.KPIC.GetById(kpiRT.KPICID)));	

		return await _kpiLogService.AddAll(kpiLogs);
	}
}