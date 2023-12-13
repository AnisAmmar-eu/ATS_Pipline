using Core.Entities.Packets.Models.DTO;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB;

public partial class Packet
{
	public Packet()
	{
	}

	public Packet(DTOPacket dto)
	{
		StationCycleRID = dto.StationCycleRID;
		Status = dto.Status;
		HasError = dto.HasError;
	}

	public override DTOPacket ToDTO()
	{
		return new(this);
	}

	public async Task Create(IAnodeUOW anodeUOW)
	{
		await anodeUOW.Packet.Add(this);
		anodeUOW.Commit();
	}

	public async Task Build(IAnodeUOW anodeUOW)
	{
		await InheritedBuild(anodeUOW);
		anodeUOW.Packet.Update(this);
		anodeUOW.Commit();
	}

	protected virtual Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		return Task.CompletedTask;
	}
}