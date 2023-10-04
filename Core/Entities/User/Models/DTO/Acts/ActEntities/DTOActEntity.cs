using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DTO.Acts.ActEntities.ActEntityRoles;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.User.Models.DTO.Acts.ActEntities;

public partial class DTOActEntity : DTOBaseEntity, IDTO<ActEntity, DTOActEntity>
{
	public string RID { get; set; }
	public int? EntityID { get; set; }
	public int? ParentID { get; set; }
	public string? SignatureType { get; set; }
	public DTOAct? Act { get; set; }

	public List<DTOActEntityRole> Applications { get; set; } = new();
	// public DTOLogin Login { get; set; }
}