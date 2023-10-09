using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Entities.Parameters.CameraParams.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;

namespace Core.Entities.Parameters.CameraParams.Services;

public class CameraParamService : ServiceBaseEntity<ICameraParamRepository, CameraParam, DTOCameraParam>, ICameraParamService
{
	public CameraParamService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

}