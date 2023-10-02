using Core.Shared.Models.DB.Kernel;

namespace Core.Shared.Models.DTO.Kernel;

public class DTOBaseChoice : DTOBaseEntity
{
	public string RID { get; set; } = "";
	public string? Name { get; set; }
	public string? Description { get; set; }

	public override BaseEntity ToModel()
	{
		return base.ToModel();
	}
}