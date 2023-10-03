using Core.Entities.ExtTags.Models.DTO;
using Core.Entities.ServicesMonitors.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Models.DB;

public partial class ExtTag : BaseEntity, IBaseEntity<ExtTag, DTOExtTag>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	public string ValueType { get; set; }
	public string CurrentValue { get; set; }
	public string NewValue { get; set; }
	
	public bool IsReadOnly { get; set; }
	public bool HasNewValue { get; set; }
	public int ServiceID { get; set; }
	public string? Path { get; set; }
	
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