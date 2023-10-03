using Core.Entities.ExtTags.Models.DTO;
using Core.Entities.ServicesMonitors.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Models.DB;

public partial class ExtTag : BaseEntity, IBaseEntity<ExtTag, DTOExtTag>
{
	public string RID;
	public string Name;
	public string Description;
	public int CurrentValue;
	public int NewValue;
	public bool IsReadOnly;
	public bool HasNewValue;
	public int ServiceID;
	public string? Path;
	
	#region Nav Properties

	private ServicesMonitor? _service;

	public ServicesMonitor Service
	{
		set => _service = value;
		get => _service
		       ?? throw new InvalidOperationException("Uninitialized property: " + nameof(ServicesMonitor));
	}

	#endregion
}