using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.User.Models.DB.Acts.ActEntities
{
	public partial class ActEntity : BaseEntity, IBaseEntity<ActEntity, DTOActEntity>
	{
		public ActEntity()
		{
			RID = "";
		}
		public ActEntity(DTOActEntity dto)
		{
			RID = dto.RID == "" ? dto.Act?.RID + '.' + dto.EntityID : dto.RID;
			ParentID = dto.ParentID;
			EntityID = dto.EntityID;
		}
		public ActEntity(DTOActEntity dto, Act act, string signatureType)
		{
			RID = dto.RID == "" ? act.RID + '.' + dto.EntityID : dto.RID;
			ParentID = dto.ParentID;
			EntityID = dto.EntityID;
			SignatureType = signatureType;
			Act = act;
		}

		public override DTOActEntity ToDTO()
		{
			return new DTOActEntity(this);
		}
	}
}
