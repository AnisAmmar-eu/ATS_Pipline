using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Shared.Models.TwinCat;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using TwinCAT;
using TwinCAT.Ads;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;

public partial class OTTwinCat
{
	public OTTwinCat()
	{
	}

	public OTTwinCat(DTOOTTwinCat dtoOTTwinCat) : base(dtoOTTwinCat)
	{
	}

	public override DTOOTTwinCat ToDTO()
	{
		return new(this);
	}

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		CancellationToken cancel = CancellationToken.None;
		try
		{
			AdsClient tcClient = TwinCatConnectionManager.Connect(int.Parse(Address));
			uint varHandle = tcClient.CreateVariableHandle(ConnectionPath);
			ResultValue<int> resultRead = await tcClient.ReadAnyAsync<int>(varHandle, cancel);
			if (resultRead.ErrorCode != AdsErrorCode.NoError)
				throw new();
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while monitoring TwinCat:\n {error}", e);
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
			return []; // The TwinCat will be marked as disconnected at next monitoring.
		}

		List<IOTTag> updatedTags = [];
		foreach (IOTTag iotTag in IOTTags)
		{
			bool hasBeenUpdated = false;
			if (iotTag is not OTTagTwinCat otTagTwinCat)
				continue;

			uint varHandle = tcClient.CreateVariableHandle(iotTag.Path);
			if (iotTag is { IsReadOnly: false, HasNewValue: true })
			{
				ResultWrite resultWrite = await WriteFromType(tcClient, varHandle, otTagTwinCat, cancel);
				if (resultWrite.ErrorCode != AdsErrorCode.NoError)
					return []; // Same as above

				iotTag.HasNewValue = false;
				hasBeenUpdated = true;
			}

			// Else updates the current value bc it might have changed since.
			ResultValue<object> resultRead = await ReadFromType(tcClient, varHandle, otTagTwinCat, cancel);
			if (resultRead.ErrorCode != AdsErrorCode.NoError)
				return []; // Same as above

			hasBeenUpdated = hasBeenUpdated || iotTag.CurrentValue != resultRead.Value?.ToString();
			if (hasBeenUpdated)
				updatedTags.Add(iotTag);

			iotTag.CurrentValue = resultRead.Value?.ToString()!;
		}

		return updatedTags;
	}

	private async Task<ResultValue<object>> ReadFromType(
		AdsClient tcClient,
		uint varHandle,
		OTTagTwinCat tag,
		CancellationToken cancel)
	{
		return new ResultValue<object>(tag.ValueType switch {
			IOTTagType.Int => await tcClient.ReadAnyAsync<int>(varHandle, cancel),
			IOTTagType.UShort => await tcClient.ReadAnyAsync<ushort>(varHandle, cancel),
			IOTTagType.String => await tcClient.ReadAnyAsync<string>(varHandle, cancel),
			IOTTagType.Bool => await tcClient.ReadAnyAsync<bool>(varHandle, cancel),
			_ => throw new InvalidOperationException(Name + " tag has an invalid type for TwinCat"),
		});
	}

	private async Task<ResultWrite> WriteFromType(
		AdsClient tcClient,
		uint varHandle,
		OTTagTwinCat tag,
		CancellationToken cancel)
	{
		return tag.ValueType switch {
			IOTTagType.Int => await tcClient.WriteAnyAsync(varHandle, int.Parse(tag.NewValue), cancel),
			IOTTagType.UShort => await tcClient.WriteAnyAsync( varHandle, ushort.Parse(tag.NewValue), cancel),
			IOTTagType.String => await tcClient.WriteAnyAsync(varHandle, tag.NewValue, cancel),
			IOTTagType.Bool => await tcClient.WriteAnyAsync(varHandle, bool.Parse(tag.NewValue), cancel),
			_ => throw new InvalidOperationException(Name + " tag has an invalid tag.ValueType for TwinCat"),
		};
	}
}