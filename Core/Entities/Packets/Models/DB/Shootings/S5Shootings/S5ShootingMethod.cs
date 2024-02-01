using Core.Entities.Packets.Models.DTO.Shootings.S5Shootings;
using Core.Entities.Packets.Models.Structs;

namespace Core.Entities.Packets.Models.DB.Shootings.S5Shootings;

public partial class S5Shooting
{
	public S5Shooting()
	{
	}

	public S5Shooting(DTOS5Shooting dtos5Shooting) : base(dtos5Shooting)
	{
		IsDoubleAnode = dtos5Shooting.IsDoubleAnode;
	}

	public S5Shooting(ShootingStruct adsStruct) : base(adsStruct)
	{
	}

	public override DTOS5Shooting ToDTO()
	{
		return new(this);
	}
}