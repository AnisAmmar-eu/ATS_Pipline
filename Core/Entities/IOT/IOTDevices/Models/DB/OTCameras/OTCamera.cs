using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Stemmer.Cvb;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;

public partial class OTCamera : IOTDevice, IBaseEntity<OTCamera, DTOOTCamera>
{
	[NotMapped] public Device _device { get; set; }
}