using System.Net;
using Core.Shared.Exceptions;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core.Shared.Models.HttpResponse;

public class ControllerResponseObject
{
	// With default statusCode
	public ControllerResponseObject()
	{
		Status = new ControllerStatusObject { Code = 200 };
	}

	public ControllerResponseObject(object? result)
	{
		Result = result;
		Status = new ControllerStatusObject { Code = 200 };
	}

	public ControllerResponseObject(string message)
	{
		Status = new ControllerStatusObject { Code = 200, Message = message };
	}

	public ControllerResponseObject(string message, object? result)
	{
		Result = result;
		Status = new ControllerStatusObject { Code = 200, Message = message };
	}

	// With statusCode
	public ControllerResponseObject(int code)
	{
		Status = new ControllerStatusObject { Code = code };
	}

	public ControllerResponseObject(int code, object? result)
	{
		Result = result;
		Status = new ControllerStatusObject { Code = code };
	}

	public ControllerResponseObject(int code, string message)
	{
		Status = new ControllerStatusObject { Code = code, Message = message };
	}

	public ControllerResponseObject(int code, string message, object? result)
	{
		Result = result;
		Status = new ControllerStatusObject { Code = code, Message = message };
	}

	public object? Result { get; set; }
	public ControllerStatusObject Status { get; set; }

	/*
	public JsonResult JsonResult()
	{
	    return new JsonResult(this);
	}
	*/

	public ObjectResult SuccessResult()
	{
		Status.Code = 200;
		return new ObjectResult(this)
		{
			StatusCode = Status.Code
		};
	}

	public BadRequestObjectResult BadRequestResult()
	{
		Status.Code = 400;
		return new BadRequestObjectResult(this);
	}

	public ObjectResult ErrorResult()
	{
		return new ObjectResult(this)
		{
			StatusCode = Status.Code
		};
	}

	public async Task<JsonResult> SuccessResult(ILogService logService, ControllerContext context)
	{
		await CreateLog(logService, context);

		return new JsonResult(this);
	}

	public async Task<ObjectResult> ErrorResult(ILogService logService, ControllerContext context, Exception e)
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

		await CreateLog(logService, context);

		return new ObjectResult(Result)
		{
			StatusCode = Status.Code
		};
	}

	private async Task CreateLog(ILogService logService, ControllerContext context)
	{
		try
		{
			await logService.Create(
				DateTimeOffset.Now,
				Dns.GetHostName(),
				context.ActionDescriptor.ControllerTypeInfo.Assembly.ManifestModule.Name,
				context.ActionDescriptor.ControllerTypeInfo.Name,
				context.ActionDescriptor.ActionName,
				context.ActionDescriptor.AttributeRouteInfo?.Template ?? "",
				Status.Code,
				JsonConvert.SerializeObject(Result)
			);
		}
		catch
		{
			// ignored
		}
	}
}