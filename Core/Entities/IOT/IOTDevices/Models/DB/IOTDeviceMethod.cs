using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Entities.IOT.IOTDevices.Models.DB;

public partial class IOTDevice
{
	protected IOTDevice()
	{
	}

	public IOTDevice(DTOIOTDevice dtoIOTDevice) : base(dtoIOTDevice)
	{
		ID = dtoIOTDevice.ID;
		TS = (DateTimeOffset)dtoIOTDevice.TS!;
		RID = dtoIOTDevice.RID;
		Name = dtoIOTDevice.Name;
		Description = dtoIOTDevice.Description;
		Address = dtoIOTDevice.Address;
		IsConnected = dtoIOTDevice.IsConnected;
		IOTTags = dtoIOTDevice.IOTTags.ConvertAll(tag => tag.ToModel());
	}

	public override DTOIOTDevice ToDTO()
	{
		return new(this);
	}

	public virtual Task<bool> CheckConnection(ILogger logger)
	{
		return Task.FromResult(true);
	}

    /// <summary>
    ///     Will apply all the tags of the device which need to be updated.
    /// </summary>
    /// <param name="anodeUOW"></param>
    /// <param name="logger"></param>
    /// <returns>True if at least one tag is applied. False otherwise.</returns>
    public virtual Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW, ILogger logger)
	{
		return Task.FromResult(new List<IOTTag>());
	}
}