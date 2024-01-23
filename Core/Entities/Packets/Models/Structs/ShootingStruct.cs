using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct ShootingStruct : IBaseADS<Packet>
{
	public RIDStruct CycleRID;
	public int Status;
	public TimestampStruct TS;
	public int SyncIndex;
	public int AnodeType;
	public int AnodeSize;
	public int Cam01Status;
	public int Cam02Status;
	public float Cam01Temp;
	public float Cam02Temp;
	public float TT01;

	public Packet ToModel()
	{
		return new Shooting(this);
	}
}