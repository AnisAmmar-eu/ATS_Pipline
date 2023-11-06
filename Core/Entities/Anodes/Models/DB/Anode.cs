using Core.Entities.Anodes.Models.DTO;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB;

public abstract partial class Anode : BaseEntity, IBaseEntity<Anode, DTOAnode>
{
	public string S1S2CycleRID { get; set; } = string.Empty;
	public string Status { get; set; } = PacketStatus.Initialized;
	public DateTimeOffset? ClosedTS { get; set; }
	
	public int S1S2CycleID { get; set; }
	public int? S3S4CycleID { get; set; }
	
	public S3S4Cycle? S3S4Cycle { get; set; }

	#region NavProperties

	private S1S2Cycle? _s1S2Cycle;
	
	public S1S2Cycle S1S2Cycle
	{
		set => _s1S2Cycle = value;
		get => _s1S2Cycle ?? throw new InvalidOperationException("Uninitialized property: " + nameof(S1S2Cycle));
	}
	#endregion
}