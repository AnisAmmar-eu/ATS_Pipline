using Core.Entities.Alarms.AlarmsPLC.Models.DTO;

namespace Core.Entities.Alarms.AlarmsPLC.Models.DB;

public partial class AlarmPLC
{
	public AlarmPLC() : base()
	{}

	public AlarmPLC(DTOAlarmPLC dtoAlarmPLC) : base(dtoAlarmPLC)
	{
		ID = dtoAlarmPLC.ID;
		TS = (DateTimeOffset)dtoAlarmPLC.TS!;
		AlarmID = dtoAlarmPLC.AlarmID;
		IsActive = dtoAlarmPLC.IsActive;
		IsOneShot = dtoAlarmPLC.IsOneShot;
	}
	public override DTOAlarmPLC ToDTO()
	{
		return new DTOAlarmPLC(this);
	}
}