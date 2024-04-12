namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Match;

public partial class Match : BackgroundService
{
	public string Family { get; set; }
	public int InstanceMatchID { get; set; }
}