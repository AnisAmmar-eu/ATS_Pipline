using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting : Packet, IBaseEntity<Shooting, DTOShooting>
{
	public Shooting(ShootingStruct adsStruct)
	{
		// TODO
		// CycleStationRID = adsStruct.CycleStationRID;
		AnodeIDKey = (int)adsStruct.AnodeIDKey;
		GlobalStationStatus = adsStruct.GlobalStationStatus;
		ProcedurePerformance = (int)adsStruct.ProcedurePerformance;
		LedStatus = adsStruct.LedStatus;
		// TODO
		// ShootingTS = adsStruct.ShootingTS;
	}

	public override DTOShooting ToDTO(string? languageRID = null)
	{
		return new DTOShooting(this);
	}
}