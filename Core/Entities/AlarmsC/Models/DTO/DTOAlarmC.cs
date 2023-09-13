using System.Collections;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.Journals.Models.DTO;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using DTOAlarmRT = Core.Entities.AlarmsRT.Models.DTO.DTOAlarmRT;

namespace Core.Entities.AlarmsC.Models.DTO;

public partial class DTOAlarmC : DTOBaseEntity, IDTO<AlarmC, DTOAlarmC>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public virtual ICollection<DTOJournal>? Journals { get; set; }
	public virtual DTOAlarmRT? AlarmRT { get; set; }
};