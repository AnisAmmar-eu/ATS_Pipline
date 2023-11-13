using Core.Entities.Packets.Models.DTO.Shootings.S3S4Shootings;
using Core.Entities.Packets.Models.Structs;

namespace Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings;

public partial class S3S4Shooting
{
	public S3S4Shooting()
	{
	}

	public S3S4Shooting(ShootingStruct adsStruct) : base(adsStruct)
	{
	}

	public override DTOS3S4Shooting ToDTO()
	{
		return new DTOS3S4Shooting(this);
	}
}