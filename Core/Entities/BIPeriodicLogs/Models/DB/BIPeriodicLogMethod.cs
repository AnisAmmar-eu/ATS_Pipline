using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB;

// BI = Business Intelligence
public partial class BIPeriodicLog : BaseEntity, IBaseEntity<BIPeriodicLog, DTOBIPeriodicLog>
{
	public BIPeriodicLog(DTOBIPeriodicLog dtoBIPeriodicLog)
	{
		ID = dtoBIPeriodicLog.ID;
		TS = (DateTimeOffset)dtoBIPeriodicLog.TS!;
		NbMatched = dtoBIPeriodicLog.NbMatched;
		NbSigned = dtoBIPeriodicLog.NbSigned;
		NbUnsigned = dtoBIPeriodicLog.NbUnsigned;

		Cam1Matched = dtoBIPeriodicLog.Cam1Matched;
		Cam2Matched = dtoBIPeriodicLog.Cam2Matched;

		InactiveAlarms = dtoBIPeriodicLog.InactiveAlarms;
		NonAckAlarms = dtoBIPeriodicLog.NonAckAlarms;
		ActiveAlarms = dtoBIPeriodicLog.ActiveAlarms;

		NbAnodeS1 = dtoBIPeriodicLog.NbAnodeS1;
		NbAnodeS2 = dtoBIPeriodicLog.NbAnodeS2;
		NbAnodeS3 = dtoBIPeriodicLog.NbAnodeS3;
		NbAnodeS4 = dtoBIPeriodicLog.NbAnodeS4;
		NbAnodeS5 = dtoBIPeriodicLog.NbAnodeS5;
	}

	public async Task Create(IAlarmUOW alarmUOW)
	{
		await alarmUOW.BIPeriodicLog.Add(this);
		alarmUOW.Commit();
	}

	public async Task<DTOBIPeriodicLog> Build(IAlarmUOW alarmUOW, DTOBIPeriodicLog dtobiPeriodicLog)
	{
		dtobiPeriodicLog = await InheritedBuild(alarmUOW, dtobiPeriodicLog);
		alarmUOW.BIPeriodicLog.Update(this);
		alarmUOW.Commit();
		return dtobiPeriodicLog;
	}

	protected virtual Task<DTOBIPeriodicLog> InheritedBuild(IAlarmUOW alarmUOW, DTOBIPeriodicLog dtobiPeriodicLog)
	{
		return Task.FromResult(dtobiPeriodicLog);
	}
	public override DTOBIPeriodicLog ToDTO()
	{
		return new DTOBIPeriodicLog(this);
	}
}