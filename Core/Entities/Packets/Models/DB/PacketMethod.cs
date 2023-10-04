using Core.Entities.Packets.Dictionary;
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
		CycleStationRID = "";
		Status = PacketStatus.Initialised;
	}

	public Packet(DTOPacket dto)
	{
		ID = dto.ID;
		TS = (DateTimeOffset)dto.TS!;
		CycleStationRID = dto.CycleStationRID;
		Status = dto.Status;
		Type = dto.Type;
	}

	public override DTOPacket ToDTO()
	{
		return new DTOPacket(this);
	}

	public virtual async Task Create(IAlarmUOW alarmUOW)
	{
		await alarmUOW.Packet.Add(this);
		alarmUOW.Commit();
	}

	public async Task<DTOPacket> Build(IAlarmUOW alarmUOW, DTOPacket dtoPacket)
	{
		dtoPacket = await InheritedBuild(alarmUOW, dtoPacket);
		alarmUOW.Packet.Update(this);
		alarmUOW.Commit();
		return dtoPacket;
	}

	protected virtual Task<DTOPacket> InheritedBuild(IAlarmUOW alarmUOW, DTOPacket dtoPacket)
	{
		return Task.FromResult(dtoPacket);
	}
}