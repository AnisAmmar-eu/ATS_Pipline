using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;
using Stemmer.Cvb;

namespace Core.Entities.Parameters.CameraParams.Services;

public interface ICameraParamService : IServiceBaseEntity<CameraParam, DTOCameraParam>
{
	public DTOCameraParam GetDeviceParameters(Device device);
	public void SetDeviceParameters(Device device, DTOCameraParam dtoCameraParam);
	public Task RunAcquisitionAsync(Device device, DTOCameraParam dtoCameraParam, string extension);
}