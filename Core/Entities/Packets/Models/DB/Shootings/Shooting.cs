using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting : Packet, IBaseEntity<Shooting, DTOShooting>
{
	public DateTimeOffset ShootingTS { get; set; }
	public int SyncIndex { get; set; }
	public string AnodeType { get; set; } = string.Empty;
	public int AnodeSize { get; set; }
	public int Cam01Status { get; set; }
	public bool HasFirstShoot { get; set; }
	public int Cam02Status { get; set; }
	public bool HasSecondShoot { get; set; }

	// These 2 variables are only used when a shooting packet is retrieved from ADS to verify if there is an image.
	// because the context does not allow to use configuration.
	[NotMapped]
	public string ImagesPath { get; set; } = string.Empty;

	[NotMapped]
	public string Extension { get; set; } = string.Empty;
}