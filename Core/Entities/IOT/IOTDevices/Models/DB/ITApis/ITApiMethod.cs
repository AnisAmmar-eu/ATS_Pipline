using System.Net;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.TwinCat;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ITApis;

public partial class ITApi
{
	public ITApi()
	{
	}

	public ITApi(DTOITApi dtoITApi) : base(dtoITApi)
	{
	}

	public override DTOITApi ToDTO() => new(this);

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		using HttpClient httpClient = new();
		CancellationTokenSource cancelSource = new();
		bool isConnected;
		try
		{
			HttpResponseMessage response = await httpClient.GetAsync(Address + ConnectionPath);
			isConnected = response.StatusCode == HttpStatusCode.OK;
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while trying to connect to {name}:\n{error}", RID, e);
			isConnected = false;
		}

		if (!Station.IsServer && RID == ITApisDict.ServerReceiveRID)
		{
			AdsClient tcClient = await TwinCatConnectionManager.Connect(ADSUtils.AdsPort, logger, 1000, cancelSource.Token);
			uint statusHandle
				= tcClient.CreateVariableHandle(IOTTagPath.ServerStatusWrite);
			tcClient.WriteAny(statusHandle, isConnected);
		}

		return isConnected;
	}

	public override Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW, ILogger logger)
	{
		return Task.FromResult(IOTTags.Where(tag => tag.HasNewValue)
			.Select(tag => {
				tag.CurrentValue = tag.NewValue;
				tag.HasNewValue = false;
				return tag;
			})
			.ToList());
	}
}