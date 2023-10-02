using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct DetectionStruct : IBaseADS<Packet, DetectionStruct> // TODO Not present in the Variables list.
{
	public uint CycleStationRID;
	public uint MeasuredType;
	[MarshalAs(UnmanagedType.U1)]
	public bool IsMismatched;
	public int AnodeSize;
	public Packet ToModel()
	{
		return new Detection(this);
	}
}