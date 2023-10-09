using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Parameters.CameraParams.Services;

public interface ICameraParamService : IServiceBaseEntity<CameraParam, DTOCameraParam>
{
	
}