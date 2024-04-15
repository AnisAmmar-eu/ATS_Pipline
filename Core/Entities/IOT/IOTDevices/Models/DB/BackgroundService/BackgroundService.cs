namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices;

public partial class BackgroundService: IOTDevice
{
	public int StationID { get; set; }
	public string AnodeType {  get; set; }
	public DateTimeOffset WatchdogTime { get; set; }
}