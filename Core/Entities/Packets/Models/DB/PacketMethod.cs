using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB;

public partial class Packet : BaseEntity, IBaseEntity<Packet, DTOPacket>
{
	public Packet()
	{
		Type = "";
		StationCycleRID = "";
		Status = PacketStatus.Initialized;
	}

	public Packet(DTOPacket dto)
	{
		StationCycleRID = dto.StationCycleRID;
		Status = dto.Status;
		Type = dto.Type;
		HasError = dto.HasError;
	}

	public override DTOPacket ToDTO()
	{
		return new DTOPacket(this);
	}

	public virtual async Task Create(IAnodeUOW anodeUOW)
	{
		await anodeUOW.Packet.Add(this);
		anodeUOW.Commit();
	}

	public async Task<DTOPacket> Build(IAnodeUOW anodeUOW, DTOPacket dtoPacket)
	{
		dtoPacket = await InheritedBuild(anodeUOW, dtoPacket);
		anodeUOW.Packet.Update(this);
		anodeUOW.Commit();
		return dtoPacket;
	}

	protected virtual Task<DTOPacket> InheritedBuild(IAnodeUOW anodeUOW, DTOPacket dtoPacket)
	{
		return Task.FromResult(dtoPacket);
	}
}