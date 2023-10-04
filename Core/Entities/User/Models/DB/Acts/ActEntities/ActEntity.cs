using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.User.Models.DB.Acts.ActEntities;

public partial class ActEntity : BaseEntity, IBaseEntity<ActEntity, DTOActEntity>
{
	public string RID { get; set; }
	public int ActID { get; set; }
	public int? ParentID { get; set; }
	public int? EntityID { get; set; }
	public string? SignatureType { get; set; }

	#region Nav Properties

	private Act? _act;

	public Act Act
	{
		set => _act = value;
		get => _act
		       ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Act));
	}

	public ICollection<ActEntityRole> ActEntityRoles { get; set; } = new List<ActEntityRole>();

	#endregion
}