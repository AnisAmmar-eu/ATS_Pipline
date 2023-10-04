using Core.Entities.User.Models.DTO.Acts;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.User.Models.DB.Acts
{
	public partial class Act : BaseEntity, IBaseEntity<Act, DTOAct>
	{
		public Act()
		{
			RID = "";
		}
		public Act(string rid, string? entityType = null, string? parentType = null)
		{
			RID = rid;
			EntityType = entityType;
			ParentType = parentType;
		}
		public Act(DTOAct dto)
		{
			RID = dto.RID;
			EntityType = dto.EntityType;
			ParentType = dto.ParentType;
		}

		public override DTOAct ToDTO()
		{
			return new DTOAct(this);
		}
	}
}
