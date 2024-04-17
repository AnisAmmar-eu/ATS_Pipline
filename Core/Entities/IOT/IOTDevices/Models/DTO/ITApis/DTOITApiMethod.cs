using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;

public partial class DTOITApi
{
	public DTOITApi()
	{
	}

	public DTOITApi(ITApi itApi) : base(itApi)
	{
	}

	public override ITApi ToModel() => new(this);
}