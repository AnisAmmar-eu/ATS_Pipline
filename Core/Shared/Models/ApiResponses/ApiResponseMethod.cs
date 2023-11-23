using System.Net;
using System.Reflection;
using System.Text.Json;
using Core.Shared.Exceptions;
using Core.Shared.Models.ApiResponses.ApiStatuses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;

namespace Core.Shared.Models.ApiResponses;

public partial class ApiResponse
{
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = null,
	};
	
	// With default statusCode
	public ApiResponse()
	{
		Status = new ApiStatus { Code = 200 };
	}

	public ApiResponse(object? result)
	{
		Result = result;
		Status = new ApiStatus { Code = 200 };
	}

	public ApiResponse(string message)
	{
		Status = new ApiStatus { Code = 200, Message = message };
	}

	public ApiResponse(string message, object? result)
	{
		Result = result;
		Status = new ApiStatus { Code = 200, Message = message };
	}

	// With statusCode
	public ApiResponse(int code)
	{
		Status = new ApiStatus { Code = code };
	}

	public ApiResponse(int code, object? result)
	{
		Result = result;
		Status = new ApiStatus { Code = code };
	}

	public ApiResponse(int code, string message)
	{
		Status = new ApiStatus { Code = code, Message = message };
	}

	public ApiResponse(int code, string message, object? result)
	{
		Result = result;
		Status = new ApiStatus { Code = code, Message = message };
	}

	public JsonHttpResult<ApiResponse> SuccessResult()
	{
		Status.Code = 200;
		return TypedResults.Json(this, JsonOptions);
	}


	public JsonHttpResult<ApiResponse> ErrorResult()
	{
		return TypedResults.Json(this, statusCode: Status.Code, options: JsonOptions);
	}

	public async Task<JsonHttpResult<ApiResponse>> SuccessResult(ILogService logService, Endpoint? endpoint)
	{
		await CreateLog(logService, endpoint);
		return TypedResults.Json(this, JsonOptions);
	}

	public async Task<JsonHttpResult<ApiResponse>> ErrorResult(ILogService logService, Endpoint? endpoint, Exception e)
	{
		switch (e)
		{
			case ArgumentException:
				Status.Code = 400;
				Result = e.Message;
				break;
			case UnauthorizedAccessException:
				Status.Code = 401;
				Result = "Unauthorized action: " + e.Message;
				break;
			case EntityNotFoundException:
				Status.Code = 404;
				Result = e.Message;
				break;
			case TimeoutException:
				Status.Code = 408;
				Result = "Request timeout: " + e.Message;
				break;
			case InvalidOperationException:
				Status.Code = 409;
				Result = "Request cause a conflict: " + e.Message;
				break;
			case NotImplementedException:
				Status.Code = 501;
				Result = "Not implemented request: " + e.Message;
				break;
			default:
				Status.Code = 500;
				Result = "Internal Server Error: " + e.Message;
				break;
		}

		await CreateLog(logService, endpoint);

		return TypedResults.Json(this, statusCode: Status.Code, options: JsonOptions);
	}

	private async Task CreateLog(ILogService logService, Endpoint? endpoint)
	{
		if (endpoint == null)
			throw new ArgumentException("Logs creation, endpoint is null");
		MethodInfo methodInfo = endpoint.Metadata.GetRequiredMetadata<MethodInfo>();
		try
		{
			await logService.Create(
				DateTimeOffset.Now,
				Dns.GetHostName(),
				methodInfo.Module.Name,
				methodInfo.ReflectedType?.Name ?? string.Empty,
				methodInfo.Name,
				endpoint.Metadata.GetRequiredMetadata<IRouteDiagnosticsMetadata>().Route,
				Status.Code,
				JsonSerializer.Serialize(Result)
			);
		}
		catch
		{
			// ignored
		}
	}
}