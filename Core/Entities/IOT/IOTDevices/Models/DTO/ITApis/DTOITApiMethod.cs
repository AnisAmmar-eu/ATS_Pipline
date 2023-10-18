using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;

public partial class DTOITApi : DTOIOTDevice, IDTO<ITApi, DTOITApi>
{
	public DTOITApi()
	{
	}

	public DTOITApi(ITApi itApi) : base(itApi)
	{
	}
}