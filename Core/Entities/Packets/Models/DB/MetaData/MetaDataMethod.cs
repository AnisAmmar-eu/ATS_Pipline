using Core.Entities.Packets.Models.DTO.MetaDatas;
using Core.Entities.Packets.Models.Structs;

namespace Core.Entities.Packets.Models.DB.MetaDatas;

public partial class MetaData
{
	public MetaData()
	{
	}

	public MetaData(DTOMetaData dtoMetaData) : base(dtoMetaData)
	{
		Status = dtoMetaData.Status;
		SyncIndex = dtoMetaData.SyncIndex;
		AnodeSize = dtoMetaData.AnodeSize;
		AnodeType_MD = dtoMetaData.AnodeType;
		SyncIndex_RW = dtoMetaData.SyncIndex_RW;
		Double_RW = dtoMetaData.Double_RW;
		AnodeType_RW = dtoMetaData.AnodeType_RW;
		Trolley = dtoMetaData.Trolley;
		AnodeTypeStatus = dtoMetaData.AnodeTypeStatus;
		Cam01Status = dtoMetaData.Cam01Status;
		Cam02Status = dtoMetaData.Cam02Status;
		Cam01Temp = dtoMetaData.Cam01Temp;
		Cam02Temp = dtoMetaData.Cam02Temp;
		TT01 = dtoMetaData.TT01;

		SN_StationID = dtoMetaData.SN_StationID;
		SN_Year = dtoMetaData.SN_Year;
		SN_Month = dtoMetaData.SN_Month;
		SN_Day = dtoMetaData.SN_Day;
		SN_Vibro = dtoMetaData.SN_Vibro;
		SN_Number = dtoMetaData.SN_Number;
	}

	public MetaData(MetaDataStruct adsStruct)
	{
		TS = adsStruct.TS.GetTimestamp();
		StationCycleRID = adsStruct.CycleRID.ToRID();
		TwinCatStatus = adsStruct.Status;

		SyncIndex = adsStruct.SyncIndex;
		AnodeSize = adsStruct.AnodeSize;
		AnodeType_MD = adsStruct.AnodeType;
		SyncIndex_RW = adsStruct.SyncIndex_RW;
		Double_RW = adsStruct.Double_RW;
		AnodeType_RW = adsStruct.AnodeType_RW;
		Trolley = adsStruct.Trolley;
		AnodeTypeStatus = adsStruct.AnodeTypeStatus;
		Cam01Status = adsStruct.Cam01Status;
		Cam02Status = adsStruct.Cam02Status;
		Cam01Temp = adsStruct.Cam01Temp;
		Cam02Temp = adsStruct.Cam02Temp;
		TT01 = adsStruct.TT01;

		SN_StationID = adsStruct.SN.StationID;
		SN_Year = adsStruct.SN.Year;
		SN_Month = adsStruct.SN.Month;
		SN_Day = adsStruct.SN.Day;
		SN_Vibro = adsStruct.SN.Vibro;
		SN_Number = adsStruct.SN.Number;
	}

	public override DTOMetaData ToDTO() => new(this);
}