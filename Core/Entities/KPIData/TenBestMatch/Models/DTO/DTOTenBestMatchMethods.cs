using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Mapster;

namespace Core.Entities.KPIData.TenBestMatchs.Models.DTO;

public partial class DTOTenBestMatch
{
	public DTOTenBestMatch()
	{
	}

	public override TenBestMatch ToModel() => this.Adapt<TenBestMatch>();
}