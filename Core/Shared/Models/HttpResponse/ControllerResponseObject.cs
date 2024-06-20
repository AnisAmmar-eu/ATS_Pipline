using System.Text.Json;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Core.Shared.Models.HttpResponse;

public class ControllerResponseObject
{
	public ControllerResponseObject()
	{
		Status = new() { Code = 200 };
	}

	public ControllerResponseObject(object? result)
	{
		Result = result;
		Status = new() { Code = 200 };
	}

	public ControllerResponseObject(string message)
	{
		Status = new() { Code = 200, Message = message };
	}

	public ControllerResponseObject(string message, object? result)
	{
		Result = result;
		Status = new() { Code = 200, Message = message };
	}

	public ControllerResponseObject(int code)
	{
		Status = new() { Code = code };
	}

	public ControllerResponseObject(int code, object? result)
	{
		Result = result;
		Status = new() { Code = code };
	}

	public ControllerResponseObject(int code, string message)
	{
		Status = new() { Code = code, Message = message };
	}

	public ControllerResponseObject(int code, string message, object? result)
	{
		Result = result;
		Status = new() { Code = code, Message = message };
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
		CreateLog();
		return new(this) { StatusCode = Status.Code };
	}

	public BadRequestObjectResult BadRequestResult()
	{
		Status.Code = 400;
		return new(this);
	}

	public ObjectResult ErrorResult() => new(this) { StatusCode = Status.Code };

	public ObjectResult ErrorResult(Exception e)
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

		CreateLog();

		return new(Result) { StatusCode = Status.Code };
	}

	private void CreateLog()
	{
		try
		{
			string result = JsonSerializer.Serialize(Result);
			result = result[..((result.Length > 50) ? 50 : result.Length)];

			if (Status.Code == 200)
				Serilog.Log.Information("[{code}]: {response}", Status.Code, result);
			else if (Status.Code == 404)
				Serilog.Log.Warning("[{code}]: {response}", Status.Code, result);
			else
				Serilog.Log.Error("[{code}]: {response}", Status.Code, result);
		}
		catch
		{
			Serilog.Log.Error("[{code}]: Error during response log.", Status.Code);
		}
	}
}