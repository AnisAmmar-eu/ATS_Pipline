using Core.Shared.Models.DB.Kernel;

namespace Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;

public partial class ActEntityRole : BaseEntity
{
	public string RID { get; set; }
	public int ActEntityID { get; set; }
	public string? Level { get; set; }

	public string ApplicationID { get; set; }
	public string ApplicationType { get; set; }

	#region Nav Properties

	private ActEntity? _actEntity;

	public ActEntity ActEntity
	{
		set => _actEntity = value;
		get => _actEntity
			?? throw new InvalidOperationException("Uninitialized property: " + nameof(ActEntity));
	}

	#endregion Nav Properties
}