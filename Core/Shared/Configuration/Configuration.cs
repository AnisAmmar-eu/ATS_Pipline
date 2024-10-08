using System.Configuration;
using Core.Entities.IOT.Dictionaries;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;

namespace Core.Shared.Configuration;

/// <summary>
/// Provides helper functions for easier configuration managing.
/// Those functions automatically throw if the configuration is not found thus returning a non-nullable value.
/// </summary>
public static class Configuration
{
	public static T GetValueWithThrow<T>(this IConfiguration configuration, string path)
		=> configuration.GetValue<T>(path) ?? throw new ConfigurationErrorsException($"Missing {path}");

	public static T GetSectionWithThrow<T>(this IConfiguration configuration, string path)
		=> configuration.GetSection(path).Get<T>() ?? throw new ConfigurationErrorsException($"Missing {path}");

	public static string GetConnectionStringWithThrow(this IConfiguration configuration, string path)
		=> configuration.GetConnectionString(path) ?? throw new ConfigurationErrorsException($"Missing {path}");

	/// <summary>
	/// Allows to load config common to every Api upon startup
	/// </summary>
	/// <param name="configuration"></param>
	public static void LoadBaseConfiguration(this IConfiguration configuration)
	{
		Station.Name = configuration.GetValueWithThrow<string>(ConfigDictionary.StationName);
		Server.WatchdogDelay = TimeSpan.FromSeconds(configuration.GetValueWithThrow<int>(ConfigDictionary.WatchdogDelay));

		// TODO IF station is server then different config
		ITApisDict.ADSAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiADSAddress);
		ITApisDict.AlarmAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiAlarmAddress);
		ITApisDict.CameraAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiCameraAddress);
		ITApisDict.IOTAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiIOTAddress);
		ITApisDict.ServerReceiveAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiServerReceiveAddress);
		ITApisDict.StationCycleAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiStationCycleAddress);
		ITApisDict.UserAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiUserAddress);
		ITApisDict.VisionAddress = configuration.GetValueWithThrow<string>(ConfigDictionary.ApiVisionAddress);
	}
}