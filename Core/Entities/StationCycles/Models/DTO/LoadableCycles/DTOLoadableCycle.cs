using System.Text.Json.Serialization;
using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.LoadableCycles;

[JsonDerivedType(typeof(DTOS1S2Cycle))]
public partial class DTOLoadableCycle : DTOStationCycle, IDTO<LoadableCycle, DTOLoadableCycle>;