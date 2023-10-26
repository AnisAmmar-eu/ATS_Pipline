using System.Runtime.InteropServices;
using Core.Shared.Dictionaries;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct RIDStruct
{
	public ushort StationID { get; set; }
	public TimestampStruct TS { get; set; }

	public string ToRID()
	{
		return $"{StationID}_{TS.GetTimestamp().ToString(ADSUtils.TSFormat)}";
	}
}