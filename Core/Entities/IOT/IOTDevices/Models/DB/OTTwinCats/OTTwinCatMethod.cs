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
			try
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
				// Nullable warning ignored because ReadFromType always returns a ResultValue
				object? readValue = await ReadFromType(tcClient, varHandle, otTagTwinCat, cancel);

				hasBeenUpdated = hasBeenUpdated || iotTag.CurrentValue != readValue?.ToString();
				if (hasBeenUpdated)
					updatedTags.Add(iotTag);

				iotTag.CurrentValue = readValue?.ToString()!;
			}
			catch
			{
				// ignored
			}
		}

		return updatedTags;
	}

	/// <summary>
	///		Reads according to the given typ
	/// </summary>
	/// <param name="tcClient"></param>
	/// <param name="varHandle"></param>
	/// <param name="tag"></param>
	/// <param name="cancel"></param>
	/// <returns>The value is returned as an object because ResultValue<int> is not convertible to ResultValue<object> etc...</returns>
	/// <exception cref="InvalidOperationException"></exception>
	private Task<object?> ReadFromType(
		AdsClient tcClient,
		uint varHandle,
		OTTagTwinCat tag,
		CancellationToken cancel)
	{
		return tag.ValueType switch {
			IOTTagType.Int => ReadAndCheck<int>(tcClient, varHandle, tag, cancel),
			IOTTagType.UInt => ReadAndCheck<uint>(tcClient, varHandle, tag, cancel),
			IOTTagType.UShort => ReadAndCheck<ushort>(tcClient, varHandle, tag, cancel),
			IOTTagType.String => ReadAndCheck<string>(tcClient, varHandle, tag, cancel),
			IOTTagType.Bool => ReadAndCheck<bool>(tcClient, varHandle, tag, cancel),
			_ => throw new InvalidOperationException(Name + " tag has an invalid type for TwinCat"),
		};
	}

	private static async Task<object?> ReadAndCheck<T>
		(AdsClient tcClient, uint varHandle, OTTagTwinCat tag, CancellationToken cancel)
	{
		ResultValue<T> resultValue = await tcClient.ReadAnyAsync<T>(varHandle, cancel);
		if (resultValue.ErrorCode != AdsErrorCode.Succeeded)
			throw new AdsException($"Could not read {tag.RID}: {resultValue.ErrorCode.ToString()}");

		return resultValue.Value;
	}

	private async Task<ResultWrite> WriteFromType(
		AdsClient tcClient,
		uint varHandle,
		OTTagTwinCat tag,
		CancellationToken cancel)
	{
		return tag.ValueType switch {
			IOTTagType.Int => await tcClient.WriteAnyAsync(varHandle, int.Parse(tag.NewValue), cancel),
			IOTTagType.UInt => await tcClient.WriteAnyAsync(varHandle, uint.Parse(tag.NewValue), cancel),
			IOTTagType.UShort => await tcClient.WriteAnyAsync(varHandle, ushort.Parse(tag.NewValue), cancel),
			IOTTagType.String => await tcClient.WriteAnyAsync(varHandle, tag.NewValue, cancel),
			IOTTagType.Bool => await tcClient.WriteAnyAsync(varHandle, bool.Parse(tag.NewValue), cancel),
			_ => throw new InvalidOperationException(Name + " tag has an invalid tag.ValueType for TwinCat"),
		};
	}
}