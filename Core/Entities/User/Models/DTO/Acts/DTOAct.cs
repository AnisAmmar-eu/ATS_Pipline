using Core.Entities.User.Models.DB.Acts;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.User.Models.DTO.Acts;

public partial class DTOAct : DTOBaseEntity, IDTO<Act, DTOAct>
{
	public string RID { get; set; }
	public string? EntityType { get; set; }
	public string? ParentType { get; set; }
}