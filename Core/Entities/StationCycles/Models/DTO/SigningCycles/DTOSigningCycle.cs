using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.SigningCycles;

public partial class DTOSigningCycle : DTOStationCycle, IDTO<LoadableCycle, DTOSigningCycle>;