using Core.Entities.Packets.Models.DB.Shootings.S5Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings.S5Shootings;

public partial class DTOS5Shooting : DTOShooting, IDTO<S5Shooting, DTOS5Shooting>
{
	public bool IsDoubleAnode { get; set; } // TODO DoubleAnodeDetection
}