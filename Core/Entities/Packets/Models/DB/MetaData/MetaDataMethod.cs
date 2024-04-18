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

	/// <summary>
	/// Generates a serial number based on the provided parameters.
	/// </summary>
	/// <returns>The generated serial number.</returns>
	public string GetSerialNumber()
	{
		/*
			Décomposition: 4-A-12-1-5698"
			"1 Digit : SN_Year
			Dernier chiffre de l'année ( 4 = 2024 par exemple)

			"1 Digit: SN_Month
			valeur entière comprise entre 1 et 12 correspondant
			au mois de l'année à associer ensuite à une lettre-caractère allant de A-M = 1-12
			la lettre I est exclue"
			A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8, J = 9, K = 10, L = 11, M = 12

			"2 Digits max - entier: SN_Day
			à intégrer sous forme de 2 digit avec les zéros non significatifs"

			"1 Digit : SN_Vibro
			Entier égal soit à 1, soit à 2 (deux maeurs possibles)
			identifiant du Vibro"
			
			"4 digits: SN_Number
			numéro de série
			à intégrer sous forme de 4 digit avec les zéros non significatifs"
		*/

		string _SN_Year = (SN_Year % 10).ToString();
		string[] months = ["A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M"];
		string _SN_Month = months[SN_Month - 1];
		string _SN_Day = SN_Day.ToString().PadLeft(2, '0');
		string _SN_Vibro = SN_Vibro.ToString();
		string _SN_Number = SN_Number.ToString().PadLeft(4, '0');

		return $"{_SN_Year}-{_SN_Month}-{_SN_Day}-{_SN_Vibro}-{_SN_Number}";
	}
}