using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.Vision.Testing.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.Testing.Models.DTO;

public partial class DTOStationTest : DTOBaseEntity, IDTO<StationTest, DTOStationTest>
{
	public int StationID { get; set; }
	public int AnodeType { get; set; }
	public int SN_number { get; set; }
	public string Photo1 { get; set; }
	public string Photo2 { get; set; }
	public DateTimeOffset? ShootingTS { get; set; }
	public SignMatchStatus Cam1 { get; set; }
	public SignMatchStatus Cam2 { get; set; }
	public int NbActiveAlarms { get; set; }
	public bool HasPlug { get; set; }
}