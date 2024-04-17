using System.Runtime.InteropServices;
using Core.Shared.Dictionaries;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct RIDStruct
{
	public int StationID { get; set; }
	public TimestampStruct TS { get; set; }

	// Final RID : S_yyyyMMdd-HHmmss-fff
	// Example : 1_20241225-155623-543 -> Station 1 2024-12-25 15:56:23:543
	public string ToRID() => $"{StationID:0}_{TS.GetTimestamp().ToString(AnodeFormat.RIDFormat)}";
}