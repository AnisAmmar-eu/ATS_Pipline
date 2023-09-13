using Core.Entities.AlarmsC.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Entities.AlarmsC.Models.DTO;

public partial class DTOAlarmC : DTOBaseEntity, IDTO<AlarmC, DTOAlarmC>
{
	public DTOAlarmC()
	{
	}

	public DTOAlarmC(AlarmC alarmC)
	{
		ID = alarmC.ID;
		TS = alarmC.TS;
		RID = alarmC.RID;
		Name = alarmC.Name;
		Description = alarmC.Description;
	}

	public override AlarmC ToModel()
	{
		return new AlarmC(this);
	}
}