using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;
using Org.BouncyCastle.Security;
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

	public override async Task<bool> ApplyTags(IAnodeUOW anodeUOW)
	{
		CancellationToken cancel = CancellationToken.None;
		AdsClient tcClient = new();
		tcClient.Connect(int.Parse(Address));
		if (!tcClient.IsConnected)
			return false; // The TwinCat will be marked as disconnected at next monitoring.
		bool hasAnyTagChanged = false;
		foreach (IOTTag tag in IOTTags)
		{
			if (!(tag is OTTagTwinCat otTagTwinCat))
				continue;
			uint varHandle = tcClient.CreateVariableHandle(tag.Path);
			ResultValue<object> resultRead = await ReadFromType(tcClient, varHandle, cancel, otTagTwinCat);
			if (resultRead.ErrorCode != AdsErrorCode.NoError)
				return false; // Same as above
			hasAnyTagChanged = hasAnyTagChanged || tag.CurrentValue != resultRead.Value?.ToString();
			tag.CurrentValue = resultRead.Value?.ToString()!;
			if (tag.HasNewValue)
			{
				ResultWrite resultWrite = await WriteFromType(tcClient, varHandle, cancel, otTagTwinCat);
				if (resultWrite.ErrorCode != AdsErrorCode.NoError)
					return false; // Same as above
				tag.HasNewValue = false;
				tag.CurrentValue = tag.NewValue;
				hasAnyTagChanged = true;
			}

			anodeUOW.IOTTag.Update(tag);
			anodeUOW.Commit();
		}

		return hasAnyTagChanged;
	}

	private async Task<ResultValue<object>> ReadFromType(AdsClient tcClient, uint varHandle, CancellationToken cancel,
		OTTagTwinCat tag)
	{
		return new ResultValue<object>(tag.ValueType switch
		{
			_ when tag.ValueType == IOTTagType.Int => await tcClient.ReadAnyAsync<int>(varHandle, cancel),
			_ when tag.ValueType == IOTTagType.String => await tcClient.ReadAnyAsync<string>(varHandle, cancel),
			_ when tag.ValueType == IOTTagType.Bool => await tcClient.ReadAnyAsync<bool>(varHandle, cancel),
			_ => throw new InvalidParameterException(Name + " tag has an invalid type for TwinCat")
		});
	}

	private async Task<ResultWrite> WriteFromType(AdsClient tcClient, uint varHandle, CancellationToken cancel,
		OTTagTwinCat tag)
	{
		return tag.ValueType switch
		{
			_ when tag.ValueType == IOTTagType.Int => await tcClient.WriteAnyAsync(varHandle, int.Parse(tag.NewValue),
				cancel),
			_ when tag.ValueType == IOTTagType.String => await tcClient.WriteAnyAsync(varHandle, tag.NewValue, cancel),
			_ when tag.ValueType == IOTTagType.Bool => await tcClient.WriteAnyAsync(varHandle, bool.Parse(tag.NewValue),
				cancel),
			_ => throw new InvalidParameterException(Name + " tag has an invalid tag.ValueType for TwinCat")
		};
	}
}