using Core.Entities.Packets.Models.DTO.MetaDatas;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.MetaDatas;

public partial class MetaData : Packet, IBaseEntity<MetaData, DTOMetaData>
{
	public int SyncIndex { get; set; }
	public float AnodeSize { get; set; }
	public int AnodeType_MD { get; set; }
	public int SyncIndex_RW { get; set; }
	public int Double_RW { get; set; }
	public int AnodeType_RW { get; set; }
	public int Trolley { get; set; }
	public int AnodeTypeStatus { get; set; }
	public int Cam01Status { get; set; }
	public int Cam02Status { get; set; }
	public float Cam01Temp { get; set; }
	public float Cam02Temp { get; set; }
	public float TT01 { get; set; }

	public int SN_StationID { get; set; }
	public int SN_Year { get; set; }
	public int SN_Month { get; set; }
	public int SN_Day { get; set; }
	public int SN_Vibro { get; set; }
	public int SN_Number { get; set; }
}