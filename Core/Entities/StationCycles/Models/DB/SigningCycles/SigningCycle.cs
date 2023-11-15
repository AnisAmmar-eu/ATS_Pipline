using Core.Entities.StationCycles.Models.DTO.SigningCycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.SigningCycles;

public partial class SigningCycle : StationCycle, IBaseEntity<SigningCycle, DTOSigningCycle>
{
}