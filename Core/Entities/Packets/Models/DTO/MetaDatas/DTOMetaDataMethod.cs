using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.MetaDatas;

namespace Core.Entities.Packets.Models.DTO.MetaDatas;

public partial class DTOMetaData
{
	public DTOMetaData()
	{
		Type = PacketTypes.MetaData;
	}

	public DTOMetaData(MetaData metaData) : base(metaData)
	{
		Type = PacketTypes.MetaData;

		TS = metaData.TS;
		StationCycleRID = metaData.StationCycleRID;
		TwinCatStatus = metaData.TwinCatStatus;
		SyncIndex = metaData.SyncIndex;
		AnodeSize = metaData.AnodeSize;
		AnodeType = metaData.AnodeType;
		SyncIndex_RW = metaData.SyncIndex_RW;
		Double_RW = metaData.Double_RW;
		AnodeType_RW = metaData.AnodeType_RW;
		Trolley = metaData.Trolley;
		AnodeTypeStatus = metaData.AnodeTypeStatus;
		Cam01Status = metaData.Cam01Status;
		Cam02Status = metaData.Cam02Status;
		Cam01Temp = metaData.Cam01Temp;
		Cam02Temp = metaData.Cam02Temp;
		TT01 = metaData.TT01;

		SN_StationID = metaData.SN_StationID;
		SN_Year = metaData.SN_Year;
		SN_Month = metaData.SN_Month;
		SN_Day = metaData.SN_Day;
		SN_Vibro = metaData.SN_Vibro;
		SN_Number = metaData.SN_Number;
	}

	public override MetaData ToModel()
	{
		return new(this);
	}
}