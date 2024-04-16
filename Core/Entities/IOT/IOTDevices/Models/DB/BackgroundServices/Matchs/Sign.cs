using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Matchs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;

public partial class Match : BackgroundService, IBaseEntity<Match, DTOMatch>
{
	public string Family { get; set; }
	public int InstanceMatchID { get; set; }
}