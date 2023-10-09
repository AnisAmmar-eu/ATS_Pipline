using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Parameters.CameraParams.Repositories;

public interface ICameraParamRepository : IRepositoryBaseEntity<CameraParam, DTOCameraParam>
{
	
}