using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.MetaDatas;

public partial class DTOMetaData : DTOPacket, IDTO<MetaData, DTOMetaData>
{
	public int SyncIndex;
	public float AnodeSize;
	public int AnodeType;
	public int SyncIndex_RW;
	public int Double_RW;
	public int AnodeType_RW;
	public int Trolley;
	public int AnodeTypeStatus;
	public int Cam01Status;
	public int Cam02Status;
	public float Cam01Temp;
	public float Cam02Temp;
	public float TT01;

	public int SN_StationID;
	public int SN_Year;
	public int SN_Month;
	public int SN_Day;
	public int SN_Vibro;
	public int SN_Number;
}