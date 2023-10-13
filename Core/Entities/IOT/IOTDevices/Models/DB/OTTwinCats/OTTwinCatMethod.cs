using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;
using TwinCAT.Ads;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;

public partial class OTTwinCat : IOTDevice, IBaseEntity<OTTwinCat, DTOOTTwinCat>
{
	public override DTOOTTwinCat ToDTO()
	{
		return new DTOOTTwinCat(this);
	}

	public override async Task<bool> CheckConnection()
	{
		CancellationToken cancel = CancellationToken.None;
		IOTTag tag = IOTTags.Find(tag => tag.Name == IOTTagNames.CheckConnectionName)
		             ?? throw new InvalidOperationException("Cannot find Connection tag for " + Name + " device.");
		AdsClient tcClient = new();
		try
		{
			tcClient.Connect(int.Parse(Address));
			if (!tcClient.IsConnected)
				throw new Exception();
			uint varHandle = tcClient.CreateVariableHandle(tag.Path);
			ResultValue<int> resultRead = await tcClient.ReadAnyAsync<int>(varHandle, cancel);
			if (resultRead.ErrorCode != AdsErrorCode.NoError)
				throw new Exception();
		}
		catch (Exception)
		{
			return await Task.FromResult(false);
		}

		return await Task.FromResult(true);
	}

	public override async Task ApplyTags(IAnodeUOW anodeUOW)
	{
		CancellationToken cancel = CancellationToken.None;
		AdsClient tcClient = new();
		tcClient.Connect(int.Parse(Address));
		if (!tcClient.IsConnected)
			return; // The TwinCat will be marked as disconnected at next monitoring.
		foreach (IOTTag tag in IOTTags)
		{
			uint varHandle = tcClient.CreateVariableHandle(tag.Path);
			ResultValue<int> resultRead = await tcClient.ReadAnyAsync<int>(varHandle, cancel);
			if (resultRead.ErrorCode != AdsErrorCode.NoError)
				return; // Same as above
			tag.CurrentValue = resultRead.Value.ToString();
			if (tag.HasNewValue)
			{
				ResultWrite resultWrite = await tcClient.WriteAnyAsync(varHandle, int.Parse(tag.NewValue), cancel);
				if (resultWrite.ErrorCode != AdsErrorCode.NoError)
					return; // Same as above
				tag.HasNewValue = false;
				tag.CurrentValue = tag.NewValue;
			}

			anodeUOW.IOTTag.Update(tag);
			anodeUOW.Commit();
		}
	}
}