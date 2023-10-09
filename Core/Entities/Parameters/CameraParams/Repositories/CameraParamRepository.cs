using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Parameters.CameraParams.Repositories;

public class CameraParamRepository : RepositoryBaseEntity<AnodeCTX, CameraParam, DTOCameraParam>, ICameraParamRepository
{
	public CameraParamRepository(AnodeCTX context) : base(context)
	{
	}
}