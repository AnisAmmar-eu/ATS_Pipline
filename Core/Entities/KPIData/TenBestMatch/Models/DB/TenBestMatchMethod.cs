using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Mapster;

namespace Core.Entities.KPIData.TenBestMatchs.Models.DB;

public partial class TenBestMatch
{
	public TenBestMatch()
	{
	}

	public override DTOTenBestMatch ToDTO()
	{
		return this.Adapt<DTOTenBestMatch>();
	}
}