using Core.Entities.Vision.Testing.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.Testing.Models.DB;

public partial class StationTest : BaseEntity, IBaseEntity<StationTest, DTOStationTest>
{
	public int StationID { get; set; }
	public int AnodeType { get; set; }
	public int SN_number { get; set; }
	public string Photo1 { get; set; }
	public string Photo2 { get; set; }
	public DateTimeOffset ShootingTS { get; set; }
	public int Cam1Status { get; set; }
	public int Cam2Status { get; set; }
	public int NbActiveAlarms { get; set; }
	public bool HasPlug { get; set; }
}