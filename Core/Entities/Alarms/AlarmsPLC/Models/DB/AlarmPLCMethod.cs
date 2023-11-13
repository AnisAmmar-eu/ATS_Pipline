using Core.Entities.Alarms.AlarmsPLC.Models.DTO;

namespace Core.Entities.Alarms.AlarmsPLC.Models.DB;

public partial class AlarmPLC
{
	public override DTOAlarmPLC ToDTO()
	{
		return new DTOAlarmPLC(this);
	}
}