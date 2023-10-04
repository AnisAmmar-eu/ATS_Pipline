using Core.Entities.User.Models.DTO.Acts;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.User.Models.DB.Acts;

public partial class Act : BaseEntity, IBaseEntity<Act, DTOAct>
{
	public string RID { get; set; }
	public string? ParentType { get; set; }
	public string? EntityType { get; set; }
}