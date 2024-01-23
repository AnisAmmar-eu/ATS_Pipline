using System.Configuration;
using Core.Entities.IOT.Dictionaries;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;

namespace Core.Shared.Configuration;

public static class Configuration
{
	public static T GetValueWithThrow<T>(this IConfiguration configuration, string path)
	{
		T? res = configuration.GetValue<T>(path);
		if (res is null)
			throw new ConfigurationErrorsException($"Missing {path}");

		return res;
	}

	public static T GetSectionWithThrow<T>(this IConfiguration configuration, string path)
	{
		T? res = configuration.GetSection(path).Get<T>();
		if (res is null)
			throw new ConfigurationErrorsException($"Missing {path}");

		return res;
	}

	public static string GetConnectionStringWithThrow(this IConfiguration configuration, string path)
	{
		string? res = configuration.GetConnectionString(path);
		if (res is null)
			throw new ConfigurationErrorsException($"Missing {path}");

		return res;
	}

	public static void LoadBaseConfiguration(this IConfiguration configuration)
	{
		Station.Name = configuration.GetValueWithThrow<string>("StationConfig:StationName");

		// TODO IF station is server then different config
		ITApisDict.ADSAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiADS");
		ITApisDict.AlarmAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiAlarm");
		ITApisDict.CameraAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiCamera");
		ITApisDict.IOTAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiIOT");
		ITApisDict.KPIAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiKPI");
		ITApisDict.ServerReceiveAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiServerReceive");
		ITApisDict.StationCycleAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiStationCycle");
		ITApisDict.UserAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiUser");
		ITApisDict.VisionAddress = configuration.GetValueWithThrow<string>("ApiAddresses:ApiVision");
	}
}