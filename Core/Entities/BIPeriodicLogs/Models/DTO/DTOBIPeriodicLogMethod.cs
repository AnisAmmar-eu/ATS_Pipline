using Core.Entities.BIPeriodicLogs.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO;

public partial class DTOBIPeriodicLog : DTOBaseEntity, IDTO<BIPeriodicLog, DTOBIPeriodicLog>
{
	public DTOBIPeriodicLog(BIPeriodicLog biPeriodicLog)
	{
		ID = biPeriodicLog.ID;
		TS = biPeriodicLog.TS;
		NbMatched = biPeriodicLog.NbMatched;
		NbSigned = biPeriodicLog.NbSigned;
		NbUnsigned = biPeriodicLog.NbUnsigned;

		Cam1Matched = biPeriodicLog.Cam1Matched;
		Cam2Matched = biPeriodicLog.Cam2Matched;

		InactiveAlarms = biPeriodicLog.InactiveAlarms;
		NonAckAlarms = biPeriodicLog.NonAckAlarms;
		ActiveAlarms = biPeriodicLog.ActiveAlarms;

		NbAnodeS1 = biPeriodicLog.NbAnodeS1;
		NbAnodeS2 = biPeriodicLog.NbAnodeS2;
		NbAnodeS3 = biPeriodicLog.NbAnodeS3;
		NbAnodeS4 = biPeriodicLog.NbAnodeS4;
		NbAnodeS5 = biPeriodicLog.NbAnodeS5;
	}

	public override BIPeriodicLog ToModel()
	{
		return new BIPeriodicLog(this);
	}
}