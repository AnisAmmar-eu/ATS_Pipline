using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Parameters.CameraParams.Models.DB;

public partial class CameraParam : BaseEntity, IBaseEntity<CameraParam, DTOCameraParam>
{
	public override DTOCameraParam ToDTO()
	{
		return new DTOCameraParam(this);
	}
}