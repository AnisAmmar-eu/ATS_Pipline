using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB;

public partial class Packet
{
	public Packet()
	{
	}

	public Packet(DTOPacket dto) : base(dto)
	{
		StationCycleRID = dto.StationCycleRID;
		Status = dto.Status;
		TwinCatStatus = dto.TwinCatStatus;
		HasError = dto.HasError;
	}

	public override DTOPacket ToDTO() => new(this);

	public async Task Create(IAnodeUOW anodeUOW)
	{
		await anodeUOW.Packet.Add(this);
		anodeUOW.Commit();
	}

	public async Task Build(IAnodeUOW anodeUOW)
	{
		await InheritedBuild(anodeUOW);
		Status = PacketStatus.Completed;
		anodeUOW.Packet.Update(this);
		anodeUOW.Commit();
	}

	protected virtual Task InheritedBuild(IAnodeUOW anodeUOW) => Task.CompletedTask;
}