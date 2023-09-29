using System.Runtime.InteropServices;

namespace Core.Entities.Packets.Models.Structs;

public struct DetectionStruct // TODO Not present in the Variables list.
{
	public uint CycleStationRID;
	public uint MeasuredType;
	[MarshalAs(UnmanagedType.U1)]
	public bool IsMismatched;
	public int AnodeSize;
}