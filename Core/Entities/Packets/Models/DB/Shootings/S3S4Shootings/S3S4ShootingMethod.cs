using Core.Entities.Packets.Models.DTO.Shootings.S3S4Shootings;
using Core.Entities.Packets.Models.Structs;

namespace Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings;

public partial class S3S4Shooting
{
	public S3S4Shooting()
	{
	}

	public S3S4Shooting(DTOS3S4Shooting dtos3S4Shooting) : base(dtos3S4Shooting)
	{
		IsDoubleAnode = dtos3S4Shooting.IsDoubleAnode;
	}

	public override DTOS3S4Shooting ToDTO()
	{
		return new(this);
	}
}