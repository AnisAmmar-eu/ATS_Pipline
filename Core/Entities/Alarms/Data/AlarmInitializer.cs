using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Shared.Data;

namespace Core.Entities.Alarms.Data;

public static class AlarmInitializer
{
	public static void InitializeStation(AnodeCTX anodeCTX)
	{
		if (anodeCTX.AlarmC.Any())
			return;

		#region Communication

		anodeCTX.Add(new AlarmC {
			RID = "101",
			Name = "VA_ALM.ZT01_ALM_01",
			Description = "Communication problem with ZT01 laser",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});
		anodeCTX.Add(new AlarmC {
			RID = "102",
			Name = "VA_ALM.ZT02_ALM_01",
			Description = "Communication problem with ZT02 laser",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});
		anodeCTX.Add(new AlarmC {
			RID = "103",
			Name = "VA_ALM.ZT03_ALM_01",
			Description = "Communication problem with ZT03 laser",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});
		anodeCTX.Add(new AlarmC {
			RID = "104",
			Name = "VA_ALM.ZT04_AML_01",
			Description = "Communication problem with ZT04 laser",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});
		anodeCTX.Add(new AlarmC {
			RID = "105",
			Name = "VA_ALM.AT01_ALM_01",
			Description = "Communication problem with AT01 Light Sensor",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});
		anodeCTX.Add(new AlarmC {
			RID = "106",
			Name = "VA_ALM.AT02_ALM_01",
			Description = "Communication problem with AT02 Light Sensor",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});
		anodeCTX.Add(new AlarmC {
			RID = "107",
			Name = "VA_ALM.TT01_ALM_01",
			Description = "Communication problem with TT01 Temperature Sensor",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});
		anodeCTX.Add(new AlarmC {
			RID = "108",
			Name = "VA_ALM.TT02_ALM_01",
			Description = "Communication problem with TT02 Temperature Sensor",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = "Communication",
		});

		#endregion

		anodeCTX.Add(new AlarmC {
			RID = "121",
			Name = "VA_ALM.TSH01_ALM_01",
			Description = "High temperature in electrical box",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "122",
			Name = "VA_ALM.PW_F_ALM_01",
			Description = "POWER FAILURE",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "123",
			Name = "VA_ALM.PSL01_ALM_01",
			Description = "COMPRESSED AIR INLET LOW PRESSURE",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});

		anodeCTX.Add(new AlarmC {
			RID = "1001",
			Name = "VA_ALM.ZT01_ZT02_ALM_02",
			Description = "Anode detected but not D20 or DX type",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "1002",
			Name = "VA_ALM.ZT01_ZT02_ALM_03",
			Description = "Anode type detected not coherent",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});

		anodeCTX.Add(new AlarmC {
			RID = "1011",
			Name = "VA_ALM.TT01_ALM_02",
			Description = "Health Procedure S5 failed",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "1012",
			Name = "VA_ALM.TT02_ALM_02",
			Description = "Emergency mode activated for Health Procedure S5",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});

		anodeCTX.Add(new AlarmC {
			RID = "2011",
			Name = "CAM01_LED_ON",
			Description = "LED bars dysfunction",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "2012",
			Name = "CAM01_LED_OFF",
			Description = "Station lighting problem",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "2021",
			Name = "CAM02_LED_ON",
			Description = "LED bars dysfunction",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "2022",
			Name = "CAM02_LED_OFF",
			Description = "Station lighting problem",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});

		anodeCTX.Add(new AlarmC {
			RID = "3001",
			Name = "PLC_RW_COM",
			Description = "The communication with ATS central PLC fail",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "3002",
			Name = "PLC_PP_COM",
			Description = "The communication with PROCESS PLC fail",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "3003",
			Name = "PLC_KILN1_COM",
			Description = "The communication with PROCESS PLC fail",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "3004",
			Name = "PLC_KILN2_COM",
			Description = "The communication with PROCESS PLC fail",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});

		anodeCTX.Add(new AlarmC {
			RID = "4000",
			Name = "SYNC_INDEX",
			Description = "Synchronisation Index Alarm",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "4001",
			Name = string.Empty,
			Description = "Connection with the camera failed",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "4002",
			Name = string.Empty,
			Description = "Connection with the camera failed",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "4012",
			Name = "Cam01_Status",
			Description = "Communication problem with Camera 01",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "4013",
			Name = "Cam01_Temp_Status",
			Description = "High temperature",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "4022",
			Name = "Cam02_Status",
			Description = "Communication problem with Camera XX",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});
		anodeCTX.Add(new AlarmC {
			RID = "4023",
			Name = "Cam02_Temp_Status",
			Description = "High temperature",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});

		anodeCTX.Add(new AlarmC {
			RID = "5000",
			Name = "TEST_MODE",
			Description = "Test mode",
			TS = DateTimeOffset.Now,
			Severity = 1,
			Category = string.Empty,
		});

		anodeCTX.SaveChanges();
	}
}