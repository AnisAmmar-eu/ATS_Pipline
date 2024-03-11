using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Shared.Data;

namespace Core.Entities.Alarms.Data;

public static class AlarmInitializer
{
	public static void InitializeStation(AnodeCTX anodeCTX)
	{
		if (anodeCTX.AlarmC.Any())
			return;

		anodeCTX.Add(new AlarmC
			{
				RID = "101",
				Name = "ZT01_ALM_01",
				Description = "Invalid or missing ZT01 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "102",
				Name = "ZT02_ALM_01",
				Description = "Invalid or missing ZT02 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "103",
				Name = "ZT03_ALM_01",
				Description = "Invalid or missing ZT03 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "104",
				Name = "ZT04_ALM_01",
				Description = "Invalid or missing ZT04 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "105",
				Name = "AT01_ALM_01",
				Description = "Invalid or missing AT01 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "106",
				Name = "AT02_ALM_01",
				Description = "Invalid or missing AT02 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "107",
				Name = "TT01_ALM_01",
				Description = "Invalid or missing TT01 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "108",
				Name = "TT02_ALM_01",
				Description = "Invalid or missing TT02 analog input signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "121",
				Name = "TSH01_ALM_01",
				Description = "High temperature in electrical box",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "PROCESS",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "122",
				Name = "PW_F_ALM_01",
				Description = "POWER FAILURE",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "GENERAL",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "123",
				Name = "PSL01_ALM_01",
				Description = "COMPRESSED AIR INLET LOW PRESSURE",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "PROCESS",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "1001",
				Name = "ZT01_ZT02_ALM_02",
				Description = "INVALID “Anode detected but not D20 or DX type",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "PROCESS",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "1002",
				Name = "ZT01_ZT02_ALM_03",
				Description = "Anode type mismatch between EGA process and laser calculation",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "PROCESS",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "1011",
				Name = "TT01_ALM_02",
				Description = "Health_Col_TT01 Limit - Ambient",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "1012",
				Name = "TT02_ALM_02",
				Description = "Health_Col_TT02 Limit - Anode Surface",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "MEASURE",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "2011",
				Name = "CAM01_LED_ON",
				Description = "LedBar ON Diagnostic failed",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "2012",
				Name = "CAM01_LED_OFF",
				Description = "LedBar OFF Diagnostic failed",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "2021",
				Name = "CAM02_LED_ON",
				Description = "LedBar ON Diagnostic failed",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "2022",
				Name = "CAM02_LED_OFF",
				Description = "LedBar OFF Diagnostic failed",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "3000",
				Name = "PLC_TT_SER",
				Description = "Timestamp synchronisation failed : Between station and Server",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "GENERAL",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "3001",
				Name = "PLC_RW_COM",
				Description = "Communication central PLC 4630-PLC-001 Failed",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "GENERAL",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "3002",
				Name = "PLC_PP_COM",
				Description = "Communication PROCESS PLC - PLC_COM_P",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "GENERAL",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "3003",
				Name = "PLC_KILN1_COM",
				Description = "Communication PROCESS PLC - PLC_COM_KILN1",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "GENERAL",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "3004",
				Name = "PLC_KILN2_COM",
				Description = "Communication PROCESS PLC - PLC_COM_KILN2",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "GENERAL",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "3005",
				Name = "PLC_IO_COM",
				Description = "PLC communication with IO Cards",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "GENERAL",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "4000",
				Name = "SYNC_INDEX",
				Description = "Synchronisation Index Alarm",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "PROCESS",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "4012",
				Name = "CAM01_02",
				Description = "Invalid or missing CAMXX heart bit signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "4013",
				Name = "CAM01_03",
				Description = "High temperature",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "4022",
				Name = "CAM02_02",
				Description = "Invalid or missing CAMXX heart bit signal",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "4023",
				Name = "CAM02_03",
				Description = "High temperature",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "VISION",
			});
		anodeCTX.Add(new AlarmC
			{
				RID = "5000",
				Name = "TEST_MODE",
				Description = "Test Mode Activated",
				TS = DateTimeOffset.Now,
				Severity = 1,
				Category = "PROCESS",
			});

		anodeCTX.SaveChanges();
	}
}