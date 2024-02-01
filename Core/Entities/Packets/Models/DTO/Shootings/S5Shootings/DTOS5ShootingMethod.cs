using Core.Entities.Packets.Models.DB.Shootings.S5Shootings;

namespace Core.Entities.Packets.Models.DTO.Shootings.S5Shootings;

public partial class DTOS5Shooting
{
	public DTOS5Shooting(S5Shooting s5Shooting) : base(s5Shooting)
	{
		IsDoubleAnode = s5Shooting.IsDoubleAnode;
	}

	public override S5Shooting ToModel()
	{
		return new(this);
	}
}