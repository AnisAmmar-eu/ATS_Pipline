using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB;

public partial class IOTDevice
{
	protected IOTDevice()
	{}
	
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
	public virtual async Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW)
	{
		return await Task.FromResult(new List<IOTTag>());
	}
}