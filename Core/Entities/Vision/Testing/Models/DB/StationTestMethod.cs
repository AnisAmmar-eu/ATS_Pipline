using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Vision.Testing.Models.DTO;
using Core.Shared.Dictionaries;
using Mapster;

namespace Core.Entities.Vision.Testing.Models.DB;

public partial class StationTest
{
	public StationTest()
	{
	}

	public override DTOStationTest ToDTO() => this.Adapt<DTOStationTest>();

	public MetaData MapMetadata(MetaData metadata)
	{
		metadata.StationCycleRID = $"{StationID:0}_{ShootingTS.ToString(AnodeFormat.RIDFormat)}";
		metadata.SN_StationID = this.StationID;
		metadata.AnodeTypeStatus = this.AnodeType;
		metadata.Cam01Status = this.Cam1Status;
		metadata.Cam02Status = this.Cam2Status;
		metadata.SN_Number = this.SN_number;
		metadata.TS = this.TS;
		metadata.NbActiveAlarms = this.NbActiveAlarms;
		return metadata;
	}

	public Shooting MapShooting(Shooting shooting)
	{
		shooting.StationCycleRID = $"{StationID:0}_{ShootingTS.ToString(AnodeFormat.RIDFormat)}";
		shooting.ShootingTS = ShootingTS;
		shooting.AnodeType = AnodeTypeDict.AnodeTypeIntToString(AnodeType);
		shooting.Cam01Status = Cam1Status;
		shooting.Cam02Status = Cam2Status;
		shooting.TS = TS;
		shooting.HasPlug = this.HasPlug;
		return shooting;
	}
}