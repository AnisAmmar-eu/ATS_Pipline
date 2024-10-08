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

	public override DTOOTTwinCat ToDTO() => new(this);

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		try
		{
			CancellationTokenSource cancelSource = new();
			cancelSource.CancelAfter(200);
			AdsClient tcClient = await TwinCatConnectionManager.Connect(int.Parse(Address), logger, 1000, cancelSource.Token);
			uint varHandle = tcClient.CreateVariableHandle(ConnectionPath);
			ResultValue<bool> resultRead = await tcClient.ReadAnyAsync<bool>(varHandle, cancelSource.Token);
			if (resultRead.ErrorCode != AdsErrorCode.NoError)
				throw new();
		}
		catch (Exception e)
		{
			logger.LogError("Error while monitoring TwinCat:\n {error}", e);
			return false;
		}

		return true;
	}

	public override async Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW, ILogger logger)
	{
		CancellationTokenSource cancelSource = new();
		cancelSource.CancelAfter(800);
		AdsClient tcClient;
		try
		{
			tcClient = await TwinCatConnectionManager.Connect(int.Parse(Address), logger, 1000, cancelSource.Token);
		}
		catch (AdsException e)
		{
			logger.LogError("Error while connecting to {RID} when applying tags: {e}", RID, e);
			return []; // The TwinCat will be marked as disconnected at next monitoring.
		}

		List<IOTTag> updatedTags = [];
		foreach (IOTTag tag in IOTTags)
		{
			try
			{
				bool hasBeenUpdated = false;
				if (tag is not OTTagTwinCat otTagTwinCat)
					continue;

				uint varHandle = tcClient.CreateVariableHandle(tag.Path);
				if (tag is { IsReadOnly: false, HasNewValue: true })
				{
					ResultWrite resultWrite = await WriteFromType(tcClient, varHandle, otTagTwinCat, cancelSource.Token);
					if (resultWrite.ErrorCode != AdsErrorCode.NoError)
						continue; // Same as above

					tag.HasNewValue = false;
					hasBeenUpdated = true;
				}

				// Else updates the current value bc it might have changed since.
				// Nullable warning ignored because ReadFromType always returns a ResultValue
				object? readValue = await ReadFromType(tcClient, varHandle, otTagTwinCat, cancelSource.Token);

				hasBeenUpdated = hasBeenUpdated
					|| tag.CurrentValue != readValue?.ToString()
					|| tag.CurrentValue != tag.NewValue;
				if (hasBeenUpdated)
					updatedTags.Add(tag);

				tag.CurrentValue = readValue?.ToString()!;
				//if (!tag.HasNewValue)
				//	tag.NewValue = readValue?.ToString()!;
			}
			catch (Exception e)
			{
				logger.LogError("Error while applying tag {TagRID}: {Error}", tag.RID, e);
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
			IOTTagType.UShort => ReadAndCheck<ushort>(tcClient, varHandle, tag, cancel),
			IOTTagType.String => ReadAndCheck<string>(tcClient, varHandle, tag, cancel),
			IOTTagType.Bool => ReadAndCheck<bool>(tcClient, varHandle, tag, cancel),
			IOTTagType.Float => ReadAndCheck<float>(tcClient, varHandle, tag, cancel),
			_ => throw new InvalidOperationException(Name + " tag has an invalid type for TwinCat"),
		};
	}

	private static async Task<object?> ReadAndCheck<T>
		(AdsClient tcClient, uint varHandle, OTTagTwinCat tag, CancellationToken cancel)
	{
		ResultValue<T> resultValue = await tcClient.ReadAnyAsync<T>(varHandle, cancel);
		return (resultValue.ErrorCode != AdsErrorCode.Succeeded)
			? throw new AdsException($"Could not read {tag.RID}: {resultValue.ErrorCode.ToString()}")
			: (object?)resultValue.Value;
	}

	private async Task<ResultWrite> WriteFromType(
		AdsClient tcClient,
		uint varHandle,
		OTTagTwinCat tag,
		CancellationToken cancel)
	{
		return tag.ValueType switch {
			IOTTagType.Int => await tcClient.WriteAnyAsync(varHandle, int.Parse(tag.NewValue), cancel),
			IOTTagType.UShort => await tcClient.WriteAnyAsync(varHandle, ushort.Parse(tag.NewValue), cancel),
			IOTTagType.String => await tcClient.WriteAnyAsync(varHandle, tag.NewValue, cancel),
			IOTTagType.Bool => await tcClient.WriteAnyAsync(varHandle, bool.Parse(tag.NewValue), cancel),
			IOTTagType.Float => await tcClient.WriteAnyAsync(varHandle, float.Parse(tag.NewValue), cancel),
			_ => throw new InvalidOperationException(Name + " tag has an invalid tag.ValueType for TwinCat"),
		};
	}
}