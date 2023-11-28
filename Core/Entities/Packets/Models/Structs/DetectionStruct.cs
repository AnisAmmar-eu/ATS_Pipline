using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct DetectionStruct : IBaseADS<Detection>
{
	public RIDStruct StationCycleRID;
	public ushort AnodeHigh; // This is LaserAnodeSize
	public bool AnodeNotAnnounced;

	public Detection ToModel()
	{
		return new Detection(this);
	}
}