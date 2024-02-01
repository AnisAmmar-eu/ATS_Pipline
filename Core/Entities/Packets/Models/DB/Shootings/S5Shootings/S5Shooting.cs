using Core.Entities.Packets.Models.DTO.Shootings.S5Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings.S5Shootings;

public partial class S5Shooting : Shooting, IBaseEntity<S5Shooting, DTOS5Shooting>
{
	public bool IsDoubleAnode { get; set; } // TODO DoubleAnodeDetection
}