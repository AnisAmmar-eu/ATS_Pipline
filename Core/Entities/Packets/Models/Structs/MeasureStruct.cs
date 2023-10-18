namespace Core.Entities.Packets.Models.Structs;

public struct MeasureStruct
{
	public RIDStruct RID;

	public ushort AnodeType;
	public bool IsSameType;
	public ushort AnodeLength;
}