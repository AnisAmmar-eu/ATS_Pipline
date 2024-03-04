using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;
/*
TS 				: DB_TS;
		CycleRID		: DB_PXX_CycleRID;
		Status			: DINT;
		SyncIndex		: DINT;
		AnodeSize		: REAL;
		AnodeType		: DINT;
		// from Central PLC Announce
		SyncIndex_RW	: DINT;
		Double_RW		: DINT;
		AnodeType_RW	: DINT;
		SN				: DB_PXX_SN;
		Trolley			: DINT;
		// from Station
		AnodeTypeStatus	: DINT;
		Cam01Status		: DINT;
		Cam02Status		: DINT;
		Cam01Temp		: REAL;
		Cam02Temp		: REAL;
		TT01			: REAL;
*/
[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct MetaDataStruct : IBaseADS<Packet>
{
	public TimestampStruct TS;
	public RIDStruct CycleRID;
	public int Status;
	public int SyncIndex;
	public float AnodeSize;
	public int AnodeType;
	public int SyncIndex_RW;
	public int Double_RW;
	public int AnodeType_RW;
	public SNStruct SN;
	public int Trolley;
	public int AnodeTypeStatus;
	public int Cam01Status;
	public int Cam02Status;
	public float Cam01Temp;
	public float Cam02Temp;
	public float TT01;

	public Packet ToModel()
	{
		return new MetaData(this);
	}
}

/*
StationID	: DINT; 

Year		: DINT; 

Month		: DINT; 

Day		: DINT; 

Vibro		: DINT; 

Number		: DINT; 
*/
[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct SNStruct
{
	public int StationID;
	public int Year;
	public int Month;
	public int Day;
	public int Vibro;
	public int Number;
}