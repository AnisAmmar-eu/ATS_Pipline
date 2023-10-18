using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.S3S4Cycles;

public partial class DTOS3S4Cycle : DTOStationCycle, IDTO<S3S4Cycle, DTOS3S4Cycle>
{
	public string? AnnounceID { get; set; }

	public string? InFurnaceStatus { get; set; }
	public int? InFurnaceID { get; set; }
	public DTOInFurnace? InFurnacePacket { get; set; }

	public string? OutFurnaceStatus { get; set; }
	public int? OutFurnaceID { get; set; }
	public DTOOutFurnace? OutFurnacePacket { get; set; }
}