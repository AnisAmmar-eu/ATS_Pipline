using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Models.DTO;

public partial class DTOTenBestMatch : DTOBaseEntity, IDTO<TenBestMatch, DTOTenBestMatch>
{
	public int Rank { get; set; }
	public string AnodeID { get; set; } = string.Empty;
	public int Score { get; set; }

	public int KPIID { get; set; }
}