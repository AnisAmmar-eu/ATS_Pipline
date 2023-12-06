using System.Text;
using System.Text.Json;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Furnaces;
using Core.Entities.StationCycles.Models.DB.MatchingCycles.S3S4Cycles;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces;

public abstract partial class Furnace
{
	protected Furnace()
	{
	}

	protected Furnace(DTOFurnace dtoFurnace) : base(dtoFurnace)
	{
	}

	public override DTOFurnace ToDTO()
	{
		return new(this);
	}

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		StationCycle = await anodeUOW.StationCycle
			.GetBy([cycle => cycle.RID == StationCycleRID], withTracking: false);
		if (StationCycle is not S3S4Cycle s3S4Cycle)
			throw new InvalidOperationException("Furnace packet associated with a non S3S4Cycle");

		if (s3S4Cycle.Status == PacketStatus.Sent)
		{
			using HttpClient httpClient = new();
			StringContent content = new(JsonSerializer.Serialize(ToDTO()), Encoding.UTF8, "application/json");
			HttpResponseMessage response
				= await httpClient.PostAsync($"{Station.ServerAddress}/apiServerReceive/furnacePackets", content);
			if (response.IsSuccessStatusCode)
				Status = PacketStatus.Sent;
		}

		FurnaceAssign(s3S4Cycle);
		anodeUOW.StationCycle.Update(s3S4Cycle);
	}

	protected virtual void FurnaceAssign(S3S4Cycle cycle)
	{
	}
}