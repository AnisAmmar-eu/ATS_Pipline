using System.Text.Json.Serialization;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.IOT.IOTDevices.Models.DTO;

[JsonDerivedType(typeof(DTOServerRule))]
public partial class DTOIOTDevice : DTOBaseEntity, IDTO<IOTDevice, DTOIOTDevice>, IBindableFromHttpContext<DTOIOTDevice>
{
	public string Type { get; set; } = string.Empty;
	public string RID { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public bool IsConnected { get; set; }
	public List<DTOIOTTag> IOTTags { get; set; } = [];
}