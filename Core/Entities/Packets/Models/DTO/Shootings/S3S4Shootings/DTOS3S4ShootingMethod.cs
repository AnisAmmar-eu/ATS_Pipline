using Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings.S3S4Shootings;

public partial class DTOS3S4Shooting : DTOShooting, IDTO<S3S4Shooting, DTOS3S4Shooting>
{
	public DTOS3S4Shooting(S3S4Shooting s3S4Shooting) : base(s3S4Shooting)
	{
		IsDoubleAnode = s3S4Shooting.IsDoubleAnode;
	}
}