using System.Runtime.InteropServices;

namespace Core.Entities.Packets.Models.Structs;

public struct RIDStruct
{
	public uint StationID { get; set; }
	public TimestampStruct TS { get; set; }

	public string ToRID()
	{
		return StationID.ToString() + TS.GetTimestamp().ToUnixTimeMilliseconds().ToString();
	}
}