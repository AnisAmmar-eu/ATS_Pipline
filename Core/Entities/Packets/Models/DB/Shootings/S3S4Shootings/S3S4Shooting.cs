using Core.Entities.Packets.Models.DTO.Shootings.S3S4Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings;

public partial class S3S4Shooting : Shooting, IBaseEntity<S3S4Shooting, DTOS3S4Shooting>
{
	public bool IsDoubleAnode { get; set; } // TODO DoubleAnodeDetection
}