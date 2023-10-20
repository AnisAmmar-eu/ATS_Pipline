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

	/// <summary>
	///     Will apply all the tags of the device which need to be updated.
	/// </summary>
	/// <param name="anodeUOW"></param>
	/// <returns>True if at least one tag is applied. False otherwise.</returns>
	public virtual async Task<bool> ApplyTags(IAnodeUOW anodeUOW)
	{
		return await Task.FromResult(false);
	}
}