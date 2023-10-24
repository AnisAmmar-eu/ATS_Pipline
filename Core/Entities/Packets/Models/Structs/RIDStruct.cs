using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Core.Shared.Dictionaries;
using Microsoft.VisualBasic;

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