using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.TwinCat;
using Core.Shared.UnitOfWork.Interfaces;
using Org.BouncyCastle.Security;
using TwinCAT;
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
		try
		{
			AdsClient tcClient = TwinCatConnectionManager.Connect(int.Parse(Address));
			uint varHandle = tcClient.CreateVariableHandle(ConnectionPath);
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

	public override async Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW)
	{
		CancellationToken cancel = CancellationToken.None;
		AdsClient tcClient;
		try
		{
			tcClient = TwinCatConnectionManager.Connect(int.Parse(Address));
		}
		catch (AdsException)
		{
			return new List<IOTTag>(); // The TwinCat will be marked as disconnected at next monitoring.
		}

		List<IOTTag> updatedTags = new();
		foreach (IOTTag iotTag in IOTTags)
		{
			if (!(iotTag is OTTagTwinCat otTagTwinCat))
				continue;
			uint varHandle = tcClient.CreateVariableHandle(iotTag.Path);
			if (iotTag is { IsReadOnly: false, HasNewValue: true })
			{
				ResultWrite resultWrite = await WriteFromType(tcClient, varHandle, cancel, otTagTwinCat);
				if (resultWrite.ErrorCode != AdsErrorCode.NoError)
					return new List<IOTTag>(); // Same as above
				iotTag.HasNewValue = false;
				iotTag.CurrentValue = iotTag.NewValue;
				updatedTags.Add(iotTag);
			}
			else
			{
				// Else updates the current value bc it might have changed since.
				ResultValue<object> resultRead = await ReadFromType(tcClient, varHandle, cancel, otTagTwinCat);
				if (resultRead.ErrorCode != AdsErrorCode.NoError)
					return new List<IOTTag>(); // Same as above
				if (iotTag.CurrentValue != resultRead.Value?.ToString())
					updatedTags.Add(iotTag);
				iotTag.CurrentValue = resultRead.Value?.ToString()!;
			}
		}

		return updatedTags;
	}

	private async Task<ResultValue<object>> ReadFromType(AdsClient tcClient, uint varHandle, CancellationToken cancel,
		OTTagTwinCat tag)
	{
		return new ResultValue<object>(tag.ValueType switch
		{
			_ when tag.ValueType == IOTTagType.Int => await tcClient.ReadAnyAsync<int>(varHandle, cancel),
			_ when tag.ValueType == IOTTagType.UShort => await tcClient.ReadAnyAsync<ushort>(varHandle, cancel),
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
			_ when tag.ValueType == IOTTagType.UShort => await tcClient.WriteAnyAsync(varHandle,
				ushort.Parse(tag.NewValue), cancel),
			_ when tag.ValueType == IOTTagType.String => await tcClient.WriteAnyAsync(varHandle, tag.NewValue, cancel),
			_ when tag.ValueType == IOTTagType.Bool => await tcClient.WriteAnyAsync(varHandle, bool.Parse(tag.NewValue),
				cancel),
			_ => throw new InvalidParameterException(Name + " tag has an invalid tag.ValueType for TwinCat")
		};
	}
}