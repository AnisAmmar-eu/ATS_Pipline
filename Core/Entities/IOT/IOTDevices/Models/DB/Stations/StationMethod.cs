using System.Net;
using System.Text.Json;
using Core.Entities.IOT.IOTDevices.Models.DTO.Stations;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.ApiResponses;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Entities.IOT.IOTDevices.Models.DB.Stations;

public partial class Station
{
	public Station()
	{
	}

	public Station(DTOStation dtoStation) : base(dtoStation)
	{
	}

	public override DTOStation ToDTO()
	{
		return new(this);
	}

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		using HttpClient httpClient = new();
		try
		{
			HttpResponseMessage response = await httpClient.GetAsync(Address + ConnectionPath);

            ApiResponse? apiResponse
    = JsonSerializer.Deserialize<ApiResponse>(await response.Content.ReadAsStreamAsync());
            if (apiResponse is null)
                throw new ApplicationException("Could not deserialize ApiIOT response");

            if (apiResponse.Result is not JsonElement jsonElement)
                throw new ApplicationException("JSON Exception, ApiResponse from ApiIOT is broken");

            oldestShooting = jsonElement.Deserialize<DateTimeOffset>();

            return response.StatusCode == HttpStatusCode.OK;
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while trying to connect to {name}:\n{error}", RID, e);
			return false;
		}
	}

	public override Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW, ILogger logger)
	{
		return Task.FromResult(IOTTags.Where(tag => tag.HasNewValue)
			.Select(tag =>
			{
				tag.CurrentValue = tag.NewValue;
				tag.HasNewValue = false;
				return tag;
			})
			.ToList());
	}
}