using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct DetectionStruct : IBaseADS<Packet, DetectionStruct> // TODO Not present in the Variables list.
{
	public RIDStruct StationCycleRID;
	public uint AnodeHigh; // This is LaserAnodeSize

	public Packet ToModel()
	{
		return new Detection(this);
	}
}