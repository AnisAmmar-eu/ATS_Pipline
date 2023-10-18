using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB;

public partial class IOTDevice : BaseEntity, IBaseEntity<IOTDevice, DTOIOTDevice>
{
	public override DTOIOTDevice ToDTO()
	{
		return new DTOIOTDevice(this);
	}

	public virtual async Task<bool> CheckConnection()
	{
		return await Task.FromResult(true);
	}

	public virtual async Task ApplyTags(IAnodeUOW anodeUOW)
	{
		await Task.CompletedTask;
	}
}