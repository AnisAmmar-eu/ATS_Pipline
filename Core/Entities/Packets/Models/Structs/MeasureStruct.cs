using System.Runtime.InteropServices;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct MeasureStruct
{
	public RIDStruct RID;

	public ushort AnodeType;
	public bool IsSameType;
	public ushort AnodeLength;
}